import React, { Component } from 'react';
import Card from './Card';

export default class Search extends Component {
    constructor(props) {
        super(props);
        this.state = {
            cards: [],
            searchOptions: {
                name: "",
                setCode: ""
            },
            loading: false
        };
    }

    componentDidMount() {}

    static renderCards(cards) {
        return (
            <div className="card-grid list">
                {cards.map(card =>
                    <div key={card.id}>
                        <Card id={card.id} card={card} />
                    </div>
                )}
            </div>
        );
    }

    populateSearch = () => {
        this.setState(prevState => {
            let newState = Object.assign({}, prevState);
            newState.loading = true;
            return newState;
        });
        fetch('mtg/cards/search', {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.state.searchOptions)
        }).then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    this.setState(prevState => {
                        let newState = Object.assign({}, prevState);
                        newState.cards = data;
                        newState.loading = false;
                        return newState;
                    });
                })
            }
        })
    }

    render() {
        let contents = this.state.loading
            ? <p>Loading...</p>
            : Search.renderCards(this.state.cards);

        return (
            <div className="collapse" id="search">
                <h2>Search</h2>
                <div className="list-group list-group-flush mx-3 mt-4">
                    <div className="input-group mb-3">
                        <input onChange={event => this.handleSearchInput(event, "name")} type="text" className="form-control" id="search-bar-name" placeholder="Name" aria-describedby="button-addon2" />
                        <input onChange={event => this.handleSearchInput(event, "setCode")} type="text" className="form-control" id="search-bar-set" placeholder="Set" aria-describedby="button-addon2" />
                    </div>
                    <div className="input-group mb-3">
                        <input onChange={event => this.handleSearchInput(event, "artist")} type="text" className="form-control" id="search-bar-artist" placeholder="Artist" aria-describedby="button-addon2" />
                        <input onChange={event => this.handleSearchInput(event, "collectorNumber")} type="text" className="form-control" id="search-bar-collector-number" placeholder="Collector Number" aria-describedby="button-addon2" />
                    </div>
                    <div className="input-group mb-3">
                        <input onChange={event => this.handleSearchInput(event, "text")} type="text" className="form-control" id="search-bar-text" placeholder="Text" aria-describedby="button-addon2" />
                    </div>
                    <div className="input-group mb-3">
                        <button onClick={this.populateSearch} className="btn btn-outline-secondary" type="button" id="button-addon2">Search</button>
                    </div>
                    <div className="search-results" id="search-results">
                        {contents}
                    </div>
                </div>
            </div>
        );
    }

    handleSearchInput = (event, field) => {
        this.setState(prevState => {
            let newState = Object.assign({}, prevState);
            newState.searchOptions[field] = event.target.value;
            return { newState };
        })
    }
}