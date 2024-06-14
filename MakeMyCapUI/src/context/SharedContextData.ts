import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { PersistData } from '../utils/storage/PersistData';


export class UserChangeEndpoint {

    onUserChange: (loggedIn: boolean) => Promise<void>;

    constructor(onUserChange: (loggedIn: boolean) => Promise<void>) {
        this.onUserChange = onUserChange;
    }
}


export class SharedContextData {

    private authenticatedUser: AuthenticatedUser | null = null;
    private registeredEndpoints: UserChangeEndpoint[] = [];

    public getAuthenticatedUser(): AuthenticatedUser | null {
        return this.authenticatedUser;
    }

    public setAuthenticatedUser(user: AuthenticatedUser | null) {
        if (user && user.token) {
            this.updateCacheFor(user)
        } else {
            this.clearCache();
        }
        this.authenticatedUser = user;
        this.notifyEndpointsOfUserChange(user != null);
    }

    private notifyEndpointsOfUserChange(loggedIn: boolean) {
        this.registeredEndpoints.forEach((endpoint) => {
            try {
                endpoint.onUserChange(loggedIn).catch(() => {console.log("Error while processing endpoint notification")});
            } catch (e) {
                console.log("Error while calling endpoint for notification");
            }
        })
    }

    public registerUserChangeNotificationEndpoint(endpoint: UserChangeEndpoint) {
        this.registeredEndpoints.push(endpoint);
    }

    public deregisterUserChangeNotificationEndpoint(endpointToRemove: UserChangeEndpoint) {
        this.registeredEndpoints = this.registeredEndpoints.filter((endpoint) => endpointToRemove !== endpoint);
    }

    public checkForCachedAuthenticatedUser(): AuthenticatedUser | null {
        const token = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_TOKEN);
        const userName = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_NAME);
        const email = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_EMAIL);

        if (!token || !userName || !email) {
            return null;
        }
        return new AuthenticatedUser(token, userName, email);
    }

    public clearCachedAuthenticatedUser() {
        this.clearCache();
    }

    private updateCacheFor(user: AuthenticatedUser) {
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_TOKEN, user.token);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_NAME, user.userName);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_EMAIL, user.email);
    }

    private clearCache() {
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_TOKEN);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_NAME);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_EMAIL);
    }
}
