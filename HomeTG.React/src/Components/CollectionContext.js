import { createContext, useContext, useState, useEffect, useReducer } from 'react';
import { useOperations } from '../OperationsContext';
import { useParams } from "react-router-dom";

export function CollectionsProvider({children}) {
    const { collection = "Main", pageNumber = 1 } = useParams();
    const [collectionsLoading, setCollectionsLoading] = useState(true);

    const [collections, collectionsDispatch] = useReducer(
        collectionsReducer, []
    );

    const ops = useOperations();

    useEffect(() => {
        ops.fetch("Listing collections", [], '/collection/list').then(data => {
            collectionsDispatch({
                type: 'overwrite',
                collections: data,
            });
            setCollectionsLoading(false);
        });
    }, [collection])

    return (
        <CollectionContext.Provider value={collection}>
            <PageNumberContext.Provider value={pageNumber}>
                <CollectionsContext.Provider value={collections}>
                    <CollectionsDispatchContext.Provider value={collectionsDispatch}>
                        {children}
                    </CollectionsDispatchContext.Provider>
                </CollectionsContext.Provider>
            </PageNumberContext.Provider>
        </CollectionContext.Provider>
    );
}

const CollectionContext = createContext("Main");
export function useCollection() {
    return useContext(CollectionContext);
}

const PageNumberContext = createContext(0);
export function usePageNumber() {
    return useContext(PageNumberContext);
}

const CollectionsContext = createContext([]);
export function useCollections() {
    return useContext(CollectionsContext);
}

const CollectionsDispatchContext = createContext(null);
export function useCollectionsDispatch() {
    return useContext(CollectionsDispatchContext);
}

function collectionsReducer(collections, action) {
    switch (action.type) {
        case 'added': {
            return [...collections, action.item];
        }
        case 'addrange': {
            return [...collections, ...action.collections];
        }
        case 'overwrite': {
            return [...action.collections];
        }
        case 'deleted': {
            return collections.filter(t => t.id !== action.id);
        }
        default: {
            throw Error('Unknown action: ' + action.type);
        }
    }
}
