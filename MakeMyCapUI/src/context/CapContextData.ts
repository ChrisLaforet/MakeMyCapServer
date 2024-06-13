import { CapLookup } from '../lookup/CapLookup';
import { Thread } from '../model/Thread';

export class CapContextData {

    private capLookup: CapLookup | null = null;
    private threads: Thread[] | null = null;

    public getCapLookup(): CapLookup | null {
        return this.capLookup;
    }

    public setCapLookup(capLookup: CapLookup | null) {
        this.capLookup = capLookup;
    }

    public getThreadLookup(): Thread[] | null {
        return this.threads;
    }

    public setThreadLookup(threads: Thread[] | null) {
        this.threads = threads;
    }
}
