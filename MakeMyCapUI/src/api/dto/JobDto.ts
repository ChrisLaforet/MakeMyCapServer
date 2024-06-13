export class JobDto {
    public readonly id: number;
    public readonly customerName: string;
    public readonly userCode: string;
    public readonly userName: string;
    public readonly startSerial: number;
    public readonly submitted: string;
    public readonly isCompleted: boolean;
    public readonly isFailed: boolean;
    public readonly failureReason: string | null;
    public readonly caps: CapDto[] = [];

    constructor(id: number, customerName: string, userCode: string, userName: string,
                startSerial: number, isCompleted: boolean, isFailed: boolean, submitted: string,
                failureReason: string | null = null) {
        this.id = id;
        this.customerName = customerName;
        this.userCode = userCode;
        this.userName = userName;
        this.startSerial = startSerial;
        this.submitted = submitted;
        this.isCompleted = isCompleted;
        this.isFailed = isFailed;
        this.failureReason = failureReason;
    }
}

export class CapDto {
    public readonly id: number;
    public readonly name: string;
    public readonly styles: StyleDto[] = [];
    public readonly artworks: ArtworkDto[] = [];

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }
}

export class StyleDto {
    public readonly id: number;
    public readonly name: string;

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }
}

export class ArtworkDto {
    public readonly filename: string;
    public readonly stitch_count: number | null;
    public readonly placements: PlacementDto[] = [];
    public readonly threads: ThreadDto[] = [];

    constructor(filename: string, stitch_count: number | null = null) {
        this.filename = filename;
        this.stitch_count = stitch_count;
    }
}

export class PlacementDto {
    public readonly code: string;
    public readonly artType: string;

    constructor(code: string, artType: string) {
        this.code = code;
        this.artType = artType;
    }
}

export class ThreadDto {
    public readonly code: string;
    public readonly name: string;
    public readonly rgb: string;

    constructor(code: string, name: string, rgb: string) {
        this.code = code;
        this.name = name;
        this.rgb = rgb;
    }
}
