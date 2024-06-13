export class StyleRecord {

    public id: number;
    public cap_id: number;
    public style_name: string;
    public artboard_name: string;

    constructor(id: number, cap_id: number, style_name: string, artboard_name: string) {
        this.id = id;
        this.cap_id = cap_id;
        this.style_name = style_name;
        this.artboard_name = artboard_name;
    }
}
