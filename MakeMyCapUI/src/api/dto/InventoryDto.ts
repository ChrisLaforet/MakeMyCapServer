export class InventoryDto {

    public readonly sku: string;
    public readonly description: string;
    public onHand: number;
    public readonly lastUsage: number;
    public readonly distributorCode: string;
    public readonly distributorName: string;

    constructor(sku: string, description: string, onHand: number, lastUsage: number, distributorCode: string, distributorName: string) {
        this.sku = sku;
        this.description = description;
        this.onHand = onHand;
        this.lastUsage = lastUsage;
        this.distributorCode = distributorCode;
        this.distributorName = distributorName;
    }
}
