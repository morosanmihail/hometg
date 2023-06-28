import React from 'react';
import Search from "../Components/Search";
import ViewProviders from './ViewProviders';

export default function SearchView() {
    return (
        <ViewProviders>
            <Search startSearch={false} dedicatedPage={true} />
        </ViewProviders>
    )
}
