import React, { useState, useEffect, createContext, useContext } from 'react';
import { useOperations } from '../../OperationsContext';

const CardSetsContext = createContext([]);
export function useCardSets() {
    return useContext(CardSetsContext);
}

export function CardSetsProvider({children}) {
    const ops = useOperations();

    const [sets, setSets] = useState([]);

    useEffect(() => {
        ops.fetch(
            "Getting all available sets", [], '/mtg/sets'
        ).then(data => {
            setSets(["", ...data]);
        })
    }, []);

    return (
        <CardSetsContext.Provider value={sets}>
            {children}
        </CardSetsContext.Provider>
    );
}
