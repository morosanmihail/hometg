import React, { useState } from 'react';
import Search from "../Components/Search";
import CardListNav from "../Components/CardListNav";
import { CardCacheProvider } from '../Components/CardListContexts/CardCacheContext';
import { CardLoaderProvider } from '../Components/CardListContexts/CardLoaderContext';
import { CardSetsProvider } from '../Components/ReusableConstants/CardSets';
import CardList from '../Components/CardList';
import { SelectedCardsProvider } from '../Components/CardListContexts/SelectedCardsContext';
import { CardsProvider } from '../Components/CardListContexts/CardsContext';
import { CollectionsProvider } from '../Components/CollectionContext';
import Header from '../Components/Layout/Header';
import { RefreshCardListProvider } from '../Components/CardListContexts/RefreshCardListContext';

export default function CardListView({showSearch=false}) {
    return (
        <CollectionsProvider>
            <Header/>
            <main>
                <CardsProvider>
                    <SelectedCardsProvider>
                        <CardCacheProvider>
                            <CardLoaderProvider>
                                <RefreshCardListProvider>
                                    <CardSetsProvider>
                                        <Search dedicatedPage={showSearch} />
                                        <CardListNav/>
                                    </CardSetsProvider>
                                    <CardList />
                                </RefreshCardListProvider>
                            </CardLoaderProvider>
                        </CardCacheProvider>
                    </SelectedCardsProvider>
                </CardsProvider>
            </main>
        </CollectionsProvider>
    )
}
