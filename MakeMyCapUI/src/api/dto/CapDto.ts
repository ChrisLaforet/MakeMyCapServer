export class CapDto {
    public readonly id: number | null;
    public readonly cap_code: string;
    public readonly filename: string;
    public readonly has_front_right: boolean;
    public readonly has_front_center: boolean;
    public readonly has_front_left: boolean;
    public readonly has_bill_right: boolean;
    public readonly has_bill_center: boolean;
    public readonly has_bill_left: boolean;
    public readonly has_side_left: boolean;
    public readonly has_side_right: boolean;
    public readonly has_back: boolean;
    public readonly has_strap: boolean;
    public readonly images_on_right: boolean;

    constructor(cap_code: string, filename: string, has_front_right: boolean,
                has_front_center: boolean, has_front_left: boolean, has_bill_right: boolean, has_bill_center: boolean,
                has_bill_left: boolean, has_side_left: boolean, has_side_right: boolean, has_back: boolean,
                has_strap: boolean, images_on_right: boolean, id: number | null = null) {
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
