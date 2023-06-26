import React, { Fragment, useState } from 'react';
import { useOperations } from '../../OperationsContext';
import { useCollection } from '../CollectionContext';

export default function ImportCards({ setRefresh }) {
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
        formData.append("collection", collection);

        ops.fetch("Importing into " + collection, [], '/collection/import', {
            method: "post",
            body: formData,
        }).then(data => {setRefresh(true);});
    }

    return (
        <Fragment>
            <form className="d-flex">
                <div className="input-group">
                    <input onChange={handleFileChange} type="file" className="form-control" id="inputGroupFile02"/>
                    <button onClick={handleUploadClick} className="btn btn-outline-secondary" type="button" id="inputGroupFileAddon04">Import</button>
                </div>
            </form>
        </Fragment>
    );
}
