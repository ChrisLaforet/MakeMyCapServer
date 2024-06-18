import "./Operation.css";

export default function StatusBlock() {

    return (
        <div className="border border-success rounded col-lg-5 col-12 status-block-container">
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
