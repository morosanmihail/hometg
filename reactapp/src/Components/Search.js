import React, { useState } from 'react';
import Card from './Card';

function Search({ collection, onAdd }) {
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(false);
    const [searchOptions, setSearchOptions] = useState({});

    const renderCards = (cards) => {
        return (
            <div className="card-grid list">
                {cards.map(card =>
                    <Card key={card.id} id={card.id} card={card} currentCollection={collection} onAdd={onAdd} />
                )}
            </div>
        );
    }

    const populateSearch = () => {
        setLoading(true);
        fetch('/mtg/cards/search', {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(searchOptions)
        }).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCards(data);
                    setLoading(false);
                })
            }
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
            </div>
        </div>
    );
}

export default Search;