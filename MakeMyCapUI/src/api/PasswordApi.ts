import { ApiHelper } from './ApiHelper';

export class PasswordApi {

    public static async requestPasswordReset(username: string): Promise<null> {
        const body = {
            username
        };

        return fetch(ApiHelper.CreateApiUrl('User', 'request_password_reset'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey()
            },
            body: JSON.stringify(body)
        })
            .then(() => {
                console.log(`Password reset request completed for ${username}`)
                Promise.resolve();
                return null;
            })
            .catch(err => {
                console.log(`Caught exception requesting password reset for ${username}, err`);
                Promise.resolve();
                return null;
            });
    }

    public static async requestPasswordChange(username: string, confirmationCode: string, password: string): Promise<null> {
        const body = {
            username,
            confirmationCode,
            password
        };

        return fetch(ApiHelper.CreateApiUrl('User', 'execute_password_reset'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey()
            },
            body: JSON.stringify(body)
        })
            .then(() => {
                console.log(`Password change request completed for ${username}`)
                Promise.resolve();
                return null;
            })
            .catch(err => {
                console.log(`Caught exception requesting password change for ${username}`, err);
                Promise.resolve();
                return null;
            });
    }
}
