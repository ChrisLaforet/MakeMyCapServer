import "./Operation.css";
import StatusBlock from './StatusBlock';

export default function ShowStatus() {

    return (
        <>
            <h1 className="display-page-title operation-header">Make My Cap Server Status</h1>
            <div className="row status-block-row">
                <StatusBlock />
                <StatusBlock />
            </div>
            <div className="row status-block-row">
                <StatusBlock />
                <StatusBlock />
            </div>
        </>
    );
}


// <div>
//     <h1 className="display-4" style="padding-bottom: 10px">Make My Cap Server Status</h1>
//
//     <div className="row" style="padding:4px;">
//         <div className="border border-success rounded col-lg-5 col-12" style="padding: 4px; margin: 4px;">
//             <table className="table table-striped table-sm" style="margin-top: 8px;">
//                 <thead className="table-dark">
//                 <tr>
//                     <th style="width: 35%">Service name</th>
//                     <th style="width: 25%">Start time</th>
//                     <th style="width: 25%">End time</th>
//                     <th>Failed?</th>
//                 </tr>
//                 </thead>
//                 <tbody>
//                 @foreach (var log in @Model.InventoryServiceStatus)
//                 {
//                     <tr>
//                         <td>@log.ServiceName</td>
//                         <td>@log.StartTime</td>
//                         <td>@log.EndTime</td>
//                         <td>@log.Failed</td>
//                     </tr>
//                 }
//                 </tbody>
//             </table>
//         </div>
//         <div className="border border-primary rounded col-lg-5 col-12" style="padding: 4px; margin: 4px;">
//             <table className="table table-striped table-sm" style="margin-top: 8px;">
//                 <thead className="table-dark">
//                 <tr>
//                     <th style="width: 35%">Service name</th>
//                     <th style="width: 25%">Start time</th>
//                     <th style="width: 25%">End time</th>
//                     <th>Failed?</th>
//                 </tr>
//                 </thead>
//                 <tbody>
//                 @foreach (var log in @Model.EmailServiceStatus)
//                 {
//                     <tr>
//                         <td>@log.ServiceName</td>
//                         <td>@log.StartTime</td>
//                         <td>@log.EndTime</td>
//                         <td>@log.Failed</td>
//                     </tr>
//                 }
//                 </tbody>
//             </table>
//         </div>
//         <div className="border border-info rounded col-lg-5 col-12" style="padding: 4px; margin: 4px;">
//             <table className="table table-striped table-sm" style="margin-top: 8px;">
//                 <thead className="table-dark">
//                 <tr>
//                     <th style="width: 35%">Service name</th>
//                     <th style="width: 25%">Start time</th>
//                     <th style="width: 25%">End time</th>
//                     <th>Failed?</th>
//                 </tr>
//                 </thead>
//                 <tbody>
//                 @foreach (var log in @Model.FulfillmentServiceStatus)
//                 {
//                     <tr>
//                         <td>@log.ServiceName</td>
//                         <td>@log.StartTime</td>
//                         <td>@log.EndTime</td>
//                         <td>@log.Failed</td>
//                     </tr>
//                 }
//                 </tbody>
//             </table>
//         </div>
//         <div className="border border-secondary rounded col-lg-5 col-12" style="padding: 4px; margin: 4px;">
//             <table className="table table-striped table-sm" style="margin-top: 8px;">
//                 <thead className="table-dark">
//                 <tr>
//                     <th style="width: 35%">Service name</th>
//                     <th style="width: 25%">Start time</th>
//                     <th style="width: 25%">End time</th>
//                     <th>Failed?</th>
//                 </tr>
//                 </thead>
//                 <tbody>
//                 @foreach (var log in @Model.OrderPlacementServiceStatus)
//                 {
//                     <tr>
//                         <td>@log.ServiceName</td>
//                         <td>@log.StartTime</td>
//                         <td>@log.EndTime</td>
//                         <td>@log.Failed</td>
//                     </tr>
//                 }
//                 </tbody>
//             </table>
//         </div>
//     </div>
