export class User {
    public readonly userName: string;
    public readonly email: string;

    constructor(userName: string, email: string) {
        this.userName = userName;
        this.email = email;
    }
}
