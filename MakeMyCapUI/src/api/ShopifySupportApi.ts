import { ApiHelper } from './ApiHelper';
import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { DistributorDto } from './dto/DistributorDto';
import { DistributorSkusDto } from './dto/DistributorSkusDto';
import { SkuDto } from './dto/SkuDto';
import { UserDto } from './dto/UserDto';


export class ShopifySupportApi {

    public static async getDistributors(authenticatedUser: AuthenticatedUser): Promise<DistributorDto[] | null> {

        return fetch(ApiHelper.CreateApiUrl('ShopifySupport', 'distributors'), {
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
                        return ShopifySupportApi.decodeDistributorsResponse(response);
                    }
                }
                console.log('Get distributors request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting distributors');
                console.log(err);
                return null;
            });
    }

    private static decodeDistributorsResponse(json: any): DistributorDto[] {
        const response: DistributorDto[] = [];
        const distributors = Object(json)["distributors"];
        for (let key in distributors) {
            const distributor = distributors[key];
            const code = distributor["distributorCode"];
            const name = distributor["distributorName"];
            response.push(new DistributorDto(code, name));
        }
        return response;
    }

    public static async getSkusForDistributor(distributorCode: string, authenticatedUser: AuthenticatedUser): Promise<DistributorSkusDto | null> {

        return fetch(ApiHelper.CreateApiUrl('ShopifySupport', 'skus') + `?id=${distributorCode}`, {
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
                        return ShopifySupportApi.decodeSkusResponse(response);
                    }
                }
                console.log('Get skus request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting skus');
                console.log(err);
                return null;
            });
    }

    private static decodeSkusResponse(json: any): DistributorSkusDto {
        const code = Object(json)["distributorCode"];
        const name = Object(json)["distributorName"];

        const distributorSkus: SkuDto[] = [];
        const skus = Object(json)["skus"];
        for (let key in skus) {
            const item = skus[key];
            const sku = item["sku"];
            const distributorCode = item["distributorCode"];
            const distributorSku = item["distributorSku"];
            const brand = item["brand"];
            const styleCode = item["styleCode"];
            const partId = item["partId"];
            const color = item["color"];
            const colorCode = item["colorCode"];
            const sizeCode = item["sizeCode"];
            distributorSkus.push(new SkuDto(sku, distributorCode, distributorSku, brand, styleCode, partId, color, colorCode, sizeCode));
        }
        return new DistributorSkusDto(new DistributorDto(code, name), distributorSkus);
    }

    public static async createSku(skuDto: SkuDto, authenticatedUser: AuthenticatedUser): Promise<boolean> {
        return fetch(ApiHelper.CreateApiUrl('ShopifySupport', 'create-sku'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify(skuDto)
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`New sku record created for ${skuDto.sku}`)
                    return true;
                }
                console.log(`New sku record not created for ${skuDto.sku}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception creating sku record for ${skuDto.sku}`);
                console.log(err);
                return false;
            });
    }

    public static async updateSku(originalSku: string, skuDto: SkuDto, authenticatedUser: AuthenticatedUser): Promise<boolean> {
        return fetch(ApiHelper.CreateApiUrl('ShopifySupport', 'update-sku'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify({
                originalSku: originalSku,
                newSku: skuDto.sku,
                distributorSku: skuDto.distributorSku,
                brand: skuDto.brand,
                styleCode: skuDto.styleCode,
                partId: skuDto.partId,
                color: skuDto.color,
                colorCode: skuDto.colorCode,
                sizeCode: skuDto.sizeCode
            })
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`Sku record updated for ${originalSku}`)
                    return true;
                }
                console.log(`Sku record not updated for ${originalSku}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception updating sku record for ${originalSku}`);
                console.log(err);
                return false;
            });
    }
}
