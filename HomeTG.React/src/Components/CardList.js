import React, { useState, useEffect } from 'react';
import Card from './Card';
import { Link } from "react-router-dom";
import Search from "./Search";

function CardList({ collection, offset }) {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(true);
    const [refresh, setRefresh] = useState(false);
    const [selected, setSelected] = useState([]);
    const [destinationCollection, setDestinationCollection] = useState(collection);
    const [validCollections, setValidCollections] = useState([]);

    let pageSize = 12;

    useEffect(() => {
        fetch('/collection/cards/' + collection + '/list?offset=' + offset).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCards(data);
                    setLoading(false);
                    setRefresh(false);
                    setSelected([]);
                })
            }
        });
        fetch('/collection/list').then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setValidCollections(data);
                })
            }
        });
    }, [collection, offset, refresh])

    const moveCards = () => {
        fetch('/collection/move/' + destinationCollection, {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(selected),
        }).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setRefresh(true);
                    setSelected([]);
                })
            }
        });
    }

    const deleteCards = () => {
    }

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

    const renderCards = (cards) => {
        return (
            <React.Fragment>
                {cards.map(card =>
                    <Card id={card.id} details={card} key={card.collectionId + "-" + card.id} onSelectCard={onSelectCard} />
                )}
            </React.Fragment>
        );
    }

    let contents = (loading || refresh) ? <p>Loading...</p> : renderCards(cards);

    return (
        <React.Fragment>
            <Search collection={collection} onAdd={onAdd} />

            <nav className="navbar navbar-dark bg-dark">
                <div className="form-row align-items-center">
                    <div className="col-auto">
                        <span className="badge badge-primary">{selected.length} selected</span>
                    </div>
                    <div className="col-auto">
                        <button onClick={deleteCards} type="button" className="btn btn-danger">🗑️</button>
                    </div>
                    <div className="col-auto">
                        <button onClick={moveCards} type="button" className="btn btn-info" alt="Move to collection">Move to</button>
                    </div>
                    <div className="col-auto">
                        <select onChange={(e) => setDestinationCollection(e.target.value)} className="form-control" id="exampleFormControlSelect1">
                            {validCollections.map(c => 
                                <option key={c.id} dropdown={c.id} value={c.id}>{c.id}</option>
                            )}
                        </select>
                    </div>
                    <div className="col-auto">
                        <button type="button" className="btn btn-danger">🗑️ collection</button>
                    </div>
                </div>
            </nav>
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