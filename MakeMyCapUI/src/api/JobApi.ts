import { AuthenticatedUser } from '../security/auth/AuthenticatedUser';
import { ApiHelper } from './ApiHelper';
import { JobRequest } from '../model/JobRequest';
import { DtoMapper } from './DtoMapper';
import { ArtworkDto, CapDto, JobDto, PlacementDto, StyleDto, ThreadDto } from './dto/JobDto';
import { StatsDto } from './dto/StatsDto';
import { JobStatDto } from './dto/JobStatDto';

export class JobApi {

    public static async createNewJob(jobRequest: JobRequest, authenticatedUser: AuthenticatedUser): Promise<boolean> {

        return fetch(ApiHelper.CreateApiUrl('create_job'), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
            body: JSON.stringify(DtoMapper.mapJobRequestToDto(jobRequest))
        })
            .then(async data => {
                if (data.ok) {
                    console.log(`New job saved for ${authenticatedUser.userName}`)
                    return true;
                }
                console.log(`New job not saved for ${authenticatedUser.userName}`)
                return false;
            })
            .catch(err => {
                console.log(`Caught exception saving job for ${authenticatedUser.userName}`);
                console.log(err);
                return false;
            });
    }

    public static async loadJobs(authenticatedUser: AuthenticatedUser): Promise<JobDto[] | null> {

        return fetch(ApiHelper.CreateApiUrl('get_jobs'), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey(),
                'Authorization': ApiHelper.GetBearer(authenticatedUser)
            },
        })
            .then(async data => {
                if (data.ok) {
                    const response = await data.json();
                    if (response) {
                        return JobApi.decodeMyJobsResponse(response);
                    }
                }
                console.log('Get jobs request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting jobs');
                console.log(err);
                return null;
            });
    }

    private static decodeMyJobsResponse(json: any): JobDto[] {
        const response: JobDto[] = [];
        const jobs: object[] = Object(json)["jobs"];
        for (let key in jobs) {
            const job = jobs[key];

            const id = Object(job)["id"];
            const userCode = Object(job)["user_code"];
            const userName = Object(job)["user_name"];
            const customerName = Object(job)["customer_name"];
            const startSerial = Object(job)["start_serial"];
            const submitted = Object(job)["submitted"];
            const isCompleted = Object(job)["completed"];
            const isFailed = Object(job)["failed"];
            const failureReason = Object(job)["failure_reason"];
            const record = new JobDto(id, customerName, userCode, userName, startSerial, isCompleted, isFailed, submitted, failureReason);
            JobApi.extractCapsFrom(Object(job)["caps"], record);
            response.push(record);
        }
        return response;
    }

    private static extractCapsFrom(caps: any[], record: JobDto) {
        for (let key in caps) {
            const cap = caps[key];

            const id = Object(cap)["id"];
            const name = Object(cap)["name"];
            const capDto = new CapDto(id, name);
            JobApi.extractStylesFrom(Object(cap)["styles"], capDto);
            JobApi.extractArtworksFrom(Object(cap)["artworks"], capDto);
            record.caps.push(capDto);
        }
    }

    private static extractStylesFrom(styles: any[], record: CapDto) {
        for (let key in styles) {
            const style = styles[key];

            const id = Object(style)["id"];
            const name = Object(style)["name"];
            record.styles.push(new StyleDto(id, name));
        }
    }

    private static extractArtworksFrom(artworks: any[], record: CapDto) {
        for (let key in artworks) {
            const artwork = artworks[key];

            const filename = Object(artwork)["filename"];
            const stitchCount = Object(artwork)["stitch_count"];
            const artworkDto = new ArtworkDto(filename, stitchCount);
            JobApi.extractPlacementsFrom(Object(artwork)["placements"], artworkDto);
            JobApi.extractThreadsFrom(Object(artwork)["threads"], artworkDto);
            record.artworks.push(artworkDto);
        }
    }

    private static extractPlacementsFrom(placements: any[], record: ArtworkDto) {
        for (let key in placements) {
            const placement = placements[key];

            const code = Object(placement)["code"];
            const artType = Object(placement)["type"];
            record.placements.push(new PlacementDto(code, artType));
        }
    }

    private static extractThreadsFrom(threads: any[], record: ArtworkDto) {
        for (let key in threads) {
            const thread = threads[key];

            const code = Object(thread)["code"];
            const name = Object(thread)["name"];
            const rgb = Object(thread)["rgb"];
            record.threads.push(new ThreadDto(code, name, rgb));
        }
    }

    public static async loadStats(): Promise<StatsDto | null> {

        return fetch(ApiHelper.CreateApiUrl('get_stats'), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'X-Api-Key': ApiHelper.GetApiKey()
            },
        })
            .then(async data => {
                if (data.ok) {
                    const response = await data.json();
                    if (response) {
                        return JobApi.decodeStatsResponse(response);
                    }
                }
                console.log('Get stats request failed')
                return null;
            })
            .catch(err => {
                console.log('Caught exception getting stats');
                console.log(err);
                return null;
            });
    }

    private static decodeStatsResponse(json: any): StatsDto | null {
        const stats = Object(json)["stats"];
        if (!stats) {
            return null;
        }
        const total24Hours = Object(stats)["total_24_hours"];
        const total7Days = Object(stats)["total_7_days"];
        const total30Days = Object(stats)["total_30_days"]
        const response: StatsDto = new StatsDto(total24Hours, total7Days, total30Days);

        const jobs: object[] = Object(stats)["jobs"];
        for (let key in jobs) {
            const job = jobs[key];

            const id = Object(job)["id"];
            const userName = Object(job)["user_name"];
            const customerName = Object(job)["customer_name"];
            const submitted = Object(job)["submitted"];
            response.jobs7Days.push(new JobStatDto(id, customerName, userName, submitted))
        }
        return response;
    }

}
