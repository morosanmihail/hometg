import React, { Fragment } from 'react';
import SelectionTracker from './CardListNavButtons/SelectionTracker';
import DeleteCards from './CardListNavButtons/DeleteCards';
import MoveCards from './CardListNavButtons/MoveCards';
import ImportCards from './CardListNavButtons/ImportCards';
import DeleteCollection from './CardListNavButtons/DeleteCollection';
import ExportCollection from './CardListNavButtons/ExportCollection';
import QuickSearch from './CardListNavButtons/QuickSearch';

export default function CardListNav() {
    return (
        <Fragment>
            <nav className="navbar navbar-expand-md bg-body-tertiary" data-bs-theme="dark">
                <div className="container-fluid">
                    <QuickSearch/>
                    <SelectionTracker/>
                    <DeleteCards/>
                    <MoveCards/>
                    <ImportCards/>
                    <ExportCollection/>
                    <DeleteCollection/>
                </div>
            </nav>
        </Fragment>
    );
}
