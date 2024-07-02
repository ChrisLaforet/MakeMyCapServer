import { ApiHelper } from './ApiHelper';
import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { InventoryDto } from './dto/InventoryDto';
import { AvailableSkuDto } from './dto/AvailableSkuDto';
import { SkuDto } from './dto/SkuDto';

export class InventoryApi {

    public static async getInHouseInventory(authenticatedUser: AuthenticatedUser): Promise<InventoryDto[] | null> {

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
                console.log('Get in-house inventory request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting in-house inventory');
                console.log(err);
                return null;
            });
    }

    private static decodeInventoryResponse(json: any): InventoryDto[] {
        const response: InventoryDto[] = [];

        const items = Object(json)["items"];
        for (let key in items) {
            const item = items[key];
            const sku = item["sku"];
            const description = item["description"];
            const onHand = item["onHand"];
            const lastUsage = item["lastUsage"]
            const distributorCode = item["distributorCode"];
            const distributorName = item["distributorName"];

            response.push(new InventoryDto(sku, description, onHand, lastUsage, distributorCode, distributorName));
        }
        return response;
    }

    public static async getAvailableSkus(authenticatedUser: AuthenticatedUser): Promise<AvailableSkuDto[] | null> {

        return fetch(ApiHelper.CreateApiUrl('Inventory','available-skus'), {
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
                        return InventoryApi.decodeAvailableSkusResponse(response);
                    }
                }
                console.log('Get available skus request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting available skus');
                console.log(err);
                return null;
            });
    }

    private static decodeAvailableSkusResponse(json: any): AvailableSkuDto[] {
        const response: AvailableSkuDto[] = [];

        const items = Object(json)["skus"];
        for (let key in items) {
            const item = items[key];
            const sku = item["sku"];
            const description = item["description"];
            const distributorCode = item["distributorCode"];
            const distributorName = item["distributorName"];

            response.push(new AvailableSkuDto(sku, description, distributorCode, distributorName));
        }
        return response;
    }

    public static async createInHouseInventory(sku: string, onHand: number, authenticatedUser: AuthenticatedUser): Promise<boolean> {

        return fetch(ApiHelper.CreateApiUrl('Inventory', 'create-in-house-inventory'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify({
                sku: sku,
                onHand: onHand,
            })
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`New in-house inventory record created for ${sku}`)
                    return true;
                }
                console.log(`New in-house inventory record not created for ${sku}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception creating in-house inventory record for ${sku}`);
                console.log(err);
                return false;
            });
    }

    public static async updateInHouseInventory(sku: string, onHand: number, authenticatedUser: AuthenticatedUser): Promise<boolean> {

        return fetch(ApiHelper.CreateApiUrl('Inventory', 'update-in-house-inventory'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify({
                sku: sku,
                onHand: onHand,
            })
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`In-house inventory record updated for ${sku}`)
                    return true;
                }
                console.log(`In-house inventory record not updated for ${sku}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception updating in-house inventory record for ${sku}`);
                console.log(err);
                return false;
            });
    }
}
