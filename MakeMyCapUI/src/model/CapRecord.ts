import { StyleRecord } from './StyleRecord';

export class CapRecord {
    public id: number;
    public cap_code: string;
    public filename: string;
    public has_front_right: boolean = false;
    public has_front_center: boolean = false;
    public has_front_left: boolean = false;
    public has_bill_right: boolean = false;
    public has_bill_center: boolean = false;
    public has_bill_left: boolean = false;
    public has_side_left: boolean = false;
    public has_side_right: boolean = false;
    public has_back: boolean = false;
    public has_strap: boolean = false;
    public images_on_right: boolean = false;
    public styles: StyleRecord[] = [];

    constructor(id: number, cap_code: string, filename: string, has_front_right: boolean,
                has_front_center: boolean, has_front_left: boolean, has_bill_right: boolean, has_bill_center: boolean,
                has_bill_left: boolean, has_side_left: boolean, has_side_right: boolean, has_back: boolean,
                has_strap: boolean, images_on_right: boolean) {
        this.id = id;
        this.cap_code = cap_code;
        this.filename = filename;
        this.has_front_right = has_front_right;
        this.has_front_center = has_front_center;
        this.has_front_left = has_front_left;
        this.has_bill_right = has_bill_right;
        this.has_bill_center = has_bill_center;
        this.has_bill_left = has_bill_left;
        this.has_side_left = has_side_left;
        this.has_side_right = has_side_right;
        this.has_back = has_back;
        this.has_strap = has_strap;
        this.images_on_right = images_on_right;
    }
}
