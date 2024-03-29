import React from 'react';
import { useCollection } from './CollectionContext';
import { useOperations } from '../OperationsContext';
import { useCardsDispatch } from '../Components/CardListContexts/CardsContext';
import { useRefreshCardList } from './CardListContexts/RefreshCardListContext';

export default function CardDetails({id, details = null, toggleSelected}) {
    const ops = useOperations();
    const currentCollection = useCollection();
    const cardsDispatch = useCardsDispatch();
    const triggerRefresh = useRefreshCardList();

    const updateQuantity = (delta, deltaFoil) => {
        let collection = details == null ? currentCollection : details.collectionId;
        let add = parseInt(delta) >= 0 && parseInt(deltaFoil) >= 0;
        let url = '/collection/cards/' + collection + '/' + (add ? 'add' : 'delete');
        let body = {
            id: id,
            collectionId: collection,
            quantity: Math.abs(parseInt(delta)),
            foilQuantity: Math.abs(parseInt(deltaFoil))
        }

        ops.fetch(
            "Updating quantities for card " + id, {}, url,
            {
                method: "post",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(body),
            }
        ).then(data => {
            cardsDispatch({type: 'added', card: add ? data[0] : data});
            triggerRefresh(true);
        })
    }

    return (
        <div className="card-img-overlay d-flex" onClick={toggleSelected}>
            <div className="align-self-center">
                <div className="btn-group-vertical">
                {
                    (details != null) ?
                    <React.Fragment>
                        <button onClick={e => updateQuantity(1, 0)} className="btn btn-sm btn-outline-success">+</button>
                        <span className="btn badge bg-secondary">{details.quantity}</span>
                        <button onClick={e => updateQuantity(-1, 0)} className="btn btn-sm btn-outline-danger">-</button>

                        <span className="btn"></span>

                        <button onClick={e => updateQuantity(0, 1)} className="btn btn-sm btn-outline-success">+</button>
                        <span className="btn badge bg-info">{details.foilQuantity}</span>
                        <button onClick={e => updateQuantity(0, -1)} className="btn btn-sm btn-outline-danger">-</button>
                    </React.Fragment>
                    :
                    <React.Fragment>
                        <button onClick={e => updateQuantity(1, 0)} className="btn btn-sm btn-light">Add</button>
                        <span className="btn"></span>
                        <button onClick={e => updateQuantity(0, 1)} className="btn btn-sm btn-info">Add Foil</button>
                    </React.Fragment>
                }
                </div>
            </div>
        </div>
    );
}
