import React, { useState, useContext } from 'react';
import { CollectionContext } from './CollectionContext';
import { OperationsContext } from '../OperationsContext';

export default function CardDetails({id, details = null, onAdd, selected, toggleSelected}) {
    const ops = useContext(OperationsContext);
    const currentCollection = useContext(CollectionContext);

    const updateQuantity = (delta, deltaFoil) => {
        let collection = currentCollection != null ? currentCollection : details.collectionId;
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
            onAdd(add ? data[0] : data);
        })
    }

    return (
        <>
        {
            (details != null) ?
            <React.Fragment>
                <button className="btn btn-sm btn-outline-primary" onClick={toggleSelected}>{selected ? "☑️" : "◼️"}</button>

                <span className="btn"></span>

                <button onClick={event => updateQuantity(1, 0)} className="btn btn-sm btn-outline-success">+</button>
                <span className="btn badge badge-light">{details.quantity}</span>
                <button onClick={event => updateQuantity(-1, 0)} className="btn btn-sm btn-outline-danger">-</button>

                <span className="btn"></span>

                <button onClick={event => updateQuantity(0, 1)} className="btn btn-sm btn-outline-success">+</button>
                <span className="btn badge badge-info">{details.foilQuantity}</span>
                <button onClick={event => updateQuantity(0, -1)} className="btn btn-sm btn-outline-danger">-</button>
            </React.Fragment>
            :
            <React.Fragment>
                <button onClick={event => updateQuantity(1, 0)} className="btn btn-sm btn-light">Add</button>
                <span className="btn"></span>
                <button onClick={event => updateQuantity(0, 1)} className="btn btn-sm btn-info">Add Foil</button>
            </React.Fragment>
        }
        </>
    );
}
