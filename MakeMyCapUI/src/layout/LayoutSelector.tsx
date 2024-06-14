import { useSharedContext } from '../context/SharedContext';
import { useEffect, useRef, useState } from 'react';
import { UserChangeEndpoint } from '../context/SharedContextData';
import RootLayout from './RootLayout';
import BaseLayout from './BaseLayout';

export default function LayoutSelector() {

    const sharedContextData = useSharedContext();
    const onUserChangeCallback = useRef<UserChangeEndpoint | null>(null);
    const [userChanged, setUserChanged] = useState<boolean>(false);

    function isLoggedIn() {
        if (!sharedContextData) {
            console.log('Cannot find sharedContext at LayoutSelector');
            return false;
        }

        const user = sharedContextData.getAuthenticatedUser();
        return user != null;
    }

    async function onUserChange(isLoggedIn: boolean): Promise<void> {
        setUserChanged(isLoggedIn);
    }

    useEffect(() => {
        const handler = new UserChangeEndpoint(onUserChange);
        onUserChangeCallback.current = handler;
        sharedContextData.registerUserChangeNotificationEndpoint(handler);

        return () => {
            if (onUserChangeCallback.current) {
                sharedContextData.deregisterUserChangeNotificationEndpoint(onUserChangeCallback.current)
                onUserChangeCallback.current = null;
            }
        }
    }, [userChanged])

    return (
        <div>
            {
                isLoggedIn() ? <RootLayout /> : <BaseLayout />
            }
        </div>
    );
}
