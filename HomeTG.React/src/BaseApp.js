import React from 'react';
import { OperationsProvider } from './OperationsContext';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import CardListView from './Views/CardListView';
import SearchView from './Views/SearchView';

export default function BaseApp() {
    return (
        <OperationsProvider>
            <BrowserRouter>
                <Routes>
                    <Route exact path="/" element={<CardListView />} />
                    <Route path="/search" element={<SearchView />} />
                    <Route path="/c/:collection" element={<CardListView />} />
                    <Route path="/c/:collection/:pageNumber" element={<CardListView />} />
                </Routes>
            </BrowserRouter>
        </OperationsProvider>
    );
}
