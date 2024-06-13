import { ApiHelper } from './ApiHelper';
import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { Thread } from '../model/Thread';

export class ThreadApi {

    public static async loadThreadsLookup(authenticatedUser: AuthenticatedUser): Promise<Thread[] | null> {

        return fetch(ApiHelper.CreateApiUrl('get_threads'), {
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
                        return ThreadApi.decodeResponse(response);
                    }
                }
                console.log('Get threads request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting threads');
                console.log(err);
                return null;
            });
    }

    private static decodeResponse(json: any): Thread[] {
        const response: Thread[] = [];

        const threads: object[] = Object(json)["threads"];
        for (let key in threads) {
            const thread = threads[key];
            const code = Object(thread)["code"];
            const colorName = Object(thread)["colorName"];
            const rgb = Object(thread)["rgb"];
            response.push(new Thread(code, colorName, rgb));
        }
        return response;
    }
}
