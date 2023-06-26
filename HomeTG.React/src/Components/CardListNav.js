import React, { Fragment, useState } from 'react';
import { confirm } from "./ConfirmCollectionDelete";
import { useNavigate } from "react-router-dom";
import { useOperations } from '../OperationsContext';
import { useCollection } from './CollectionContext';

export default function CardListNav({ collections, selected, setSelected, setRefresh }) {
    const navigate = useNavigate();
    const collection = useCollection();
    const [destinationCollection, setDestinationCollection] = useState(collection);
    const [file, setFile] = useState();

    const ops = useOperations();

    const handleFileChange = (e) => {
        if (e.target.files) {
          setFile(e.target.files[0]);
        }
    };

    const handleUploadClick = () => {
        if (!file) {
            return;
        }

        const formData = new FormData();
        formData.append("file", file);

        ops.fetch("Importing into " + collection, [], '/collection/cards/' + collection + '/import', {
            method: "post",
            headers: {
                'Content-Type': file.type,
                'Content-Length': `${file.size}`,
            },
            body: formData,
            // body: file,
        }).then(data => {});
    }

    const deleteCards = () => {
        confirm({ confirmType: "cards", selectedCount: selected.length }).then(
            ({ input }) => {
                ops.fetch("Removing items from " + collection, [], '/collection/cards/' + collection + '/remove', {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(selected),
                }).then(data => {
                    setRefresh(true);
                    setSelected([]);
                });
            },
            () => {

            }
        );
    }

    const moveCards = () => {
        ops.fetch("Moving items between " + collection + " and " + destinationCollection, [], '/collection/move/' + destinationCollection, {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(selected),
        }).then(data => {
            setRefresh(true);
            setSelected([]);
        });
    }

    const deleteCollection = () => {
        let moveToCollections = collections.filter(s => s.id !== collection);
        moveToCollections.push("");
        confirm({ confirmType: "collection", collection: collection, collections: moveToCollections }).then(
            ({ input }) => {
                ops.fetch("Deleting collection " + collection, {}, '/collection/remove/' + collection + '?keepCardsInCollection=' + input, {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(selected),
                }).then(data => {
                    navigate('/' + (input ? 'c/' + input : ''));
                });
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
                        <input type="file" onChange={handleFileChange} />
                        <div>{file && `${file.name} - ${file.type}`}</div>
                        <button type="button" className="btn btn-info" onClick={handleUploadClick}>Import</button>
                    </div>
                    <div className="col-auto">
                        <a href={'/collection/export/' + collection}>
                            <button type="button" className="btn btn-info">Export</button>
                        </a>
                    </div>
                    <div className="col-auto">
                        <button onClick={deleteCollection} type="button" className="btn btn-danger">🗑️ collection</button>
                    </div>
                    <div className="col-auto">
                        <span className="badge badge-primary">{Object.keys(ops.operations).length} operations active</span>
                    </div>
                    {Object.entries(ops.operations).map( ([key, o]) =>
                        <div key={key} className="col-auto">
                            <span className="badge badge-primary">{o.message}</span>
                        </div>
                    )}
                </div>
            </nav>
        </Fragment>
    );
}
