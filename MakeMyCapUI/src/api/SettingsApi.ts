import { ApiHelper } from './ApiHelper';
import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { SettingsDto } from './dto/SettingsDto';
import { NotificationEmailsDto } from './dto/NotificationEmailsDto';


export class SettingsApi {

    public static async getSettings(authenticatedUser: AuthenticatedUser): Promise<SettingsDto | null> {

        return fetch(ApiHelper.CreateApiUrl('Settings','settings'), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            }
        })
            .then(async data => {
                if (data.ok) {
                    const response = await data.json();
                    if (response) {
                        return SettingsApi.decodeSettingsResponse(response);
                    }
                }
                console.log('Get settings request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting settings');
                console.log(err);
                return null;
            });
    }

    private static decodeSettingsResponse(json: any): SettingsDto {
        const values = Object(json)["settings"];
        const inventoryCheckHours = values["inventoryCheckHours"];
        const fulfillmentCheckHours = values["fulfillmentCheckHours"];
        const nextPoSequence = values["nextPoSequence"];

        return new SettingsDto(inventoryCheckHours, fulfillmentCheckHours, nextPoSequence);
    }

    public static async getNotifications(authenticatedUser: AuthenticatedUser): Promise<NotificationEmailsDto | null> {

        return fetch(ApiHelper.CreateApiUrl('Settings','notifications'), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            }
        })
            .then(async data => {
                if (data.ok) {
                    const response = await data.json();
                    if (response) {
                        return SettingsApi.decodeNotificationsResponse(response);
                    }
                }
                console.log('Get notification emails request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting notification emails');
                console.log(err);
                return null;
            });
    }

    private static decodeNotificationsResponse(json: any): NotificationEmailsDto {
        const values = Object(json)["notificationEmails"];

        const warningEmail1 = values["warningEmail1"];
        const warningEmail2 = values["warningEmail2"];
        const warningEmail3 = values["warningEmail3"];
        const criticalEmail1 = values["criticalEmail1"];
        const criticalEmail2 = values["criticalEmail2"];
        const criticalEmail3 = values["criticalEmail3"];

        return new NotificationEmailsDto(warningEmail1, warningEmail2, warningEmail3, criticalEmail1, criticalEmail2, criticalEmail3);
    }
}
