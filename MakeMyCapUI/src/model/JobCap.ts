import { JobStyle } from './JobStyle';
import { JobImage } from './JobImage';
import { Cap } from './Cap';

export class JobCap {

    public capModelId: number;
    public readonly cap: Cap;
    public readonly styles: JobStyle[] = [];
    public readonly images: JobImage[] = [];

    constructor(capModelId: number, cap: Cap) {
        this.capModelId = capModelId;
        this.cap = cap;
    }

    public cloneFor(capId: string, capName: string): JobCap {
        const newJobCap = new JobCap(0, this.cap.cloneFor(capId, capName));
        newJobCap.styles.push(...this.styles);
        for (const image of this.images) {
            newJobCap.images.push(image.clone());
        }
        return newJobCap;
    }
}
