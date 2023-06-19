import React, { useState, useEffect } from 'react';
import Card from './Card';
import { Link, useParams } from "react-router-dom";

function CardList() {
    const { collection = "Main", offset = 0 } = useParams();
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
            <div id="listjs-grid">
                <div className="input-group">
                    <input className="search form-control" placeholder="Quick filter" />
                    <span className="btn btn-secondary sort" data-sort="name">Sort by name</span>
                    <span className="btn btn-secondary sort" data-sort="setCode">Sort by set</span>
                </div>
                <div className="card-grid list">
                    {cards.map(card =>
                        <div key={card.id}>
                            <Card id={card.id} />
                        </div>
                    )}
                </div>
            </div>
        );
    }

    let contents = loading
        ? <p>Loading...</p>
        : renderCards(cards);

    return (
        <div className="container pt-4 tab-content">
            <div className="tab-pane fade show active" id="nav-main" role="tabpanel" aria-labelledby="nav-main-tab">
                <div id="main-content">
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
            </div>
        </div>
    );
}

export default CardList;