import React, { useEffect, useState } from 'react';
import {
    Route,
    Navigate,
    createBrowserRouter,
    createRoutesFromElements,
    RouterProvider,
} from 'react-router-dom';
import './App.css';
import Home from '../../pages/home/Home';
import NotFound from '../error/NotFound';
import Login from '../../pages/login/Login';
import Logoff from '../../pages/login/Logoff';
import GuardedRoute from '../../security/route/GuardedRoute';
import { LoginApi } from '../../api/LoginApi';
import { SharedContextData } from '../../context/SharedContextData';
import RootLayout from '../../layout/RootLayout';
import { SharedContext } from '../../context/SharedContext';
import PasswordChangeInstructions from '../../pages/login/PasswordChangeInstructions';
import ChangePassword from '../../pages/login/ChangePassword';
import RequestPasswordChange from '../../pages/login/RequestPasswordChange';
import LayoutSelector from '../../layout/LayoutSelector';


// For Bootstrap: https://blog.logrocket.com/using-bootstrap-react-tutorial-examples/

const router = createBrowserRouter (
    createRoutesFromElements (
        <Route path="/" element={<LayoutSelector />}>
            <Route path="/" element={<Home />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/Logoff" element={<Logoff />} />
            <Route path="/RequestPasswordChange" element={<RequestPasswordChange />} />
            <Route path="/PasswordChangeInstructions" element={<PasswordChangeInstructions />} />
            <Route path="/ChangePassword" element={<ChangePassword />} />

            {/*<Route path="/RequestNewJob" element={<GuardedRoute />}>*/}
            {/*    <Route path="/RequestNewJob" element={<RequestNewJob />} />*/}
            {/*</Route>*/}
            {/*<Route path="/MyJobs" element={<GuardedRoute />}>*/}
            {/*    <Route path="/MyJobs" element={<MyJobs />} />*/}
            {/*</Route>*/}
            {/*<Route path="/EditSettings" element={<GuardedRoute />}>*/}
            {/*    <Route path="/EditSettings" element={<EditSettings />} />*/}
            {/*</Route>*/}

            <Route path="/NotFound" element={<NotFound />} />
            <Route path="*" element={<Navigate to="/NotFound" />} />
        </Route>
    )
)

const sharedContextData = new SharedContextData();

export default function App() {

    const [loadInProgress, setLoadInProgress] = useState<boolean>(true);
    const [loginStateChanged, setLoginStateChanged] = useState<boolean>(false);

    const checkAndLoadExistingUser = async () => {
        if (!sharedContextData.getAuthenticatedUser()) {
            const cachedUser = sharedContextData.checkForCachedAuthenticatedUser();
            if (cachedUser) {
                if (await LoginApi.validateToken(cachedUser.token)) {
                    //console.log("Found cached user");
                    sharedContextData.setAuthenticatedUser(cachedUser);
                } else {
                    sharedContextData.clearCachedAuthenticatedUser();
                }
            }
        }
        if (loadInProgress) {
            setLoadInProgress(false);
        }
    }

    useEffect(() => {

    }, [loginStateChanged]);

    useEffect(() => {
        checkAndLoadExistingUser().catch(console.error);
    }, []);

    useEffect(() => {

        // See: https://via.studio/journal/hosting-a-reactjs-app-with-routing-on-aws-s3 for server-side routing
        // Fixes redirections from server-side
        // modified to use router 6.x instead of the old history object
        const path = (/#!(\/.*)$/.exec(location.hash) || [])[1];
        if (path) {
            router.navigate(path, {replace: true});
        }
    }, [])


    return (
        <div>
            {loadInProgress && <div className="ca-spinner-box"><h1><span className="ca-blue">Preparing Make My Cap Tools...</span>
            </h1><div className="spinner-border text-primary"><span className="sr-only"></span></div></div>}
            {!loadInProgress &&
                <SharedContext.Provider value={sharedContextData}>
                    <RouterProvider router={router} />
                </SharedContext.Provider>
            }
        </div>
    );
}

