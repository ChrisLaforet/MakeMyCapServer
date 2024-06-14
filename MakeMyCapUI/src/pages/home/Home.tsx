import React, { useEffect, useState } from 'react';
import { useSharedContext } from '../../context/SharedContext';


export default function Home() {

    const sharedContextData = useSharedContext();

    const [loggedIn, setLoggedIn] = useState<boolean>(false)

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            setLoggedIn(true)
        }

    }, []);


    return(
        <div>
            <h1>HOME MODULE - FIX!!</h1>
        </div>
    );
}
