import React, { Fragment, useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Sidebar from "./Components/Sidebar";
import CardList from "./Components/CardList";
import uuid from 'react-native-uuid';

function BaseApp() {
    const { collection = "Main", offset = 0 } = useParams();
    const [collections, setCollections] = useState([]);
    const [collectionsLoading, setCollectionsLoading] = useState(true);
    const [operations, setOperations] = useState({});

    const addOperation = (key, operation) => {
        setOperations(prev => { return { ...prev, [key]: operation } });
    }

    const removeOperation = (key) => {
        setOperations(prev => {
            const copy = { ...prev };
            delete copy[key];
            return copy;
        });
    }

    useEffect(() => {
        let opId = uuid.v4();
        addOperation(opId, { message: "Listing collections" });
        fetch('/collection/list').then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCollections(data);
                    setCollectionsLoading(false);
                    removeOperation(opId);
                })
            }
        });
    }, [collection])

    return (
        <Fragment>
            <header>
                <Sidebar collections={collections} setCollections={setCollections} collection={collection} loading={collectionsLoading} />

                <nav id="main-navbar" className="navbar navbar-expand-lg navbar-light bg-white fixed-top">
                    <div className="container-fluid">
                        <a className="navbar-brand" href="/">
                            HomeTG
                        </a>
                    </div>
                </nav>
            </header>
            <main>
                <CardList collections={collections} collection={collection} offset={offset}
                    operations={operations} addOperation={addOperation} removeOperation={removeOperation} />
            </main>
        </Fragment>
    );
}

export default BaseApp;
