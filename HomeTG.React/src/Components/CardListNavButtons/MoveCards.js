import React, { Fragment, useState } from 'react';
import { useOperations } from '../../OperationsContext';
import { useCollection, useCollections } from '../CollectionContext';
import { useSelectedCards, useSelectedCardsDispatch } from '../CardListContexts/SelectedCardsContext';

export default function MoveCards({setRefresh}) {
    const ops = useOperations();
    const collection = useCollection();
    const collections = useCollections();
    const selected = useSelectedCards();
    const selectedDispatch = useSelectedCardsDispatch();

    const [destinationCollection, setDestinationCollection] = useState(collection);

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
            selectedDispatch({type:'empty'});
        });
    }

    return (
        <Fragment>
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
        </Fragment>
    );
}
