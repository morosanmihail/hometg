import React, { Fragment } from 'react';
import { useCollection } from '../CollectionContext';

export default function ExportCollection() {
    const collection = useCollection();

    return (
        <Fragment>
            <div className="col-auto">
                <a href={'/collection/export/' + collection}>
                    <button type="button" className="btn btn-info">Export</button>
                </a>
            </div>
        </Fragment>
    );
}
