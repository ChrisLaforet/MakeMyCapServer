export class User {
    public readonly userName: string;
    public readonly code: string;
    public readonly email: string;
    public readonly name: string;

    constructor(userName: string, code: string, email: string, name: string) {
        this.userName = userName;
        this.code = code;
        this.email = email;
        this.name = name;
    }
}
