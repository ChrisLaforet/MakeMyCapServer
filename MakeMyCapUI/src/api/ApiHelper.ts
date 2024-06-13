import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';

export class ApiHelper {

    // TODO: CML - determine a valid way to set paths for DEV/TEST/PROD builds
    public static rootPath = 'https://xl6whcule2.execute-api.us-east-1.amazonaws.com/Prod';

    public static GetApiKey(): string {
        return '8dc1e5a9-8337-40de-adcf-57b5b60a565a';
    }

    public static GetBearer(authenticatedUser: AuthenticatedUser): string {
        return ApiHelper.FormatBearer(authenticatedUser.token);
    }

    public static FormatBearer(token: string): string {
        return `Bearer ${token}`;
    }

    public static CreateApiUrl(endpoint: string): string {
        return `${ApiHelper.rootPath}/${endpoint}`
    }
}
