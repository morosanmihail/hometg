import React from 'react';
import Search from "../Components/Search";
import CardListNav from "../Components/CardListNav";
import CardList from '../Components/CardList';
import ViewProviders from './ViewProviders';

export default function CardListView({showSearch=false}) {
    return (
        <ViewProviders>
            <Search dedicatedPage={showSearch} />
            <CardListNav/>
            <CardList />
        </ViewProviders>
    )
}
