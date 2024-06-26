export class SkuDto {

    public readonly sku: string;
    public readonly distributorCode: string;
    public readonly distributorSku: string;
    public readonly brand: string | null;
    public readonly styleCode: string | null;
    public readonly partId: string | null;
    public readonly color: string | null;
    public readonly colorCode: string | null;
    public readonly sizeCode: string | null;

    constructor(sku: string, distributorCode: string, distributorSku: string, brand: string | null,
                styleCode: string | null, partId: string | null, color: string | null, colorCode: string | null,
                sizeCode: string | null) {
        this.sku = sku;
        this.distributorCode = distributorCode;
        this.distributorSku = distributorSku;
        this.brand = brand;
        this.styleCode = styleCode;
        this.partId = partId;
        this.color = color;
        this.colorCode = colorCode;
        this.sizeCode = sizeCode
        }
    }
