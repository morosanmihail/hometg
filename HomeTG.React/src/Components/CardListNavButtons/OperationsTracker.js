import React, { Fragment } from 'react';
import { useOperations } from '../../OperationsContext';

export default function OperationsTracker() {
    const ops = useOperations();

    return (
        <Fragment>
            <span className="badge bg-secondary badge-primary">{Object.keys(ops.operations).length} operations active</span>
            {Object.entries(ops.operations).map( ([key, o]) =>
                <span key={key} className="badge bg-secondary badge-primary">{o.message}</span>
            )}
        </Fragment>
    );
}
