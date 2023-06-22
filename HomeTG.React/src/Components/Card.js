import React, { useState, useEffect, useContext } from 'react';
import { useCardCache } from './CardCacheContext';
import { CardDetails } from './CardDetails';
import { OperationsContext } from '../OperationsContext';

export default function MtGCard({ id, card = null, details = null,
    onAdd = null, onSelectCard = null
}) {
    const [_card, setCard] = useState(card);
    const [selected, setSelected] = useState(false);

    const [cache, dispatch] = useCardCache();
    const ops = useContext(OperationsContext);

    useEffect(() => {
        if (_card == null) {
            if (cache[id]) {
                setCard(cache[id]);
            } else {
                ops.fetch("Updating details for card " + id, '/mtg/cards?ids=' + id).then(data => {
                    setCard(data[id]);
                    dispatch({ type: 'ADD-TO-CACHE', id: id, data: data[id] });
                });
            }
        }
    }, [id, _card, details])

    const toggleSelected = () => {
        setSelected(s => !s);
        onSelectCard(details, !selected);
    }


    let imagePath = (_card != null && _card.cardIdentifiers != null) ? "https://api.scryfall.com/cards/" + _card.cardIdentifiers.scryfallId + "?format=image" : "";

    return (
        <React.Fragment>
            {
                (_card == null) ? <p>Loading...</p> :
                    <div className={"card" + (selected ? " border border-primary" : "")}>
                        <img className="lazyload" src={imagePath} alt={_card.name} lazyload="on" />
                            <div className="card-img-overlay d-flex">
                                <div className="align-self-center">
                                    <div className="btn-group-vertical">
                                        <CardDetails id={id} details={details} onAdd={onAdd} selected={selected} toggleSelected={toggleSelected} />
                                    </div>
                                </div>
                            </div>
                        <div className="card-info">
                            <div className="row mb-3 align-items-center">
                                <span className="name col-sm-11">{_card.name}</span>
                                <span className="setCode col-sm-11">{_card.setCode}</span>
                            </div>
                        </div>
                    </div>
            }
        </React.Fragment>
    );
}
