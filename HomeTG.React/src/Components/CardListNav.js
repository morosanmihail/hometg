import React, { Fragment } from 'react';
import SelectionTracker from './CardListNavButtons/SelectionTracker';
import DeleteCards from './CardListNavButtons/DeleteCards';
import MoveCards from './CardListNavButtons/MoveCards';
import ImportCards from './CardListNavButtons/ImportCards';
import DeleteCollection from './CardListNavButtons/DeleteCollection';
import ExportCollection from './CardListNavButtons/ExportCollection';
import OperationsTracker from './CardListNavButtons/OperationsTracker';

export default function CardListNav({ selected, setSelected, setRefresh }) {
    return (
        <Fragment>
            <nav className="navbar navbar-dark bg-dark">
                <div className="form-row align-items-center">
                    <SelectionTracker />
                    <DeleteCards setRefresh={setRefresh} />
                    <MoveCards setRefresh={setRefresh} />
                    <ImportCards/>
                    <ExportCollection/>
                    <DeleteCollection/>
                    <OperationsTracker/>
                </div>
            </nav>
        </Fragment>
    );
}
