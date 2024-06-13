export class UserDto {
    public readonly login: string;
    public readonly code: string;
    public readonly email: string;
    public readonly name: string;
    public readonly nextSequence: number;
    public readonly admin: boolean;
    public readonly artboardPath: string;
    public readonly artifactPath: string;
    public readonly outputPath: string;

    constructor(login: string, code: string, name: string, email: string, nextSequence: number, admin: boolean, artboardPath: string, artifactPath: string, outputPath: string) {
        this.login = login;
        this.code = code;
        this.email = email;
        this.nextSequence = nextSequence;
        this.admin = admin;
        this.name = name;
        this.artboardPath = artboardPath;
        this.artifactPath = artifactPath;
        this.outputPath = outputPath;
    }
}
