import { createContext, useContext, useReducer } from 'react';

export function SelectedCardsProvider({children}) {
    const [selected, selectedDispatch] = useReducer(
        selectedCardsReducer, []
    );

    return (
        <SelectedCardsContext.Provider value={selected}>
            <SelectedCardsDispatchContext.Provider value={selectedDispatch}>
                {children}
            </SelectedCardsDispatchContext.Provider>
        </SelectedCardsContext.Provider>
    );
}

const SelectedCardsContext = createContext([]);
export function useSelectedCards() {
    return useContext(SelectedCardsContext);
}

const SelectedCardsDispatchContext = createContext(null);
export function useSelectedCardsDispatch() {
    return useContext(SelectedCardsDispatchContext);
}

function selectedCardsReducer(selected, action) {
    switch (action.type) {
        case 'added': {
            return [...selected, action.card];
        }
        case 'deleted': {
            return selected.filter(t => t.id !== action.card.id);
        }
        case 'empty': {
            return [];
        }
        default: {
            throw Error('Unknown action: ' + action.type);
        }
    }
}
