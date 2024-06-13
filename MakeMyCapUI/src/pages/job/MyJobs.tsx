import React, { useEffect, useState } from "react";
import { useSharedContext } from '../../context/SharedContext';
import { JobDto } from '../../api/dto/JobDto';
import { JobApi } from '../../api/JobApi';
import { JobDtoToRequestMapper } from '../../mapper/JobDtoToRequestMapper';
import { Alerter } from '../../layout/Alerter';
import { useNavigate } from 'react-router-dom';
import { CapApi } from '../../api/CapApi';
import { CapLookup } from '../../lookup/CapLookup';
import { Cap } from '../../model/Cap';


export default function MyJobs() {

    const sharedContextData = useSharedContext();

    const [myJobs, setMyJobs] = useState<JobDto[] | null>(() => null);
    const [caps, setCaps] = useState<Cap[]>(() => []);

    const navigate = useNavigate();

    const loadMyJobs = async () => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            const lookup = await JobApi.loadJobs(authenticatedUser);
            if (lookup) {
                setMyJobs(lookup);
            } else {
                setMyJobs([]);
            }

        }
    }

    const loadCaps = async () => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            const lookup = await CapApi.loadCapLookup(authenticatedUser);
            if (lookup) {
                setCaps(lookup.getCapsList());
            }
        }
    }

    function cloneJob(job: JobDto) {
        if (confirm(`Are you sure you want to clone job ${job.id} for ${job.customerName}?`)) {
            Alerter.showInfo(`Cloning job ${job.id} for ${job.customerName}`, Alerter.DEFAULT_TIMEOUT);

            const authenticatedUser = sharedContextData.getAuthenticatedUser();
            sharedContextData.setJobRequest(JobDtoToRequestMapper.mapJobDtoToJobRequest(job, caps, authenticatedUser!.extractUser()));
            navigate('/RequestNewJob');
        }
    }

    useEffect(() => {
        loadMyJobs().catch(console.error);
        loadCaps().catch(console.error);
    }, []);

    return (
        <div>
            <h2 className="ca-blue"><span className="ca-red">Listing of</span> <span className="ca-blue">All My Jobs</span></h2>
            {!myJobs && <div className="ca-spinner-box spinner-border text-primary"><span className="sr-only"></span></div>}
            {myJobs &&
                <div className="ca_tabular-data-container">
                    <table className="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th scope="col">Job Id</th>
                                <th scope="col">Customer name</th>
                                <th scope="col"># caps</th>
                                <th scope="col">Start serial</th>
                                <th scope="col">Completed</th>
                                <th scope="col">Failed</th>
                                <th scope="col">Submitted (UTC)</th>
                                <th scope="col">Operations</th>
                            </tr>
                        </thead>
                        <tbody>
                        {(myJobs).map(function(job, key) {
                                return (
                                    <tr>
                                        <th scope="row">{job.id}</th>
                                        <td>{job.customerName}</td>
                                        <td>{job.caps.length}</td>
                                        <td>{job.startSerial}</td>
                                        <td>{job.isCompleted ? "Y" : ""}</td>
                                        <td>{job.isFailed ? "Y" : ""}</td>
                                        <td>{job.submitted}</td>
                                        <td>
                                            <button className="btn btn-sm btn-outline-primary" onClick={() => cloneJob(job)}>Clone as new job</button>
                                        </td>
                                    </tr>
                                );
                            })
                        }
                        </tbody>
                    </table>
                </div>
            }
        </div>
    );
}
