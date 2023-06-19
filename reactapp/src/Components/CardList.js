import React, { useState, useEffect } from 'react';
import Card from './Card';
import { Link } from "react-router-dom";

function CardList({ collection, offset }) {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(true);

    let pageSize = 12;

    useEffect(() => {
        fetch('/collection/cards/' + collection + '/list?offset=' + offset).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCards(data);
                    setLoading(false);
                })
            }
        });
    }, [collection, offset])

    const renderCards = (cards) => {
        return (
            <React.Fragment>
                {cards.map(card =>
                    <Card id={card.id} details={card} key={card.collectionId + "-" + card.id} />
                )}
            </React.Fragment>
        );
    }

    let contents = (loading) ? <p>Loading...</p> : renderCards(cards);

    return (
        <div className="container pt-4 tab-content">
            <div className="tab-pane fade show active" id="nav-main" role="tabpanel" aria-labelledby="nav-main-tab">
                <div id="main-content">
                    <div id="listjs-grid">
                        <div className="input-group">
                            <input className="search form-control" placeholder="Quick filter" />
                            <span className="btn btn-secondary sort" data-sort="name">Sort by name</span>
                            <span className="btn btn-secondary sort" data-sort="setCode">Sort by set</span>
                        </div>
                        <div className="card-grid list">
                            {contents}
                        </div>
                    </div>
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
            </div>
        </div>
    );
}

export default CardList;