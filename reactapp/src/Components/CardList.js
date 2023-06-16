import React, { Component } from 'react';
import Card from './Card';

export default class CardList extends Component {
    constructor(props) {
        super(props);
        this.state = { cards: [], loading: true };
    }

    componentDidMount() {
        this.populateCards();
    }

    static renderCards(cards) {
        return (
            <div id="listjs-grid">
                <div class="input-group">
                    <input class="search form-control" placeholder="Quick filter" />
                    <span class="btn btn-secondary sort" data-sort="name">Sort by name</span>
                    <span class="btn btn-secondary sort" data-sort="setCode">Sort by set</span>
                </div>
                <div class="card-grid list">
                    {cards.map(card =>
                        <div>
                            <Card id={card.id} />
                        </div>
                    )}
                </div>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p>Loading...</p>
            : CardList.renderCards(this.state.cards);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateCards() {
        const response = await fetch('collection/cards/Main/list');
        if (response.status === 200) {
            const data = await response.json();
            this.setState({ cards: data, loading: false });
        }
    }
}