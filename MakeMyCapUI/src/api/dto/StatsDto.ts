import { JobStatDto } from './JobStatDto';

export class StatsDto {
    total24Hours: number;
    total7Days: number;
    total30Days: number;

    jobs7Days: JobStatDto[] = [];

    constructor(total24Hours: number, total7Days: number, total30Days: number) {
        this.total24Hours = total24Hours;
        this.total7Days = total7Days;
        this.total30Days = total30Days;
    }
}
