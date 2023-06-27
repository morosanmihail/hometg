import { createContext, useContext, useReducer } from 'react';

const CardCacheContext = createContext();

function CardCacheProvider(props) {
    const [cache, dispatch] = useReducer(CardCacheReducer, {})
    const value = [cache, dispatch]

    return (
        <CardCacheContext.Provider value={value} {...props} />
    )
}

function CardCacheReducer(state, action) {
    switch (action.type) {
        case 'ADD-TO-CACHE':
            return { ...state, [action.id]: action.data }
        default: {
            throw new Error(`unhandled action type ${action.type}`)
        }
    }
}

function useCardCache() {
    return useContext(CardCacheContext);
}

export {
    useCardCache,
    CardCacheProvider,
    CardCacheReducer,
    CardCacheContext
}
