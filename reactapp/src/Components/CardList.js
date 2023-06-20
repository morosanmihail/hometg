import React, { useState, useEffect } from 'react';
import Card from './Card';
import { Link } from "react-router-dom";
import Search from "./Search";

function CardList({ collection, offset }) {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(true);
    const [refresh, setRefresh] = useState(false);

    let pageSize = 12;

    useEffect(() => {
        fetch('/collection/cards/' + collection + '/list?offset=' + offset).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCards(data);
                    setLoading(false);
                    setRefresh(false);
                })
            }
        });
    }, [collection, offset, refresh])

    const renderCards = (cards) => {
        return (
            <React.Fragment>
                {cards.map(card =>
                    <Card id={card.id} details={card} key={card.collectionId + "-" + card.id} />
                )}
            </React.Fragment>
        );
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

    let contents = (loading || refresh) ? <p>Loading...</p> : renderCards(cards);

    return (
        <React.Fragment>
            <Search collection={collection} onAdd={onAdd} />

                <div className="card-grid list">
                    {contents}
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
        </React.Fragment>
    );
}

export default CardList;