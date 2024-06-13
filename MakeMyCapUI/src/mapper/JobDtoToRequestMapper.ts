import { JobRequest } from '../model/JobRequest';
import { CapDto, JobDto } from '../api/dto/JobDto';
import { User } from '../security/auth/User';
import { JobCap } from '../model/JobCap';
import { Cap } from '../model/Cap';
import { Style } from '../model/Style';
import { JobStyle } from '../model/JobStyle';
import { JobImage } from '../model/JobImage';
import { JobThread } from '../model/JobThread';
import { JobPosition } from '../model/JobPosition';

export class JobDtoToRequestMapper {

    public static mapJobDtoToJobRequest(source: JobDto, caps: Cap[], user: User): JobRequest {
        const response = new JobRequest(source.customerName, source.startSerial, user);
        let index = 1;
        source.caps.forEach(capDto => {
            const cap = caps.find(cap => cap.id == capDto.id.toString());
            if (cap != null) {
                response.caps.push(JobDtoToRequestMapper.mapCapDtoToJobCap(capDto, cap, index++));
            }
        });

        return response;
    }

    public static splitOnAnyPathDelimiter(pathAndFile: string): string[] {
        const response: string[] = [];
        let offset = 0;
        while (true) {
            const forwardSlash = pathAndFile.indexOf('/', offset);
            const backSlash = pathAndFile.indexOf('\\', offset);
            if (forwardSlash == -1 && backSlash == -1) {
                const last = pathAndFile.substring(offset);
                if (last.length > 0) {
                    response.push(last);
                }
                break;
            } else if (forwardSlash == -1) {
                response.push(pathAndFile.substring(offset, backSlash));
                offset = backSlash + 1;
            } else if (backSlash == -1) {
                response.push(pathAndFile.substring(offset, forwardSlash));
                offset = forwardSlash + 1;
            } else {
                if (forwardSlash < backSlash) {
                    response.push(pathAndFile.substring(offset, forwardSlash));
                    offset = forwardSlash + 1;
                } else {
                    response.push(pathAndFile.substring(offset, backSlash));
                    offset = backSlash + 1;
                }
            }
        }

        return response;
    }


    public static splitPathAndFile(pathAndFile: string) : [pathname: string, filename: string] {
        let pathname = '';
        let filename = '';
        const parts = JobDtoToRequestMapper.splitOnAnyPathDelimiter(pathAndFile);
        if (parts.length > 0) {
            filename = parts[parts.length - 1];
            if (parts.length > 1) {
                pathname = parts.slice(0, parts.length - 1).join('/');
            }
        } else {
            filename = pathAndFile;
        }
        return [pathname, filename];
    }

    public static mapCapDtoToJobCap(source: CapDto, cap: Cap, index: number): JobCap {
        const response = new JobCap(index, cap);
        source.styles.forEach(style => {
            response.styles.push(new JobStyle(new Style(style.id.toString(), style.name)));
        });
        source.artworks.forEach(artwork => {
            const nameParts = JobDtoToRequestMapper.splitPathAndFile(artwork.filename);
            const image = new JobImage(nameParts[0], nameParts[1], '', artwork.stitch_count);
            artwork.threads.forEach(thread => {
                image.threads.push(new JobThread(thread.code));
            });
            artwork.placements.forEach(placement => {
               image.positions.push(new JobPosition(placement.code));
            });
            response.images.push(image);
        });
        return response;
    }
}
