import { User } from './User';

export class AuthenticatedUser {
    public readonly userName: string;
    public readonly code: string;
    public readonly token: string;
    public readonly email: string;
    public readonly name: string;
    public nextSequence: number;
    public artboardPath: string;
    public artifactPath: string;
    public outputPath: string;

    public static STORED_AUTHENTICATED_USER_TOKEN = 'ArtworkGenToken';
    public static STORED_AUTHENTICATED_USER_NAME = 'ArtworkGenUser';
    public static STORED_AUTHENTICATED_USER_CODE = 'ArtworkGenCode';
    public static STORED_AUTHENTICATED_USER_EMAIL = 'ArtworkGenEmail';
    public static STORED_AUTHENTICATED_USER_FULLNAME = 'ArtworkGenFullName';
    public static STORED_AUTHENTICATED_USER_NEXTSEQUENCE = 'ArtworkGenNextSequence';
    public static STORED_AUTHENTICATED_USER_ARTBOARDPATH = 'ArtworkGenArtboardPath';
    public static STORED_AUTHENTICATED_USER_ARTIFACTPATH = 'ArtworkGenArtifactPath';
    public static STORED_AUTHENTICATED_USER_OUTPUTPATH = 'ArtworkGenOutputPath';

    constructor(token: string, userName: string, code: string, email: string, name: string, artboardPath: string, artifactPath: string, outputPath: string, nextSequence: number | null) {
        this.token = token;
        this.userName = userName;
        this.code = code;
        this.email = email;
        this.name = name;
        this.artboardPath = artboardPath;
        this.artifactPath = artifactPath;
        this.outputPath = outputPath;
        this.nextSequence = !nextSequence ? 1 : nextSequence;
    }

    public extractUser(): User {
        return new User(this.userName, this.code, this.email, this.name);
    }
}
