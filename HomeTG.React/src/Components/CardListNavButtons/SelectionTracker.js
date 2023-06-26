import React, { Fragment } from 'react';
import { useSelectedCards } from '../CardListContexts/SelectedCardsContext';

export default function SelectionTracker() {
    const selected = useSelectedCards();

    return (
        <Fragment>
                <span className="badge bg-secondary badge-primary">{selected.length} selected</span>

        </Fragment>
    )
}
