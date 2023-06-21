import React from 'react';

const CardCacheContext = React.createContext();

function CardCacheProvider(props) {
    const [cache, dispatch] = React.useReducer(CardCacheReducer, {})
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
    const MyContext = React.useContext(CardCacheContext)
    if (!MyContext) {
        throw new Error(`useCardCache must be within a CardCacheProvider`)
    }
    return MyContext
}

export {
    useCardCache,
    CardCacheProvider,
    CardCacheReducer,
    CardCacheContext
}
