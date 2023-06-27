import React, { createContext, useContext } from 'react';
import { useOperations } from '../../OperationsContext';
import { useCardCache } from './CardCacheContext';

const CardLoaderContext = createContext(null);
export function useCardLoader() {
    return useContext(CardLoaderContext);
}

export function CardLoaderProvider({children}) {
    const DataLoader = require('dataloader');
    const ops = useOperations();
    const [cache, dispatch] = useCardCache();

    async function batchFunction(keys) {
        let urlParams = keys.join("&ids=");
        const results = await ops.fetch("Bulk updating details for cards", {}, '/mtg/cards?ids=' + urlParams).
            then(data => data);
        return keys.map(key => results[key] || new Error(`No card for ${key}`));
    }

    const cardLoader = new DataLoader(keys => batchFunction(keys));

    async function loadCard(id) {
        if (cache[id]) {
            return cache[id];
        }
        const card = await cardLoader.load(id);
        dispatch({ type: 'ADD-TO-CACHE', id: id, data: card });
        return card;
    }

    return (
        <CardLoaderContext.Provider value={loadCard}>
            {children}
        </CardLoaderContext.Provider>
    )
}
