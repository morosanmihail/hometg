import React, { Fragment, useState } from 'react';
import { useOperations } from '../../OperationsContext';
import { useCollection, useCollections } from '../CollectionContext';
import { useSelectedCards, useSelectedCardsDispatch } from '../CardListContexts/SelectedCardsContext';
import { useRefreshCardList } from '../CardList';

export default function MoveCards() {
    const ops = useOperations();
    const collection = useCollection();
    const collections = useCollections();
    const selected = useSelectedCards();
    const selectedDispatch = useSelectedCardsDispatch();
    const triggerRefresh = useRefreshCardList();

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
            triggerRefresh();
            selectedDispatch({type:'empty'});
        });
    }

    return (
        <Fragment>
            <form className="d-flex">
                <button onClick={moveCards} type="button" className="btn btn-outline-info">Move</button>
                <select onChange={(e) => setDestinationCollection(e.target.value)} className="form-control" id="exampleFormControlSelect1">
                    {collections.map(c =>
                        <option key={"cardlistcol-" + c.id} dropdown={c.id} value={c.id}>{c.id}</option>
                    )}
                </select>
            </form>
        </Fragment>
    );
}
