import React, { Fragment } from 'react';
import { confirm } from "./ConfirmCollectionDelete";
import { useOperations } from '../../OperationsContext';
import { useCollection } from '../CollectionContext';
import { useSelectedCards, useSelectedCardsDispatch } from '../CardListContexts/SelectedCardsContext';

export default function DeleteCards({setRefresh}) {
    const ops = useOperations();
    const collection = useCollection();
    const selected = useSelectedCards();
    const selectedDispatch = useSelectedCardsDispatch();

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
                    selectedDispatch({type:'empty'});
                });
            },
            () => {

            }
        );
    }

    return (
        <Fragment>
            <button onClick={deleteCards} type="button" className="btn btn-danger">🗑️</button>
        </Fragment>
    );
}
