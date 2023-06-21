import React, { useState, useEffect, useContext } from 'react';
import { useCardCache } from './CardCacheContext';
import { CollectionContext } from './CollectionContext';
import { OperationsContext } from '../OperationsContext';

function MtGCard({ id, card = null, details = null,
    onAdd = null, onSelectCard = null
}) {
    const [_card, setCard] = useState(card);
    const [_details, setDetails] = useState(details);
    const [selected, setSelected] = useState(false);

    const [cache, dispatch] = useCardCache();
    const currentCollection = useContext(CollectionContext);
    const ops = useContext(OperationsContext);

    const retrieveCardInfo = (id) => {
        return ops.fetch("Updating details for card " + id, '/mtg/cards?ids=' + id).then(response => {
            if (response.status === 200) {
                return response.json()
            }
        });
    }

    useEffect(() => {
        if (_card == null) {
            if (cache[id]) {
                setCard(cache[id]);
            } else {
                retrieveCardInfo(id).then(data => {
                    setCard(data[id]);
                    dispatch({ type: 'ADD-TO-CACHE', id: id, data: data[id] });
                });
            }
        }
    }, [id, _card, details])

    const toggleSelected = () => {
        setSelected(s => !s);
        onSelectCard(_details, !selected);
    }

    const updateQuantity = (delta, deltaFoil) => {
        let collection = currentCollection != null ? currentCollection : _details.collectionId;
        let add = parseInt(delta) >= 0 && parseInt(deltaFoil) >= 0;
        let url = '/collection/cards/' + collection + '/' + (add ? 'add' : 'delete');
        url = url + '?Id=' + _card.id + '&CollectionID=' + collection + '&Quantity=' + Math.abs(parseInt(delta));
        url = url + '&FoilQuantity=' + Math.abs(parseInt(deltaFoil));

        ops.fetch("Updating quantities for card " + id, url, {
            method: add ? "put" : "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        }).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    if (_details != null) {
                        setDetails(add ? data[0] : data);
                    } else {
                        if (onAdd != null) {
                            onAdd(data[0]);
                        }
                    }
                })
            }
        })
    }

    let imagePath = (_card != null && _card.cardIdentifiers != null) ? "https://api.scryfall.com/cards/" + _card.cardIdentifiers.scryfallId + "?format=image" : "";

    return (
        <React.Fragment>
            {
                (_card == null) ? <p>Loading...</p> : 
                    <div className={"card" + (selected ? " border border-primary" : "")}>
                        <img className="lazyload" src={imagePath} alt={_card.name} lazyload="on" />

                        {_details != null ?
                            <div className="card-img-overlay d-flex">
                                <div className="align-self-center">
                                    <div className="btn-group-vertical">
                                        <button className="btn btn-sm btn-outline-primary" onClick={toggleSelected}>{selected ? "☑️" : "◼️"}</button>

                                        <span className="btn"></span>

                                        <button onClick={event => updateQuantity(1, 0)} className="btn btn-sm btn-outline-success">+</button>
                                        <span className="btn badge badge-light">{_details.quantity}</span>
                                        <button onClick={event => updateQuantity(-1, 0)} className="btn btn-sm btn-outline-danger">-</button>

                                        <span className="btn"></span>

                                        <button onClick={event => updateQuantity(0, 1)} className="btn btn-sm btn-outline-success">+</button>
                                        <span className="btn badge badge-info">{_details.foilQuantity}</span>
                                        <button onClick={event => updateQuantity(0, -1)} className="btn btn-sm btn-outline-danger">-</button>
                                    </div>
                                </div>
                            </div>
                            : null}
                        <div className="card-info">
                            <div className="row mb-3 align-items-center">
                                <span className="name col-sm-11">{_card.name}</span>
                                <span className="setCode col-sm-11">{_card.setCode}</span>
                            </div>
                            {_details == null ?
                                <React.Fragment>
                                    <div className="row mb-3 align-items-center">
                                        <button onClick={event => updateQuantity(1, 0)} className="btn btn-default col-sm-12" type="button">Add Non-foil</button>
                                        <button onClick={event => updateQuantity(0, 1)} className="btn btn-default col-sm-12" type="button">Add Foil</button>
                                    </div>
                                </React.Fragment>
                                : null
                            }
                        </div>
                    </div>
            }
        </React.Fragment>
    );
}

export default MtGCard;
