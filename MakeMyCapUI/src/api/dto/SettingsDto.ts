export class SettingsDto {

    public readonly inventoryCheckHours: number;
    public readonly fulfillmentCheckHours: number;
    public readonly nextPoSequence: number;

    constructor(inventoryCheckHours: number, fulfillmentCheckHours: number, nextPoSequence: number) {
        this.inventoryCheckHours = inventoryCheckHours;
        this.fulfillmentCheckHours = fulfillmentCheckHours;
        this.nextPoSequence = nextPoSequence;
    }
}
