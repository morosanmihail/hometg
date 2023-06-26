import React from 'react';
import Sidebar from "./Components/Sidebar";
import CardList from "./Components/CardList";
import { CollectionsProvider } from './Components/CollectionContext';
import { OperationsProvider } from './OperationsContext';
import { SelectedCardsProvider } from './Components/CardListContexts/SelectedCardsContext';

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
                    <SelectedCardsProvider>
                        <CardList showSearch={showSearch} />
                    </SelectedCardsProvider>
                </main>
            </CollectionsProvider>
        </OperationsProvider>
    );
}
