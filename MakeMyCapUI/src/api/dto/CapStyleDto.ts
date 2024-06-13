export class CapStyleDto {
    public readonly id: number | null;
    public readonly cap_id: number;
    public readonly style_name: string;
    public readonly artboard_name: string;

    constructor(cap_id: number, style_name: string, artboard_name: string, id: number | null = null) {
        this.id = id;
        this.cap_id = cap_id;
        this.style_name = style_name;
        this.artboard_name = artboard_name;
    }
}
