import React, { Fragment } from 'react';
import { useSelectedCards } from '../CardListContexts/SelectedCardsContext';

export default function SelectionTracker() {
    const selected = useSelectedCards();

    return (
        <Fragment>
            <div className="col-auto">
                <span className="badge badge-primary">{selected.length} selected</span>
            </div>
        </Fragment>
    )
}
