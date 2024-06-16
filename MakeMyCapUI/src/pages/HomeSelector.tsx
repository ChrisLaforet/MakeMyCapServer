import { useSharedContext } from '../context/SharedContext';
import Login from './login/Login';
import Home from './home/Home';

export default function HomeSelector() {

    const sharedContextData = useSharedContext();

    function isLoggedIn() {
        if (!sharedContextData) {
            console.log('Cannot find sharedContext at HomeSelector');
            return false;
        }

        const user = sharedContextData.getAuthenticatedUser();
        return user != null;
    }

    return(
        <>
            {
                isLoggedIn() ? <Home /> : <Login />
            }
        </>

    );
}
