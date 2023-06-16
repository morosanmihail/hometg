import React, { Component } from 'react';

export default class MtGCard extends Component {
    constructor(props) {
        super(props);
        this.state = {
            id: props.id, card: {}, loading: true
        };
    }

    componentDidMount() {
        this.populateCard();
    }

    static renderCard(card) {
        let imagePath = "";
        imagePath = "https://api.scryfall.com/cards/" + card.cardIdentifiers.scryfallId + "?format=image";
        return (
    <div id={card.id} class="card">
        <img class="lazyload" src={imagePath} alt={card.name} lazyload="on" />
        <div class="card-info" data-id="details-{card.id}">

            <div class="row mb-3 align-items-center">
                            <span class="name">{card.name}</span>
                            <span class="setCode">{card.setCode}</span>
            </div>
        </div>
    </div>

        );
    }

    render() {
        let contents = this.state.loading
            ? <p>Loading...</p>
            : MtGCard.renderCard(this.state.card);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateCard() {
        const response = await fetch('mtg/cards?ids=' + this.state.id);
        if (response.status === 200) {
            const data = await response.json();
            this.setState({id: this.state.id, card: data[0], loading: false });
        }
    }
}