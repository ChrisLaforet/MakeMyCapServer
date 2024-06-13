import { jwtDecode, JwtPayload } from 'jwt-decode';
import { JwtRoleExtractor } from './JwtRoleExtractor';
import { AuthenticatedUser } from '../auth/AuthenticatedUser';

export class JwtHelper {

    public static validateTokenExpirationIsValid(token: string): boolean {
        try {
            const tokenPayload = jwtDecode<JwtPayload>(token);
            // @ts-ignore
            const expiration = parseInt(tokenPayload.exp, 10);
            const now = new Date();
            if (now.getTime() / 1000 <= expiration) {
                return true;
            }
            console.log('User token is expired - forcing logout');
            return false;
        } catch (e) {
            console.error('Invalid token format being validated: ' + e);
            return false;
        }
    }

    public static isUserAuthorizedFor(requiredRoles: string[], user: AuthenticatedUser): boolean {
        if (user == null) {
            return false;
        }
        // @ts-ignore
        if (!JwtHelper.validateTokenExpirationIsValid(user.token)) {
            return false;
        }
        if (requiredRoles.length == 0) {
            return true;
        }

        let notPermitted = true;
        // @ts-ignore
        const roles = JwtRoleExtractor.getRoles(user.token);
        for (const requiredRole of requiredRoles) {
            for (const role of roles) {
                if (requiredRole === role) {
                    notPermitted = false;
                    break;
                }
            }
            if (!notPermitted) {
                break;
            }
        }

        return !notPermitted;
    }
}
