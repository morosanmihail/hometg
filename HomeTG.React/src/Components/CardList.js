import React, { useState, useEffect } from 'react';
import Card from './Card';
import { Link } from "react-router-dom";
import Search from "./Search";
import CardListNav from "./CardListNav";
import { CardCacheProvider } from './CardCacheContext';
import { useCollection, useOffset } from './CollectionContext';
import { useOperations } from '../OperationsContext';
import { useSelectedCardsDispatch } from './CardListContexts/SelectedCardsContext';

export default function CardList({ showSearch=false }) {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(true);
    const [refresh, setRefresh] = useState(false);
    const collection = useCollection();
    const offset = useOffset();
    const selectedDispatch = useSelectedCardsDispatch();
    const ops = useOperations();

    let pageSize = 12;

    useEffect(() => {
        ops.fetch(
            "Listing items in " + collection, [],
            '/collection/cards/' + collection + '/list?offset=' + offset
            ).then(data => {
                setCards(data);
                setLoading(false);
                setRefresh(false);
                selectedDispatch({type:'empty'});
            });
    }, [collection, offset, refresh])

    const onAdd = (newCard) => {
        let updated = false;
        const newCards = cards.map(c => {
            if (c.id === newCard.id) {
                updated = true;
                return newCard;
            }
            return c;
        })
        if (updated) {
            setCards(newCards);
        } else {
            if (offset === 0) {
                setCards([newCard, ...cards.slice(0, pageSize - 1)]);
            } else {
                setRefresh(true)
            }
        }
    }

    return (
        <CardCacheProvider>
            <Search onAdd={onAdd} dedicatedPage={showSearch} />
            <CardListNav setRefresh={setRefresh} />
            <div className="card-grid list">
                {
                    (loading || refresh) ? <p>Loading...</p> :
                        <React.Fragment>
                            {cards.map(card =>
                                <Card id={card.id} details={card} key={card.collectionId + "-" + card.id}
                                    onAdd={onAdd} />
                            )}
                        </React.Fragment>
                }
            </div>
            <nav aria-label="Page navigation">
                <ul className="pagination center">
                    <li className={"page-item" + (parseInt(offset) === 0 ? " disabled" : "")}>
                        {
                            parseInt(offset) > 0 ?
                        <Link to={"/c/" + collection + "/" + (parseInt(offset) - pageSize)}>
                            <button className="page-link">Previous</button>
                        </Link>
                        : <button className="page-link">Previous</button>}
                    </li>
                    <li className="page-item disabled">
                        <button id="list-page" className="page-link">{(parseInt(offset) + pageSize) / pageSize}</button>
                    </li>
                    <li className={"page-item" + (cards.length < pageSize ? " disabled" : "")}>
                        {
                            cards.length >= pageSize ?
                        <Link to={"/c/" + collection + "/" + (parseInt(offset) + pageSize)}>
                            <button className="page-link">Next</button>
                        </Link>
                        : <button className="page-link">Next</button>}
                    </li>
                </ul>
            </nav>
        </CardCacheProvider>
    );
}
