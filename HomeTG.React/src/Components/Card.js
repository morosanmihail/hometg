import React, { useState, useEffect } from 'react';
import CardDetails from './CardDetails';
import { useSelectedCardsDispatch } from './CardListContexts/SelectedCardsContext';
import { useCardLoader } from './CardListContexts/CardLoaderContext';

export default function MtGCard({ id, card = null, details = null}) {
    const [_card, setCard] = useState(card);
    const [selected, setSelected] = useState(false);

    const selectedDispatch = useSelectedCardsDispatch();
    const loader = useCardLoader();

    const toggleSelected = () => {
        if (details != null) {
            selectedDispatch({type:(!selected ? 'added' : 'deleted'), card: details});
            setSelected(s => !s);
        }
    }

    useEffect(() => {
        if (_card == null) {
            async function execute() {
                const receivedCard = await loader(id);
                setCard(receivedCard);
            }
            execute();
        }
    }, [id, _card, details])

    let imagePath = (_card != null && _card.cardIdentifiers != null) ? "https://api.scryfall.com/cards/" + _card.cardIdentifiers.scryfallId + "?format=image" : "";

    return (
        <React.Fragment>
            {
                (_card == null) ? <p>Loading...</p> :
                    <div className={"card" + (selected ? " border border-primary" : "")}>
                        <img className="lazyload" src={imagePath} alt={_card.name} lazyload="on" />
                            <CardDetails id={id} details={details} toggleSelected={toggleSelected} />
                        <div className="card-info">
                            <div className="row align-items-center">
                                <span className="col-sm-8">
                                    {_card.name}
                                    {
                                        (details != null) ? <span className='badge bg-secondary'>{details.collectionId}</span> : ""
                                    }
                                </span>
                                <span className="col-sm-11">{_card.setCode}</span>
                            </div>
                        </div>
                    </div>
            }
        </React.Fragment>
    );
}
