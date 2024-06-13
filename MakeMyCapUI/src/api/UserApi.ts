import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { ApiHelper } from './ApiHelper';
import { UserDto } from './dto/UserDto';
import { PasswordDto } from './dto/PasswordDto';

export class UserApi {

    public static async loadUsers(authenticatedUser: AuthenticatedUser): Promise<UserDto[] | null> {
        return fetch(ApiHelper.CreateApiUrl('get_users'), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
        })
            .then(async data => {
                if (data.ok) {
                    const response = await data.json();
                    if (response) {
                        return UserApi.decodeResponse(response);
                    }
                }
                console.log('Get users request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting users');
                console.log(err);
                return null;
            });
    }

    private static decodeResponse(json: any): UserDto[] {
        const response: UserDto[] = [];

        const users = Object(json)["users"];
        for (let key in users) {
            const user = users[key];
            const code = user["code"];
            const login = user["login"];
            const name = user["name"];
            const email = user["email"];
            const nextSequence: number = user["nextSequence"];
            const admin: boolean = user["admin"];
            const artboardPath = user["artboardPath"];
            const artifactPath = user["artifactPath"];
            const outputPath = user["outputPath"];
            response.push(new UserDto(login, code, name, email, nextSequence, admin, artboardPath, artifactPath, outputPath));
        }
        console.log(response);
        return response;
    }

    public static async createNewUser(user: UserDto, authenticatedUser: AuthenticatedUser): Promise<boolean> {
        return fetch(ApiHelper.CreateApiUrl('create_user'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify(user)
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`New user created for ${user.login}`)
                    return true;
                }
                console.log(`New user not created for ${user.login}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception creating user for ${user.login}`);
                console.log(err);
                return false;
            });
    }

    public static async updateUser(user: UserDto, authenticatedUser: AuthenticatedUser): Promise<boolean> {

        return fetch(ApiHelper.CreateApiUrl('update_user'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify(user)
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`User record updated for ${user.login}`)
                    return true;
                }
                console.log(`User record not updated for ${user.login}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception updating user record for ${user.login}`);
                console.log(err);
                return false;
            });
    }

    public static async setPassword(login: string, password: string, authenticatedUser: AuthenticatedUser): Promise<boolean> {
        return fetch(ApiHelper.CreateApiUrl('admin_password_reset'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify(new PasswordDto(login, password))
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`Password updated for ${login}`)
                    return true;
                }
                console.log(`Password not updated for ${login}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception updating password for ${login}`);
                console.log(err);
                return false;
            });
    }
}
