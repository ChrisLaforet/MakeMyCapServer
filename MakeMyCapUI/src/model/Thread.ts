export class Thread {

    public readonly code: number;
    public readonly colorName: string;
    public readonly rgb: string;

    constructor(code: number, colorName: string, rgb: string) {
        this.code = code;
        this.colorName = colorName;
        this.rgb = rgb;
    }
}
