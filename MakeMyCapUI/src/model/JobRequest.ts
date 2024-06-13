import { JobCap } from './JobCap';
import { User } from '../security/auth/User';

export class JobRequest {

    public readonly customer: string;
    public readonly serial: number;
    public readonly requester: User;

    public readonly caps: JobCap[] = [];

    constructor(customer: string, serial: number, requester: User) {
        this.customer = customer;
        this.serial = serial;
        this.requester = requester;
    }
}
