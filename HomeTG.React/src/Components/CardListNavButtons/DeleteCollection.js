import React, { Fragment } from 'react';
import { confirm } from "./ConfirmCollectionDelete";
import { useNavigate } from "react-router-dom";
import { useOperations } from '../../OperationsContext';
import { useCollection, useCollections } from '../CollectionContext';

export default function DeleteCollection() {
    const navigate = useNavigate();
    const ops = useOperations();
    const collection = useCollection();
    const collections = useCollections();

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
            <div className="d-flex">
                <button onClick={deleteCollection} type="button" className="btn btn-danger">🗑️ collection</button>
            </div>
        </Fragment>
    );
}
