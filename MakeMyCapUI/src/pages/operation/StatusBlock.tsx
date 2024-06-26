import "./Operation.css";
import { ServiceStatusDto } from '../../api/dto/ServiceStatusDto';
import { useEffect } from 'react';

interface StatusBlockProps {
    serviceStatus: ServiceStatusDto[],
    borderColor: string
}

export default function StatusBlock({serviceStatus, borderColor}: StatusBlockProps) {

    useEffect(() => {
        console.log("ServiceStatus", serviceStatus);
    }, [serviceStatus]);

    return (
        <div className={"border rounded col-lg-5 col-12 status-block-container " + borderColor}>
            <table className="table table-striped table-sm status-block-table">
                <thead className="table-dark">
                <tr>
                    <th className="status-block-col0">Service name</th>
                    <th className="status-block-col1">Start time</th>
                    <th className="status-block-col2">End time</th>
                    <th className="status-block-col3">Failed?</th>
                </tr>
                </thead>
                <tbody>
                {
                    (serviceStatus).map(function (status, key) {
                        return (
                            <tr key={key}>
                                <td>{status.serviceName}</td>
                                <td>{status.startDateTime}</td>
                                <td>{status.endDateTime}</td>
                                <td>{status.isFailed ? 'Yes' : ''}</td>
                            </tr>
                        );
                    })
                }
                </tbody>
            </table>

        </div>

    )

}


//     @foreach (var log in @Model.InventoryServiceStatus)
// {
//     <tr>
//         <td>@log.ServiceName</td>
//     <td>@log.StartTime</td>
//     <td>@log.EndTime</td>
//     <td>@log.Failed</td>
//     </tr>
// }
// </tbody>
// </table>
// </div>
