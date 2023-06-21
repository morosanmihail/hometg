import React, { Fragment, useState } from 'react';
import { confirm } from "./ConfirmCollectionDelete";
import { useNavigate } from "react-router-dom";
import uuid from 'react-native-uuid';

function MoveCardsNav({ collection, collections, selected, setSelected, setRefresh, operations, addOperation, removeOperation  }) {
    const navigate = useNavigate();
    const [destinationCollection, setDestinationCollection] = useState(collection);

    const deleteCards = () => {
        confirm({ confirmType: "cards", selectedCount: selected.length }).then(
            ({ input }) => {
                let opId = uuid.v4();
                addOperation(opId, { message: "Removing items from " + collection });
                fetch('/collection/cards/' + collection + '/remove', {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(selected),
                }).then(response => {
                    if (response.status === 200) {
                        response.json().then(data => {
                            setRefresh(true);
                            setSelected([]);
                        })
                    }
                    removeOperation(opId);
                });
            },
            () => {

            }
        );
    }

    const moveCards = () => {
        let opId = uuid.v4();
        addOperation(opId, { message: "Moving items between " + collection + " and " + destinationCollection })
        fetch('/collection/move/' + destinationCollection, {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(selected),
        }).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setRefresh(true);
                    setSelected([]);
                })
            }
            removeOperation(opId);
        });
    }

    const deleteCollection = () => {
        let moveToCollections = collections.filter(s => s.id !== collection);
        moveToCollections.push("");
        confirm({ confirmType: "collection", collection: collection, collections: moveToCollections }).then(
            ({ input }) => {
                let opId = uuid.v4();
                addOperation(opId, { message: "Deleting collection " + collection })
                fetch('/collection/remove/' + collection + '?keepCardsInCollection=' + input, {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(selected),
                }).then(response => {
                    if (response.status === 200) {
                        response.json().then(data => {
                            navigate('/');
                        })
                    }
                });
                removeOperation(opId);
            },
            () => {

            }
        );
    }

    return (
        <Fragment>
            <nav className="navbar navbar-dark bg-dark">
                <div className="form-row align-items-center">
                    <div className="col-auto">
                        <span className="badge badge-primary">{selected.length} selected</span>
                    </div>
                    <div className="col-auto">
                        <button onClick={deleteCards} type="button" className="btn btn-danger">🗑️</button>
                    </div>
                    <div className="col-auto">
                        <button onClick={moveCards} type="button" className="btn btn-info" alt="Move to collection">Move to</button>
                    </div>
                    <div className="col-auto">
                        <select onChange={(e) => setDestinationCollection(e.target.value)} className="form-control" id="exampleFormControlSelect1">
                            {collections.map(c =>
                                <option key={"cardlistcol-" + c.id} dropdown={c.id} value={c.id}>{c.id}</option>
                            )}
                        </select>
                    </div>
                    <div className="col-auto">
                        <button onClick={deleteCollection} type="button" className="btn btn-danger">🗑️ collection</button>
                    </div>
                    <div className="col-auto">
                        <span className="badge badge-primary">{Object.keys(operations).length} operations active</span>
                    </div>
                    {Object.entries(operations).map( ([key, o]) =>
                        <div key={key} className="col-auto">
                            <span className="badge badge-primary">{o.message}</span>
                        </div>
                    )}
                </div>
            </nav>
        </Fragment>
    );
}

export default MoveCardsNav;