import { ApiHelper } from './ApiHelper';
import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { InventoryDto } from './dto/InventoryDto';

export class InventoryApi {

    private static async getInHouseInventory(authenticatedUser: AuthenticatedUser): Promise<InventoryDto[] | null> {

        return fetch(ApiHelper.CreateApiUrl('Inventory','in-house-inventory'), {
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
                        return InventoryApi.decodeInventoryResponse(response);
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

    private static decodeInventoryResponse(json: any): InventoryDto[] {
        const response: InventoryDto[] = [];

        const values = Object(json)["values"];
        for (let key in values) {
            const value = values[key];
            const sku = value["sku"];
            const description = value["description"];
            const onHand = value["onHand"];
            const lastUsage = value["lastUsage"]
            const distributorCode = value["distributorCode"];
            const distributorName = value["distributorName"];

            response.push(new InventoryDto(sku, description, onHand, lastUsage, distributorCode, distributorName));
        }
        return response;
    }

}
