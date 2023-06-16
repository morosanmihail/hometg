import React, { Component } from 'react';

export default class MtGCard extends Component {
    constructor(props) {
        super(props);
        this.state = { name: "", set: "", loading: true };
    }

    componentDidMount() {
        this.populateCard();
    }

    static renderCard(name, set) {
        return (
            <div>
                <p>{name}</p>
                <p>{set}</p>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p>Loading...</p>
            : MtGCard.renderCard(this.state.name, this.state.set);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateCard() {
        const response = await fetch('mtg/cards?ids=3ef58d1c-993e-5ade-af6a-aa77edb53fd1');
        if (response.status === 200) {
            const data = await response.json();
            this.setState({ name: data[0].name, set: data[0].setCode, loading: false });
        }
    }
}