import React, { useState, useEffect } from 'react';

function MtGCard({ id, card = null }) {
    const [_card, setCard] = useState(card);

    useEffect(() => {
        if (_card == null) {
            fetch('/mtg/cards?ids=' + id).then(response => {
                if (response.status === 200) {
                    response.json().then(data => {
                        setCard(data[0]);
                    })
                }
            });
        }
    }, [id, _card])

    const renderCard = (card) => {
        let imagePath = "";
        imagePath = "https://api.scryfall.com/cards/" + card.cardIdentifiers.scryfallId + "?format=image";
        return (
            <div id={card.id} className="card">
                <img className="lazyload" src={imagePath} alt={card.name} lazyload="on" />
                <div className="card-info" data-id="details-{card.id}">
                    <div className="row mb-3 align-items-center">
                        <span className="name">{card.name}</span>
                        <span className="setCode">{card.setCode}</span>
                    </div>
                </div>
            </div>
        );
    }

    let contents = _card == null
        ? <p>Loading...</p>
        : renderCard(_card);

    return (
        <div>
            {contents}
        </div>
    );
}

export default MtGCard;
