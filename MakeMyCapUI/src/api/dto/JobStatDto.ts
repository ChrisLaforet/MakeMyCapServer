export class JobStatDto {
    public readonly id: number;
    public readonly customerName: string;
    public readonly userName: string;
    public readonly submitted: string;

    constructor(id: number, customerName: string, userName: string, submitted: string) {
        this.id = id;
        this.customerName = customerName;
        this.userName = userName;
        this.submitted = submitted;
    }
}
