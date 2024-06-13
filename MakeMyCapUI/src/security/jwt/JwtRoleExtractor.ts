import { jwtDecode, JwtPayload } from 'jwt-decode';

export class JwtRoleExtractor {

    public static getRoles(token: string): string[] {
        try {
            const tokenPayload: object = jwtDecode<JwtPayload>(token);
            // @ts-ignore
            const expiration = parseInt(tokenPayload.exp, 10);
            const now = new Date();
            // @ts-ignore
            if (now.getTime() / 1000 > expiration) {
                console.log('User token is expired');
                return [];
            }
            // @ts-ignore
            const roles: string[] = tokenPayload['roles'];
            if (roles !== undefined) {
                return roles;
            }
        } catch (e) {
            console.error('getRoles - Invalid token format being validated: ' + e);
        }
        return [];
    }

    private static validateTokenAndExtractRoles(token: string): string [] {
        const tokenPayload: object = jwtDecode<JwtPayload>(token);
        // @ts-ignore
        const expiration = parseInt(tokenPayload.exp, 10);
        const now = new Date();
        // @ts-ignore
        if (now.getTime() / 1000 > expiration) {
            console.log('User token is expired');
            throw new Error('Token is expired');
        }
        // @ts-ignore
        return tokenPayload['roles'];
    }

    public static isUserPermittedToViewMembers(token: string): boolean {
        try {
            const roles = JwtRoleExtractor.validateTokenAndExtractRoles(token);
            if (roles === undefined) {
                return false;
            }

            return roles.includes('Administrator') || roles.includes('Member') || roles.includes('Attender');

        } catch (e) {
            console.error('isUserPermittedToViewMembers - Invalid token format being validated: ' + e);
            return false;
        }
    }

    public static isUserAnAdministrator(token: string): boolean {
        try {
            const roles = JwtRoleExtractor.validateTokenAndExtractRoles(token);
            if (roles === undefined) {
                return false;
            }

            return roles.includes('Administrator');

        } catch (e) {
            console.error('isUserAnAdministrator - Invalid token format being validated: ' + e);
            return false;
        }
    }
}
