import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { ApiHelper } from './ApiHelper';
import { UserDto } from './dto/UserDto';
import { PasswordDto } from './dto/PasswordDto';

export class AdminApi {

    public static async loadUsers(authenticatedUser: AuthenticatedUser): Promise<UserDto[] | null> {
        return fetch(ApiHelper.CreateApiUrl('Admin', 'users'), {
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
                        return AdminApi.decodeUsersResponse(response);
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

    private static decodeUsersResponse(json: any): UserDto[] {
        const response: UserDto[] = [];

        const users = Object(json)["users"];
        for (let key in users) {
            const user = users[key];
            response.push(AdminApi.decodeUserResponse(user));
        }
        return response;
    }

    private static decodeUserResponse(json: any): UserDto {

        const userName = json["userName"];
        const email = json["email"];
        const createDate = json["createDate"];
        return new UserDto(userName, email, createDate);
    }

    public static async createNewUser(user: UserDto, authenticatedUser: AuthenticatedUser): Promise<UserDto | null> {
        return fetch(ApiHelper.CreateApiUrl('Admin', 'create-user'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify({
                userName: user.userName,
                email: user.email,
            })
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`New user created for ${user.userName}`)
                    const response = await data.json();
                    if (response) {
                        return AdminApi.decodeUserResponse(response);
                    }
                }
                console.log(`New user not created for ${user.userName}`)
                return null;
            })
            .catch(err => {
                console.log(`Caught exception creating user for ${user.userName}`);
                console.log(err);
                return null;
            });
    }

    // public static async updateUser(user: UserDto, authenticatedUser: AuthenticatedUser): Promise<boolean> {
    //
    //     return fetch(ApiHelper.CreateApiUrl('Users', 'update_user'), {
    //         method: 'POST',
    //         headers: {
    //             'Content-Type': 'application/json',
    //             'X-Api-Key': ApiHelper.GetApiKey(),
    //             'Authorization': ApiHelper.GetBearer(authenticatedUser)
    //         },
    //         body: JSON.stringify(user)
    //     })
    //         .then(async data => {
    //             if (data.ok) {
    //                 console.log(`User record updated for ${user.login}`)
    //                 return true;
    //             }
    //             console.log(`User record not updated for ${user.login}`)
    //             return false;
    //         })
    //         .catch(err => {
    //             console.log(`Caught exception updating user record for ${user.login}`);
    //             console.log(err);
    //             return false;
    //         });
    // }
    //
    // public static async setPassword(login: string, password: string, authenticatedUser: AuthenticatedUser): Promise<boolean> {
    //     return fetch(ApiHelper.CreateApiUrl('Users', 'admin_password_reset'), {
    //         method: 'POST',
    //         headers: {
    //             'Content-Type': 'application/json',
    //             'X-Api-Key': ApiHelper.GetApiKey(),
    //             'Authorization': ApiHelper.GetBearer(authenticatedUser)
    //         },
    //         body: JSON.stringify(new PasswordDto(login, password))
    //     })
    //         .then(async data => {
    //             if (data.ok) {
    //                 console.log(`Password updated for ${login}`)
    //                 return true;
    //             }
    //             console.log(`Password not updated for ${login}`)
    //             return false;
    //         })
    //         .catch(err => {
    //             console.log(`Caught exception updating password for ${login}`);
    //             console.log(err);
    //             return false;
    //         });
    // }
}
