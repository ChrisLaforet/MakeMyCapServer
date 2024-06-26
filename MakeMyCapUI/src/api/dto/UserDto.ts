export class UserDto {
    public readonly userName: string;
    public readonly email: string;
    public readonly createDate: string;

    constructor(userName: string, email: string, createDate: string) {
        this.userName = userName;
        this.email = email;
        this.createDate = createDate;
    }
}
