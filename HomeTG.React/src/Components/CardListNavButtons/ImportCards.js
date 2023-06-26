import React, { Fragment, useState } from 'react';
import { useOperations } from '../../OperationsContext';
import { useCollection } from '../CollectionContext';

export default function ImportCards() {
    const ops = useOperations();
    const collection = useCollection();

    const [file, setFile] = useState();

    const handleFileChange = (e) => {
        if (e.target.files) {
          setFile(e.target.files[0]);
        }
    };

    const handleUploadClick = () => {
        if (!file) {
            return;
        }

        const formData = new FormData();
        formData.append("file", file);

        ops.fetch("Importing into " + collection, [], '/collection/cards/' + collection + '/import', {
            method: "post",
            headers: {
                'Content-Type': file.type,
                'Content-Length': `${file.size}`,
            },
            body: formData,
            // body: file,
        }).then(data => {});
    }

    return (
        <Fragment>
            <div className="col-auto">
                <input type="file" onChange={handleFileChange} />
                <div>{file && `${file.name} - ${file.type}`}</div>
                <button type="button" className="btn btn-info" onClick={handleUploadClick}>Import</button>
            </div>
        </Fragment>
    );
}
