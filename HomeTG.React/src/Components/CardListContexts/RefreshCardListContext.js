import { createContext, useContext, useState } from 'react';

export const RefreshCardListContext = createContext(null);
export function useRefreshCardList() {
    return useContext(RefreshCardListContext);
}

export const RefreshContext = createContext(null);
export function useRefresh() {
    return useContext(RefreshContext);
}

export function RefreshCardListProvider({children}) {
    const [refresh, setRefresh] = useState(false);

    return (
        <RefreshContext.Provider value={refresh}>
            <RefreshCardListContext.Provider value={setRefresh}>
                {children}
            </RefreshCardListContext.Provider>
        </RefreshContext.Provider>
    )
}
