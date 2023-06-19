import React, { Component } from 'react';

export default class MtGCard extends Component {
    constructor(props) {
        super(props);
        this.state = {
            id: props.id, card: props.card
        };
    }

    componentDidMount() {
        if (this.state.card == null)
            this.populateCard();
    }

    static renderCard(card) {
        let imagePath = "";
        imagePath = "https://api.scryfall.com/cards/" + card.cardIdentifiers.scryfallId + "?format=image";
        return (
    <div id={card.id} className="card">
        <img className="lazyload" src={imagePath} alt={card.name} lazyload="on" />
        <div className="card-info" data-id="details-{card.id}">
            <div className="row mb-3 align-items-center">
                <span className="name">{card.name}</span>
                <span className="setCode">{card.setCode}</span>
            </div>
        </div>
    </div>
        );
    }

    render() {
        let contents = this.state.card == null
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
            this.setState({id: this.state.id, card: data[0] });
        }
    }
}