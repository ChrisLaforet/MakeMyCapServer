import { JobRequest } from '../model/JobRequest';
import { JobRequestDto } from './dto/JobRequestDto';

export class DtoMapper {

    public static mapJobRequestToDto(jobRequest: JobRequest): JobRequestDto {
        return new JobRequestDto(jobRequest);
    }
}
