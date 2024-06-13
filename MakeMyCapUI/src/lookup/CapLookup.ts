import { Cap } from '../model/Cap';
import { Style } from '../model/Style';
import { Position } from '../model/Position';

export class CapLookup {

    private readonly caps: Cap[];

    constructor(caps: Cap[]) {
        this.caps = caps;
    }

    public getCapsList(): Cap[] {
        return this.caps;
    }

    public getCap(id: string): Cap | undefined {
        const match = this.caps.find(cap => cap.id == id);
        console.log(match);
        return match;
    }

    public getStylesFor(capId: string): Style[] {
        const match = this.caps.find(cap => cap.id == capId);
        return match ? match.styles : [];
    }

    public getPositionsFor(capId: string): Position[] {
        const match = this.caps.find(cap => cap.id == capId);
        return match ? match.positions : [];
    }

}
