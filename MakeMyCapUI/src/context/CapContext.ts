import { createContext, useContext } from 'react';
import { CapContextData } from './CapContextData';


export const CapContext = createContext<CapContextData | undefined>(undefined);

export function useCapContext(): CapContextData {
    const data = useContext(CapContext);
    if (!data) {
        throw new Error('useCapContext() must be used with a SharedContext (are you missing a CapContext.Provider?)');
    }
    return data;
}
