import StatusBlock from './StatusBlock';
import { useEffect, useState } from 'react';
import { useSharedContext } from '../../context/SharedContext';
import "./Operation.css";
import { ServiceStatusDto } from '../../api/dto/ServiceStatusDto';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';
import { OperationApi } from '../../api/OperationApi';


export default function ShowStatus() {

    const sharedContextData = useSharedContext();

    const [emailStatus, setEmailStatus] = useState<ServiceStatusDto[]>([]);
    const [fulfillmentStatus, setFulfillmentStatus] = useState<ServiceStatusDto[]>([]);
    const [inventoryStatus, setInventoryStatus] = useState<ServiceStatusDto[]>([]);
    const [orderStatus, setOrderStatus] = useState<ServiceStatusDto[]>([]);

    const loadEmailStatus = async (user: AuthenticatedUser) => {
        const lookup = await OperationApi.getEmailServiceStatus(user);
        setEmailStatus(lookup ? lookup : []);
    }

    const loadFulfillmentStatus = async (user: AuthenticatedUser) => {
        const lookup = await OperationApi.getFulfillmentServiceStatus(user);
        setFulfillmentStatus(lookup ? lookup : []);
    }

    const loadInventoryStatus = async (user: AuthenticatedUser) => {
        const lookup = await OperationApi.getInventoryServiceStatus(user);
        setInventoryStatus(lookup ? lookup : []);
    }

    const loadOrderStatus = async (user: AuthenticatedUser) => {
        console.log("TIMEOUT FINISHED")
        const lookup = await OperationApi.getOrderServiceStatus(user);
        setOrderStatus(lookup ? lookup : []);
    }

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            loadEmailStatus(user).catch(() => { console.log("Error loading Email statuses") } );
            loadFulfillmentStatus(user).catch(() => { console.log("Error loading Fulfillment statuses") } );
            loadInventoryStatus(user).catch(() => { console.log("Error loading Inventory statuses") } );
            loadOrderStatus(user).catch(() => { console.log("Error loading Order statuses") } );
        }
    }, []);

    return (
        <>
            <h1 className="display-page-title operation-header">Make My Cap Server Status</h1>
            <div className="row status-block-row">
                <StatusBlock serviceStatus={orderStatus} borderColor="border-success" />
                <StatusBlock serviceStatus={fulfillmentStatus} borderColor="border-info" />
            </div>
            <div className="row status-block-row">
                <StatusBlock serviceStatus={inventoryStatus} borderColor="border-danger"/>
                <StatusBlock serviceStatus={emailStatus} borderColor="border-primary" />
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
