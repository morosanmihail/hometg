import React, { useState, useContext } from 'react';
import { Link } from "react-router-dom";
import Card from './Card';
import { OperationsContext } from '../OperationsContext';
import { CollectionContext } from './CollectionContext';

function Search({ onAdd }) {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(false);
    const [searchOptions, setSearchOptions] = useState({});
    const [offset, setOffset] = useState(0);

    let pageSize = 24;

    const collection = useContext(CollectionContext);
    const ops = useContext(OperationsContext);

    const renderCards = (cards) => {
        return (
            <div className="card-grid list">
                {cards.map(card =>
                    <Card key={card.id} id={card.id} card={card} onAdd={onAdd} />
                )}
            </div>
        );
    }

    const populateSearch = (event, newOffset = 0) => {
        setLoading(true);
        setOffset(newOffset);

        ops.fetch("Searching the MtG database", [], '/mtg/cards/search?pageSize=' + pageSize + '&offset=' + newOffset, {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(searchOptions)
        }).then(data => {
            setCards(data);
            setLoading(false);
        })
    }

    const handleSearchInput = (event, field) => {
        let newState = Object.assign({}, searchOptions);
        newState[field] = event.target.value;
        setSearchOptions(newState);
    }

    let contents = loading
        ? <p>Loading...</p>
        : renderCards(cards);

    return (
        <React.Fragment>
            <div className="collapse" id="search">
                <h2>Search</h2>
                <div className="list-group list-group-flush mx-3 mt-4">
                    <div className="input-group mb-3">
                        <input onChange={event => handleSearchInput(event, "name")} type="text" className="form-control" id="search-bar-name" placeholder="Name" aria-describedby="button-addon2" />
                        <input onChange={event => handleSearchInput(event, "setCode")} type="text" className="form-control" id="search-bar-set" placeholder="Set" aria-describedby="button-addon2" />
                    </div>
                    <div className="input-group mb-3">
                        <input onChange={event => handleSearchInput(event, "artist")} type="text" className="form-control" id="search-bar-artist" placeholder="Artist" aria-describedby="button-addon2" />
                        <input onChange={event => handleSearchInput(event, "collectorNumber")} type="text" className="form-control" id="search-bar-collector-number" placeholder="Collector Number" aria-describedby="button-addon2" />
                    </div>
                    <div className="input-group mb-3">
                        <input onChange={event => handleSearchInput(event, "text")} type="text" className="form-control" id="search-bar-text" placeholder="Text" aria-describedby="button-addon2" />
                    </div>
                    <div className="input-group mb-3">
                        <button onClick={populateSearch} className="btn btn-outline-secondary" type="button" id="button-addon2">Search</button>
                    </div>
                    <div className="search-results" id="search-results">
                        {contents}
                    </div>
                        { cards.length > 0 ?
                    <nav aria-label="Page navigation">
                <ul className="pagination center">
                    <li className={"page-item" + (parseInt(offset) === 0 ? " disabled" : "")}>
                        {
                            parseInt(offset) > 0 ?
                            <button onClick={e => populateSearch(e, parseInt(offset) - pageSize)} className="page-link">Previous</button>
                        : <button className="page-link">Previous</button>}
                    </li>
                    <li className="page-item disabled">
                        <button id="list-page" className="page-link">{(parseInt(offset) + pageSize) / pageSize}</button>
                    </li>
                    <li className={"page-item" + (cards.length < pageSize ? " disabled" : "")}>
                        {
                            cards.length >= pageSize ?
                            <button onClick={e => populateSearch(e, parseInt(offset) + pageSize)}  className="page-link">Next</button>
                        : <button className="page-link">Next</button>}
                    </li>
                </ul>
            </nav>
            : null }
                </div>
                <hr />
            </div>
        </React.Fragment>
    );
}

export default Search;
