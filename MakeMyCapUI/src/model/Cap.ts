import { Style } from './Style';
import { Position } from './Position';

export class Cap {

    public readonly id: string;
    public readonly name: string;
    public readonly styles: Style[] = [];
    public readonly positions: Position[] = [];

    constructor(id: string, name: string) {
        this.id = id;
        this.name = name;
    }

    public addStyle(style: Style) {
        this.styles.push(style);
    }

    public addPosition(position: Position) {
        this.positions.push(position);
    }

    public cloneFor(capId: string, capName: string): Cap {
        const newCap = new Cap(capId, capName);
        newCap.positions.push(...this.positions);
        return newCap;
    }
}
