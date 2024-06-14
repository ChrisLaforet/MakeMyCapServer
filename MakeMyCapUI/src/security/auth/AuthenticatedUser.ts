import { User } from './User';

export class AuthenticatedUser {
    public readonly userName: string;
    public readonly token: string;
    public readonly email: string;

    public static STORED_AUTHENTICATED_USER_TOKEN = 'MMCToolsToken';
    public static STORED_AUTHENTICATED_USER_NAME = 'MMCToolsUser';
    public static STORED_AUTHENTICATED_USER_EMAIL = 'MMCToolsEmail';


    constructor(token: string, userName: string, email: string) {
        this.token = token;
        this.userName = userName;
        this.email = email;
    }

    public extractUser(): User {
        return new User(this.userName, this.email);
    }
}
