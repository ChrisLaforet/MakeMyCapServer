import { createContext, useContext } from 'react';
import { SharedContextData } from './SharedContextData';

export const SharedContext = createContext<SharedContextData | undefined>(undefined);

export function useSharedContext(): SharedContextData {
    const data = useContext(SharedContext);
    if (!data) {
        throw new Error('useSharedContext() must be used with a SharedContext (are you missing a SharedContext.Provider?)');
    }
    return data;
}
