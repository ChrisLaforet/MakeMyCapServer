export class DistributorDto {

    public readonly code: string;
    public readonly name: string;

    constructor(code: string, name: string) {
        this.code = code;
        this.name = name;
    }
}
