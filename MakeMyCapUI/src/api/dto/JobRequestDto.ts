import { JobRequest } from '../../model/JobRequest';
import { JobCap } from '../../model/JobCap';
import { JobStyle } from '../../model/JobStyle';
import { JobThread } from '../../model/JobThread';
import { JobImage } from '../../model/JobImage';


export class JobRequestDto {

    public readonly requester_code: string;
    public readonly requester_user_name: string;

    public readonly customer_name: string;
    public readonly start_serial: number | null;

    public readonly caps: JobCapDto[] = [];


    constructor(request: JobRequest) {
        this.requester_code = request.requester.code;
        this.requester_user_name = request.requester.userName;
        this.customer_name = request.customer;
        this.start_serial = request.serial;

        request.caps.forEach((cap) => {
           this.caps.push(new JobCapDto(cap));
        });
    }
}

export class JobCapDto {

    public readonly id: number;
    public readonly name: string;

    public readonly styles: JobStyleDto[] = [];

    public readonly images: JobImageDto[] = [];

    constructor(cap: JobCap) {
        this.id = parseInt(cap.cap.id);
        this.name = cap.cap.name;

        cap.styles.forEach((style) => {
            this.styles.push(new JobStyleDto(style));
        });

        cap.images.forEach((image) => {
           this.images.push(new JobImageDto(image));
        });
    }
}

export class JobStyleDto {

    public readonly id: number;
    public readonly name: string;

    constructor(style: JobStyle) {
        this.id = parseInt(style.style.id);
        this.name = style.style.name;
    }
}

export class JobImageDto {

    public readonly filename: string;
    public readonly path: string;

    public readonly type: string;

    public readonly position_codes: string[] = [];

    public readonly threads: JobThreadDto[] = [];

    public readonly stitch_count: number | null;

    constructor(image: JobImage) {
        this.filename = image.filename;
        this.path = image.path.trim();
        this.type = image.decorationType;
        this.stitch_count = image.stitchCount;

        image.positions.forEach((position) => {
            this.position_codes.push(position.code);
        });

        image.threads.forEach((thread) => {
            this.threads.push(new JobThreadDto(thread));
        });
    }
}


export class JobThreadDto {

    public readonly code: string;

    constructor(thread: JobThread) {
        this.code = thread.color;
    }
}
