import { DistributorDto } from './DistributorDto';
import { SkuDto } from './SkuDto';

export class DistributorSkusDto {

    public readonly distributor: DistributorDto;
    public readonly skus: SkuDto[];

    constructor(distributor: DistributorDto, skus: SkuDto[]) {
        this.distributor = distributor;
        this.skus = skus;
    }
}
