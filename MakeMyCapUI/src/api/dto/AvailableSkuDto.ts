export class AvailableSkuDto {

    public readonly sku: string;
    public readonly description: string;
    public readonly distributorCode: string;
    public readonly distributorName: string;

    constructor(sku: string, description: string, distributorCode: string, distributorName: string) {
        this.sku = sku;
        this.description = description;
        this.distributorCode = distributorCode;
        this.distributorName = distributorName;
    }
}
