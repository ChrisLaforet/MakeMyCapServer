import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { PersistData } from '../utils/storage/PersistData';
import { JwtRoleExtractor } from '../security/jwt/JwtRoleExtractor';
import { JobRequest } from '../model/JobRequest';


export class UserChangeEndpoint {

    onUserChange: (loggedIn: boolean) => Promise<void>;

    constructor(onUserChange: (loggedIn: boolean) => Promise<void>) {
        this.onUserChange = onUserChange;
    }
}


export class SharedContextData {

    public static ADMIN_ROLE_NAME = "AdminPermitted";

    private authenticatedUser: AuthenticatedUser | null = null;
    private isAdministrator = false;
    private registeredEndpoints: UserChangeEndpoint[] = [];

    private jobRequest: JobRequest | null = null;

    public getAuthenticatedUser(): AuthenticatedUser | null {
        return this.authenticatedUser;
    }

    public setAuthenticatedUser(user: AuthenticatedUser | null) {
        if (user && user.token) {
            this.updateCacheFor(user)
            this.isAdministrator = JwtRoleExtractor.getRoles(user.token).includes(SharedContextData.ADMIN_ROLE_NAME);
        } else {
            this.clearCache();
            this.isAdministrator = false;
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

    public isUserAnAdministrator(): boolean {
        return this.authenticatedUser != null && this.isAdministrator;
    }

    public getNextSequence(): number {
        if (this.authenticatedUser) {
            return this.authenticatedUser.nextSequence;
        }
        return 1;
    }

    public updateNextSequence(value: number) {
        if (this.authenticatedUser) {
            this.authenticatedUser.nextSequence = value;
            this.updateCacheFor(this.authenticatedUser);
        }
    }

    public checkForCachedAuthenticatedUser(): AuthenticatedUser | null {
        const token = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_TOKEN);
        const userName = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_NAME);
        const code = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_CODE);
        const email = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_EMAIL);
        const name = PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_FULLNAME);
        const artboardPath= PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_ARTBOARDPATH);
        const artifactPath= PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_ARTIFACTPATH);
        const outputPath= PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_OUTPUTPATH);

        const value= PersistData.get(AuthenticatedUser.STORED_AUTHENTICATED_USER_NEXTSEQUENCE);

        let nextSequence = 1;
        if (value && value != '') {
            nextSequence = parseInt(value);
        }
        if (!token || !userName || !code || !email || !name) {
            return null;
        }
        return new AuthenticatedUser(token, userName, code, email, name, artboardPath ? artboardPath : '', artifactPath ? artifactPath : '', outputPath ? outputPath : '', nextSequence);
    }

    public clearCachedAuthenticatedUser() {
        this.clearCache();
    }

    private updateCacheFor(user: AuthenticatedUser) {
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_TOKEN, user.token);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_NAME, user.userName);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_CODE, user.code);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_EMAIL, user.email);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_FULLNAME, user.name);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_NEXTSEQUENCE, '' + user.nextSequence);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_ARTBOARDPATH, user.artboardPath);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_ARTIFACTPATH, user.artifactPath);
        PersistData.put(AuthenticatedUser.STORED_AUTHENTICATED_USER_OUTPUTPATH, user.outputPath);
    }

    private clearCache() {
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_TOKEN);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_NAME);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_CODE);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_EMAIL);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_FULLNAME);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_NEXTSEQUENCE);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_ARTBOARDPATH);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_ARTIFACTPATH);
        PersistData.delete(AuthenticatedUser.STORED_AUTHENTICATED_USER_OUTPUTPATH);
    }

    public setJobRequest(jobRequest: JobRequest) {
        this.jobRequest = jobRequest;
    }

    public getJobRequest(): JobRequest | null {
        return this.jobRequest;
    }

    public clearJobRequest() {
        this.jobRequest = null;
    }
}
