import { createContext, useContext, useReducer } from 'react';

export const pageSize = 12;

export function CardsProvider({children}) {
    const [cards, cardsDispatch] = useReducer(
        cardsReducer, []
    );

    return (
        <CardsContext.Provider value={cards}>
            <CardsDispatchContext.Provider value={cardsDispatch}>
                {children}
            </CardsDispatchContext.Provider>
        </CardsContext.Provider>
    );
}

const CardsContext = createContext([]);
export function useCards() {
    return useContext(CardsContext);
}

const CardsDispatchContext = createContext(null);
export function useCardsDispatch() {
    return useContext(CardsDispatchContext);
}

function cardsReducer(cards, action) {
    switch (action.type) {
        case 'added': {
            let updated = false;
            const newCards = cards.map(c => {
                if (c.id === action.card.id) {
                    updated = true;
                    return action.card;
                }
                return c;
            })
            if (updated) {
                return newCards;
            }
            return cards;
        }
        case 'deleted': {
            return cards.filter(t => t.id !== action.card.id);
        }
        case 'overwrite': {
            return action.cards;
        }
        default: {
            throw Error('Unknown action: ' + action.type);
        }
    }
}
