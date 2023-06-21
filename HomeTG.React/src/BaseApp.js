import React, { Fragment, useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Sidebar from "./Components/Sidebar";
import CardList from "./Components/CardList";

function CollectionsList() {
    const { collection = "Main", offset = 0 } = useParams();
    const [collections, setCollections] = useState([]);
    const [collectionsLoading, setCollectionsLoading] = useState(true);

    useEffect(() => {
        fetch('/collection/list').then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCollections(data);
                    setCollectionsLoading(false);
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
                <CardList collections={collections} setCollections={setCollections} collection={collection} offset={offset} />
            </main>
        </Fragment>
    );
}

function BaseApp() {
    return (
        <CollectionsList/>
    );
}

export default BaseApp;
