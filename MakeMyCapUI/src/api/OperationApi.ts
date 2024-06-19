import { ApiHelper } from './ApiHelper';
import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { ServiceStatusDto } from './dto/ServiceStatusDto';

export class OperationApi {

    public static async getEmailServiceStatus(authenticatedUser: AuthenticatedUser): Promise<ServiceStatusDto[] | null> {
        return OperationApi.getServiceStatus("Email", authenticatedUser);
    }

    public static async getFulfillmentServiceStatus(authenticatedUser: AuthenticatedUser): Promise<ServiceStatusDto[] | null> {
        return OperationApi.getServiceStatus("Fulfillment", authenticatedUser);
    }

    public static async getInventoryServiceStatus(authenticatedUser: AuthenticatedUser): Promise<ServiceStatusDto[] | null> {
        return OperationApi.getServiceStatus("Inventory", authenticatedUser);
    }

    public static async getOrderServiceStatus(authenticatedUser: AuthenticatedUser): Promise<ServiceStatusDto[] | null> {
        return OperationApi.getServiceStatus("Order", authenticatedUser);
    }

    private static async getServiceStatus(key: string, authenticatedUser: AuthenticatedUser): Promise<ServiceStatusDto[] | null> {

        const url = ApiHelper.CreateApiUrl('Operation','get_service_status') + '?' +
            new URLSearchParams({
                service: key
            }).toString();

        return fetch(url, {
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
                        return OperationApi.decodeStatusResponse(response);
                    }
                }
                console.log('Get services for ' + key + ' request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting services for ' + key);
                console.log(err);
                return null;
            });
    }

    private static decodeStatusResponse(json: any): ServiceStatusDto[] {
        const response: ServiceStatusDto[] = [];

        const values = Object(json)["values"];
        for (let key in values) {
            const value = values[key];
            const serviceName = value["serviceName"];
            const startDateTime = value["startTime"];
            const endDateTime = value["endTime"];
            const failed = value["failed"]

            response.push(new ServiceStatusDto(serviceName, startDateTime, endDateTime, failed != 'False'));
        }
        return response;
    }
}
