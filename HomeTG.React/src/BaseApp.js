import React from 'react';
import CardList from "./Components/CardList";
import { CollectionsProvider } from './Components/CollectionContext';
import { OperationsProvider } from './OperationsContext';
import { SelectedCardsProvider } from './Components/CardListContexts/SelectedCardsContext';
import Header from './Components/Layout/Header';

export default function BaseApp({ showSearch = false }) {
    return (
        <OperationsProvider>
            <CollectionsProvider>
                <Header/>
                <main>
                    <SelectedCardsProvider>
                        <CardList showSearch={showSearch} />
                    </SelectedCardsProvider>
                </main>
            </CollectionsProvider>
        </OperationsProvider>
    );
}
