import { createContext, useState, useContext } from 'react';
import uuid from 'react-native-uuid';

export const OperationsContext = createContext({});

export function OperationsProvider({children}) {
    const [operations, setOperations] = useState({});

    const addOperation = (key, operation) => {
        setOperations(prev => { return { ...prev, [key]: operation } });
    }

    const removeOperation = (key) => {
        setOperations(prev => {
            const copy = { ...prev };
            delete copy[key];
            return copy;
        });
    }

    const opsFetch = async (message, defaultValue, ...args) => {
        let opId = uuid.v4();
        addOperation(opId, { message: message });
        const result = await fetch(...args).then(response => {
            if(response.status === 200) {
                return response.json();
            } else {
                console.log("Halp. Failed to get successful response from " + response.url);
                return defaultValue;
            }
        });
        removeOperation(opId);
        return result;
    }


    return (
        <OperationsContext.Provider value={{ operations: operations, fetch: opsFetch }}>
            {children}
        </OperationsContext.Provider>
    );
}

export function useOperations() {
    return useContext(OperationsContext);
}
