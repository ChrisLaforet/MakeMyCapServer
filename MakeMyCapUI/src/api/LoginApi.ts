import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { ApiHelper } from './ApiHelper';

export class LoginApi {

    public static async loginUser(username: string, password: string): Promise<AuthenticatedUser | null> {
        const credentials = {
            username,
            password
        };
// console.log(JSON.stringify(credentials))
        return fetch(ApiHelper.CreateApiUrl('User', 'login'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey()
            },
            body: JSON.stringify(credentials)
        })
            .then(async data => {
                if (data.ok) {
                    const response = await data.json();
                    if (response) {
                        return new AuthenticatedUser(
                            Object(response)["token"],
                            Object(response)["username"],
                            Object(response)["email"],
                        );
                    }
                }
                console.log(`Login attempt failed for ${username}`)
                return null;
            })
            .catch(err => {
                console.log('Caught exception logging in');
                console.log(err);
                return null;
            });

    }

    public static async logoutUser(authenticatedUser: AuthenticatedUser): Promise<any> {

        return fetch(ApiHelper.CreateApiUrl('User','logout'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: '{}'
        })
            .then(() => {
                console.log(`Logout completed for ${authenticatedUser.userName}`)
                return Promise.resolve();
            })
            .catch(err => {
                console.log('Caught exception logging out');
                console.log(err);
                return Promise.resolve();
            });
    }

    public static async validateToken(token: string): Promise<boolean> {

        return fetch(ApiHelper.CreateApiUrl('User','validate_token'), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.FormatBearer(token)
            }
        })
            .then((response) => {
                if (response.ok) {
                    return Promise.resolve(true);
                }
                return Promise.resolve(false);
            })
            .catch(err => {
                console.log('Caught exception validating token');
                console.log(err);
                return Promise.resolve(false);
            });
    }
}
