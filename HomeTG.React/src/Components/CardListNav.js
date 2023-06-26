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
            <nav className="navbar navbar-expand-md bg-body-tertiary" data-bs-theme="dark">
                <div className="container-fluid">
                    <SelectionTracker />
                    <DeleteCards setRefresh={setRefresh} />
                    <MoveCards setRefresh={setRefresh} />
                    <ImportCards setRefresh={setRefresh} />
                    <ExportCollection/>
                    <DeleteCollection/>
                </div>
            </nav>
        </Fragment>
    );
}
