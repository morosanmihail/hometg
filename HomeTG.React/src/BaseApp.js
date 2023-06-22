import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Sidebar from "./Components/Sidebar";
import CardList from "./Components/CardList";
import uuid from 'react-native-uuid';
import { CollectionContext } from './Components/CollectionContext';
import { OperationsContext } from './OperationsContext';

export default function BaseApp() {
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

    const opsFetch = async (message, defaultValue, ...args) => {
        let opId = uuid.v4();
        addOperation(opId, { message: message });
        const result = await fetch(...args).then(response => {
            if(response.status === 200) {
                return response.json();
            } else {
                console.log("Halp");
                return defaultValue;
            }
        });
        removeOperation(opId);
        return result;
    }

    useEffect(() => {
        opsFetch("Listing collections", [], '/collection/list').then(data => {
            setCollections(data);
            setCollectionsLoading(false);
        });
    }, [collection])

    return (
        <OperationsContext.Provider value={
            { operations: operations, fetch: opsFetch }
        }>
            <CollectionContext.Provider value={collection}>
                <header>
                    <Sidebar collections={collections} setCollections={setCollections} loading={collectionsLoading} />

                    <nav id="main-navbar" className="navbar navbar-expand-lg navbar-light bg-white fixed-top">
                        <div className="container-fluid">
                            <a className="navbar-brand" href="/">
                                HomeTG
                            </a>
                        </div>
                    </nav>
                </header>
                <main>
                    <CardList collections={collections} offset={offset} />
                </main>
            </CollectionContext.Provider>
        </OperationsContext.Provider>
    );
}
