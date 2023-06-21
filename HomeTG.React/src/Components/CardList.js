import React, { useState, useEffect, useContext } from 'react';
import Card from './Card';
import { Link } from "react-router-dom";
import Search from "./Search";
import MoveCardsNav from "./MoveCardsNav";
import uuid from 'react-native-uuid';
import { CardCacheProvider } from './CardCacheContext';
import { CollectionContext } from './CollectionContext';
import { OperationsContext } from '../OperationsContext';

function CardList({ offset, collections }) {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(true);
    const [refresh, setRefresh] = useState(false);
    const [selected, setSelected] = useState([]);
    const collection = useContext(CollectionContext);
    const ops = useContext(OperationsContext);

    let pageSize = 12;

    useEffect(() => {
        let opId = uuid.v4();
        ops.addOperation(opId, { message: "Listing items in " + collection });
        fetch('/collection/cards/' + collection + '/list?offset=' + offset).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCards(data);
                    setLoading(false);
                    setRefresh(false);
                    setSelected([]);
                })
            }
            ops.removeOperation(opId);
        });
    }, [collection, offset, refresh])


    const onSelectCard = (card, isSelected) => {
        if (card == null) return;
        if (isSelected) {
            setSelected(prev => [...prev, card]);
        } else {
            setSelected(prev => prev.filter(s => s.id !== card.id));
        }
    }

    const onAdd = (newCard) => {
        // TODO: this does not trigger a rerender of any updated cards, only new ones
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
                setCards([newCard, ...cards]);
            } else {
                setRefresh(true)
            }
        }
    }

    return (
        <CardCacheProvider>
            <Search onAdd={onAdd} />
            <MoveCardsNav
                collections={collections}
                selected={selected} setSelected={setSelected} setRefresh={setRefresh} />
            <div className="card-grid list">
                {
                    (loading || refresh) ? <p>Loading...</p> :
                        <React.Fragment>
                            {cards.map(card =>
                                <Card id={card.id} details={card} key={card.collectionId + "-" + card.id}
                                    onSelectCard={onSelectCard} />
                            )}
                        </React.Fragment>
                }
            </div>
            <nav aria-label="Page navigation">
                <ul className="pagination center">
                    <li className={"page-item" + (parseInt(offset) === 0 ? " disabled" : "")}>
                        <Link to={"/" + collection + "/" + (parseInt(offset) - pageSize)}>
                            <button className="page-link">Previous</button>
                        </Link>
                    </li>
                    <li className="page-item disabled">
                        <button id="list-page" className="page-link">{(parseInt(offset) + pageSize) / pageSize}</button>
                    </li>
                    <li className={"page-item" + (cards.length < pageSize ? " disabled" : "")}>
                        <Link to={"/" + collection + "/" + (parseInt(offset) + pageSize)}>
                            <button className="page-link">Next</button>
                        </Link>
                    </li>
                </ul>
            </nav>
        </CardCacheProvider>
    );
}

export default CardList;