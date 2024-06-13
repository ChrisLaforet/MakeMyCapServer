import { JobThread } from './JobThread';
import { JobPosition } from './JobPosition';

export class JobImage {

    public readonly path: string;
    public readonly filename: string;
    public readonly decorationType: string;
    public readonly stitchCount: number | null;

    public readonly threads: JobThread[] = [];
    public readonly positions: JobPosition[] = [];

    constructor(path: string, filename: string, decorationType: string, stitchCount: number | null) {
        this.path = path;
        this.filename = filename;
        this.decorationType = decorationType;
        this.stitchCount = stitchCount;
    }

    public clone(): JobImage {
        const newJobImage = new JobImage(this.path, this.filename, this.decorationType, this.stitchCount);
        newJobImage.threads.push(...this.threads);
        newJobImage.positions.push(...this.positions);
        return newJobImage;
    }
}
