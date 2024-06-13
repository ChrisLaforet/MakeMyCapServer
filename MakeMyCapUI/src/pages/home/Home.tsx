import React, { useEffect, useState } from 'react';
import { useSharedContext } from '../../context/SharedContext';
import { StatsDto } from '../../api/dto/StatsDto';
import { JobApi } from '../../api/JobApi';
import { JobStatDto } from '../../api/dto/JobStatDto';

export default function Home() {

    const sharedContextData = useSharedContext();

    const [loggedIn, setLoggedIn] = useState<boolean>(false)
    const [stats, setStats] = useState<StatsDto | null>(null)
    const [jobs, setJobs] = useState<JobStatDto[]>([]);

    const loadStats = async () => {
        const response = await JobApi.loadStats();
        if (response) {
            setStats(response);
            const user = sharedContextData.getAuthenticatedUser()!;
            if (user) {
                const myJobs: JobStatDto[] = [];
                setJobs(response.jobs7Days.filter(job => job.userName == user.name));
            }
        }

    }

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            setLoggedIn(true)
        }

        loadStats().catch(console.error);
    }, []);


    return(
        <div>
            <h1 id="ScreenTitle" className="display-page-title">
                <span className="ca-red">Generator</span> <span className="ca-blue">Stats</span>
            </h1>

            <div className="row">
                &nbsp;
            </div>
            <div className="row">
                <div>
                    <h4>Recent Usage</h4>
                    <div className="row">
                        <div className="col-3">Total artwork generated last 24 hours:</div>
                        <div className="col-1">{stats ? stats.total24Hours : 'Pending'}</div>
                    </div>
                    <div className="row">
                        <div className="col-3">Total artwork generated last 7 days:</div>
                        <div className="col-1">{stats ? stats.total7Days : 'Pending'}</div>
                    </div>
                    <div className="row">
                        <div className="col-3">Total artwork generated last 30 days:</div>
                        <div className="col-1">{stats ? stats.total30Days : 'Pending'}</div>
                    </div>
                </div>
                <div className="row">
                    &nbsp;
                </div>
                {
                    loggedIn && jobs && jobs.length > 0 &&
                    <div>
                        <div className="row">
                            &nbsp;
                        </div>
                        <h4>My Jobs for Last 7 Days</h4>
                        <div className="row">
                            <div className="col-1 fw-medium">Job Id</div>
                            <div className="col-2 fw-medium">Customer name</div>
                            <div className="col-2 fw-medium">Submitted (UTC)</div>
                        </div>
                        {(jobs).map(function (job, key) {
                                return (
                                    <div key={job.id} className="row">
                                        <div className="col-1">{job.id}</div>
                                        <div className="col-2">{job.customerName}</div>
                                        <div className="col-2">{job.submitted}</div>
                                    </div>
                                );
                            }
                        )}
                    </div>
                }
            </div>


        </div>

    );
}
