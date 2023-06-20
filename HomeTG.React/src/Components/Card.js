import React, { useState, useEffect } from 'react';

function MtGCard({ id, card = null, details = null, currentCollection = null, onAdd = null }) {
    const [_card, setCard] = useState(card);
    const [_details, setDetails] = useState(details);

    useEffect(() => {
        if (_card == null) {
            fetch('/mtg/cards?ids=' + id).then(response => {
                if (response.status === 200) {
                    response.json().then(data => {
                        setCard(data[id]);
                    })
                }
            });
        }
    }, [id, _card, _details])

    const updateQuantity = (delta, deltaFoil) => {
        let collection = currentCollection != null ? currentCollection : _details.collectionId;
        let add = parseInt(delta) >= 0 && parseInt(deltaFoil) >= 0;
        let url = '/collection/cards/' + collection + '/' + (add ? 'add' : 'delete');
        url = url + '?Id=' + _card.id + '&CollectionID=' + collection + '&Quantity=' + Math.abs(parseInt(delta));
        url = url + '&FoilQuantity=' + Math.abs(parseInt(deltaFoil));
        fetch(url, {
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

    const renderCard = (card) => {
        let imagePath = "";
        imagePath = "https://api.scryfall.com/cards/" + card.cardIdentifiers.scryfallId + "?format=image";
        return (
            <div className="card">
                <img className="lazyload" src={imagePath} alt={card.name} lazyload="on" />

                {_details != null ?
                    <div className="card-img-overlay d-flex">
                        <div className="align-self-center">
                            <div className="btn-group-vertical">
                                <button onClick={event => updateQuantity(1, 0)} className="btn btn-sm btn-success">+</button>
                                <span className="btn badge badge-light">{_details.quantity}</span>
                                <button onClick={event => updateQuantity(-1, 0)} className="btn btn-sm btn-danger">-</button>

                                <span className="btn"></span>

                                <button onClick={event => updateQuantity(0, 1)} className="btn btn-sm btn-success">+</button>
                                <span className="btn badge badge-info">{_details.foilQuantity}</span>
                                <button onClick={event => updateQuantity(0, -1)} className="btn btn-sm btn-danger">-</button>
                            </div>
                        </div>
                    </div>
                    : null}
                <div className="card-info">
                    <div className="row mb-3 align-items-center">
                        <span className="name col-sm-11">{card.name}</span>
                        <span className="setCode col-sm-11">{card.setCode}</span>
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
        );
    }

    let contents = (_card == null) ? <p>Loading...</p> : renderCard(_card);

    return (
        <React.Fragment>
            {contents}
        </React.Fragment>
    );
}

export default MtGCard;
