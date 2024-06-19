
import "./Config.css";


export default function Settings() {

    return (
        <div className='container'>
            <h1 className="display-page-title operation-header">Make My Cap Server Settings</h1>

            <div className="row status-block-row form-section">
                <form className="col-md-9 col-lg-7 col-xl-6 col-12 border rounded border-info">
                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="InventoryCheckHours" className="col-form-label">Inventory check
                                (Hours)</label>
                            <input type="number" id="InventoryCheckHours" className="form-control" required/>
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="FulfillmentCheckHours" className="col-form-label">Fulfillment check
                                (Hours)</label>
                            <input type="number" id="FulfillmentCheckHours" className="form-control" required/>
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="NextPoSequence" className="col-form-label">Next PO sequence number</label>
                            <input type="number" id="NextPoSequence" className="form-control" required/>
                        </div>
                    </div>

                    <div className="row mmc-form-row mmc-form-button-row form-button-row">
                        <button type="submit" className="btn btn-primary">Update settings</button>
                    </div>
                </form>
            </div>


            <div className="row status-block-row form-section">
                <form className="col-md-9 col-lg-7 col-xl-6 col-12 border rounded border-warning">
                    <div className="row mmc-form-row form-instructions">
                        These first Email addresses are sent WARNING/STATUS Emails with information from the server that
                        may be of interest. The system is not too "chatty" but there may be a few messages sent
                        throughout a day. One Email is required and up to three will be accepted.
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="WarningEmail1" className="col-form-label">Warning Email #1
                                (Required)</label>
                            <input type="email" id="WarningEmail1" maxLength={120} className="form-control" required/>
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="WarningEmail2" className="col-form-label">Warning Email #2</label>
                            <input type="email" id="WarningEmail2" maxLength={120} className="form-control"/>
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="WarningEmail3" className="col-form-label">Warning Email #3</label>
                            <input type="email" id="WarningEmail3" maxLength={120} className="form-control"/>
                        </div>
                    </div>

                    <div>&nbsp;</div>

                    <div className="row mmc-form-row form-instructions">
                        These Email addresses are sent CRITICAL Emails with information from the server that needs some
                        action to be taken. These message should be rare. One Email is required and up to three will be
                        accepted. It is recommended that there be three recipients to ensure action is taken.
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="CriticalEmail1" className="col-form-label">Critical Error Email #1
                                (Required)</label>
                            <input type="email" id="CriticalEmail1" maxLength={120} className="form-control" required />
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="CriticalEmail2" className="col-form-label">Critical Error Email #2</label>
                            <input type="email" id="CriticalEmail2" maxLength={120} className="form-control"/>
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="CriticalEmail3" className="col-form-label">Critical Error Email #3</label>
                            <input type="email" id="CriticalEmail3" maxLength={120} className="form-control"/>
                        </div>
                    </div>

                    <div className="row mmc-form-row mmc-form-button-row form-button-row">
                        <button type="submit" className="btn btn-primary">Update notification emails</button>
                    </div>
                </form>
            </div>
        </div>
    );
}
