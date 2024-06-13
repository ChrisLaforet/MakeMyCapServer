import { Navigate, Outlet } from 'react-router-dom';
import { JwtHelper } from '../jwt/JwtHelper';
import { useSharedContext } from '../../context/SharedContext';

// See: https://dev.to/iamandrewluca/private-route-in-react-router-v6-lg5
// See: https://stackoverflow.com/questions/69864165/error-privateroute-is-not-a-route-component-all-component-children-of-rou

export default function GuardedAdministratorRoute() {

    const sharedContextData = useSharedContext();

    function isUserAuthenticated(): boolean {

        const user = sharedContextData.getAuthenticatedUser();
        if (!user || !sharedContextData.isUserAnAdministrator()) {
            return false;
        }

        return JwtHelper.validateTokenExpirationIsValid(user.token);
    }

    return (isUserAuthenticated() ? <Outlet /> : <Navigate to='/login' /> );
}
