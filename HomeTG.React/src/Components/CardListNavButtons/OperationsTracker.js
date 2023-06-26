import React, { Fragment } from 'react';
import { useOperations } from '../../OperationsContext';

export default function OperationsTracker() {
    const ops = useOperations();

    return (
        <Fragment>
            <div className="col-auto">
                <span className="badge badge-primary">{Object.keys(ops.operations).length} operations active</span>
            </div>
            {Object.entries(ops.operations).map( ([key, o]) =>
                <div key={key} className="col-auto">
                    <span className="badge badge-primary">{o.message}</span>
                </div>
            )}
        </Fragment>
    );
}
