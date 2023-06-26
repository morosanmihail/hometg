import React from 'react';
import Sidebar from "./Components/Sidebar";
import CardList from "./Components/CardList";
import { CollectionsProvider } from './Components/CollectionContext';
import { OperationsProvider } from './OperationsContext';

export default function BaseApp({ showSearch = false }) {
    return (
        <OperationsProvider>
            <CollectionsProvider>
                <header>
                    <Sidebar/>
                    <nav id="main-navbar" className="navbar navbar-expand-lg navbar-light bg-white fixed-top">
                        <div className="container-fluid">
                            <a className="navbar-brand" href="/">
                                HomeTG
                            </a>
                        </div>
                    </nav>
                </header>
                <main>
                    <CardList showSearch={showSearch} />
                </main>
            </CollectionsProvider>
        </OperationsProvider>
    );
}
