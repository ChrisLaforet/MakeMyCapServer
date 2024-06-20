export class NotificationEmailsDto {

    public readonly warningEmail1: string;
    public readonly warningEmail2: string | null = null;
    public readonly warningEmail3: string | null = null;
    public readonly criticalEmail1: string;
    public readonly criticalEmail2: string | null = null;
    public readonly criticalEmail3: string | null = null;

    constructor(warningEmail1: string, warningEmail2: string | null, warningEmail3: string | null,
                criticalEmail1: string, criticalEmail2: string | null, criticalEmail3: string | null) {
        this.warningEmail1 = warningEmail1;
        this.warningEmail2 = warningEmail2;
        this.warningEmail3 = warningEmail3;
        this.criticalEmail1 = criticalEmail1;
        this.criticalEmail2 = criticalEmail2;
        this.criticalEmail3 = criticalEmail3;
    }
}
