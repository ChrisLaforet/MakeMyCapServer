export class ServiceStatusDto {

    public readonly serviceName: string;
    public readonly startDateTime: string;
    public readonly endDateTime: string | null;
    public readonly isFailed: boolean;

    constructor(serviceName: string, startDateTime: string, endDateTime: string | null, isFailed: boolean) {
        this.serviceName = serviceName;
        this.startDateTime = startDateTime;
        this.endDateTime = endDateTime;
        this.isFailed = isFailed;
    }
}
