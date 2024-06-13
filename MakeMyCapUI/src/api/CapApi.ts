import { ApiHelper } from './ApiHelper';
import { CapLookup } from '../lookup/CapLookup';
import { Cap } from '../model/Cap';
import { Position } from '../model/Position';
import { Style } from '../model/Style';
import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { CapRecord } from '../model/CapRecord';
import { StyleRecord } from '../model/StyleRecord';
import { CapDto } from './dto/CapDto';

export class CapApi {

    public static async loadCapLookup(authenticatedUser: AuthenticatedUser): Promise<CapLookup | null> {

        return fetch(ApiHelper.CreateApiUrl('get_caps'), {
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
                        return new CapLookup(CapApi.decodeResponse(response));
                    }
                }
                console.log('Get caps request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting caps');
                console.log(err);
                return null;
            });
    }

    private static decodeResponse(json: any): Cap[] {
        const response: Cap[] = [];
        const caps: object[] = Object(json)["caps"];
        for (let key in caps) {
            const cap = caps[key];

            const id = Object(cap)["id"];
            const code = Object(cap)["code"];
            const record = new Cap(id, code);
            CapApi.extractPositionsFrom(Object(cap)["positions"], record);
            CapApi.extractStylesFrom(Object(cap)["styles"], record);
            response.push(record);
        }
        return response;
    }

    private static extractPositionsFrom(positions: any[], record: Cap) {
        for (let key in positions) {
            const position = positions[key];
            record.addPosition(new Position(position));
        }
    }

    private static extractStylesFrom(styles: any[], record: Cap) {
        for (let key in styles) {
            const style = styles[key];
            record.addStyle(CapApi.extractStyleFrom(style));
        }
    }

    private static extractStyleFrom(style: any) {
        const id = Object(style)["id"];
        const name = Object(style)["name"];
        return new Style(id, name);
    }

    public static async loadCapRecords(authenticatedUser: AuthenticatedUser): Promise<CapRecord[] | null> {

        return fetch(ApiHelper.CreateApiUrl('get_cap_records'), {
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
                        return CapApi.decodeCapRecordsResponse(response);
                    }
                }
                console.log('Get cap records request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting caps');
                console.log(err);
                return null;
            });
    }

    private static decodeCapRecordsResponse(json: any): CapRecord[] {
        const response: CapRecord[] = [];
        const caps: object[] = Object(json)["cap_records"];
        for (let key in caps) {
            const cap = caps[key];
            const id = Object(cap)["id"];
            const cap_code = Object(cap)["cap_code"];
            const filename = Object(cap)["filename"];
            const has_front_right = Object(cap)["has_front_right"];
            const has_front_center = Object(cap)["has_front_center"];
            const has_front_left = Object(cap)["has_front_left"];
            const has_bill_right = Object(cap)["has_bill_right"];
            const has_bill_center = Object(cap)["has_bill_center"];
            const has_bill_left = Object(cap)["has_bill_left"];
            const has_side_left = Object(cap)["has_side_left"];
            const has_side_right = Object(cap)["has_side_right"];
            const has_back = Object(cap)["has_back"];
            const has_strap = Object(cap)["has_strap"];
            const images_on_right = Object(cap)["images_on_right"];
            const record = new CapRecord(id, cap_code, filename, has_front_right, has_front_center,
                has_front_left, has_bill_right, has_bill_center, has_bill_left, has_side_left, has_side_right,
                has_back, has_strap, images_on_right);

            CapApi.extractStyleRecordsFrom(Object(cap)["styles"], record);
            response.push(record);
        }
        return response;
    }

    private static extractStyleRecordsFrom(styles: any[], record: CapRecord) {
        for (let key in styles) {
            const style = styles[key];

            const id = Object(style)["id"];
            const cap_id = Object(style)["cap_id"];
            const style_name = Object(style)["style_name"];
            const artboard_name = Object(style)["artboard_name"];
            record.styles.push(new StyleRecord(id, cap_id, style_name, artboard_name));
        }
    }

    public static async createNewCap(cap: CapDto, authenticatedUser: AuthenticatedUser): Promise<boolean> {
        let capCopy= {};
        Object.assign(capCopy, cap);
        // @ts-ignore
        delete capCopy["id"];

        return fetch(ApiHelper.CreateApiUrl('create_cap'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify(cap)
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`New cap created for ${cap.cap_code}`)
                    return true;
                }
                console.log(`New cap not created for ${cap.cap_code}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception creating cap for ${cap.cap_code}`);
                console.log(err);
                return false;
            });
    }

    public static async updateCap(cap: CapDto, authenticatedUser: AuthenticatedUser): Promise<boolean> {

        return fetch(ApiHelper.CreateApiUrl('update_cap'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify(cap)
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`Cap record updated for ${cap.cap_code}`)
                    return true;
                }
                console.log(`Cap record not updated for ${cap.cap_code}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception updating cap record for ${cap.cap_code}`);
                console.log(err);
                return false;
            });
    }
}
