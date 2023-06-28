import React from 'react';
import Search from "../Components/Search";
import { CardCacheProvider } from '../Components/CardListContexts/CardCacheContext';
import { CardLoaderProvider } from '../Components/CardListContexts/CardLoaderContext';
import { CardSetsProvider } from '../Components/ReusableConstants/CardSets';
import { SelectedCardsProvider } from '../Components/CardListContexts/SelectedCardsContext';
import { CardsProvider } from '../Components/CardListContexts/CardsContext';
import { CollectionsProvider } from '../Components/CollectionContext';
import Header from '../Components/Layout/Header';

export default function SearchView() {
    return (
        <CollectionsProvider>
            <Header/>
            <main>
                <CardsProvider>
                    <SelectedCardsProvider>
                        <CardCacheProvider>
                            <CardLoaderProvider>
                                <CardSetsProvider>
                                    <Search dedicatedPage={true} />
                                </CardSetsProvider>
                            </CardLoaderProvider>
                        </CardCacheProvider>
                    </SelectedCardsProvider>
                </CardsProvider>
            </main>
        </CollectionsProvider>
    )
}
