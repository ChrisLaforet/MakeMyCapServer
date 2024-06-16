import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';

export class ApiHelper {

    // TODO: CML - determine a valid way to set paths for DEV/TEST/PROD builds
    //public static rootPath = 'https://c42.makemycap.com/api';
    public static rootPath = 'https://localhost:7266';

    public static GetApiKey(): string {
        return '1a795f68-08d5-476e-82b1-d4d0b92ae7a0';
    }

    public static GetBearer(authenticatedUser: AuthenticatedUser): string {
        return ApiHelper.FormatBearer(authenticatedUser.token);
    }

    public static FormatBearer(token: string): string {
        return `Bearer ${token}`;
    }

    public static CreateApiUrl(controller: string, endpoint: string): string {
        return `${ApiHelper.rootPath}/${controller}/${endpoint}`
    }
}
