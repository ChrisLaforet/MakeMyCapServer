import "./Config.css";
import { useEffect, useRef, useState } from 'react';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';
import { useSharedContext } from '../../context/SharedContext';
import { SettingsApi } from '../../api/SettingsApi';
import { Alerter } from '../../layout/Alerter';
import { NotificationEmailsDto } from '../../api/dto/NotificationEmailsDto';
import { SettingsDto } from '../../api/dto/SettingsDto';


export default function Settings() {

    const sharedContextData = useSharedContext();

    const [valueChanged, setValueChanged] = useState(0)
    const [value2Changed, setValue2Changed] = useState(0)

    const warningEmail1 = useRef<string | null>(null);
    const warningEmail2 = useRef<string | null>(null);
    const warningEmail3 = useRef<string | null>(null);
    const criticalEmail1 = useRef<string | null>(null);
    const criticalEmail2 = useRef<string | null>(null);
    const criticalEmail3 = useRef<string | null>(null);

    const inventoryCheckHours = useRef<number>(0);
    const fulfillmentCheckHours = useRef<number>(0);
    const nextPoSequence = useRef<number>(0);

    const loadEmailNotifications = async (user: AuthenticatedUser) => {
        const lookup = await SettingsApi.getNotifications(user);
        if (lookup) {
            warningEmail1.current = lookup.warningEmail1;
            warningEmail2.current = lookup.warningEmail2;
            warningEmail3.current = lookup.warningEmail3;
            criticalEmail1.current = lookup.criticalEmail1;
            criticalEmail2.current = lookup.criticalEmail2;
            criticalEmail3.current = lookup.criticalEmail3;
            setValueChanged(valueChanged + 1);
        }
    }

    const loadSettings = async (user: AuthenticatedUser) => {
        const lookup = await SettingsApi.getSettings(user);
        if (lookup) {
            inventoryCheckHours.current = lookup.inventoryCheckHours;
            fulfillmentCheckHours.current = lookup.fulfillmentCheckHours;
            nextPoSequence.current = lookup.nextPoSequence;
            setValue2Changed(value2Changed + 1);
        }
    }

    function setValueFor(fieldName: string, value: string) {
        let numberValue = parseInt(value, 10);
        if (!Number.isNaN(numberValue) && numberValue >= 0) {
            if (numberValue == 0) {
                numberValue = 1;
            }
            if (fieldName == 'inventoryCheckHours') {
                const finalValue = numberValue < 24 ? numberValue : inventoryCheckHours.current;
                if (inventoryCheckHours.current != finalValue) {
                    inventoryCheckHours.current = finalValue;
                    setValueChanged(valueChanged + 1);
                }
            } else if (fieldName == 'fulfillmentCheckHours') {
                const finalValue = numberValue < 24 ? numberValue : fulfillmentCheckHours.current;
                if (fulfillmentCheckHours.current != finalValue) {
                    fulfillmentCheckHours.current = finalValue;
                    setValueChanged(valueChanged + 1);
                }
            } else {
                const finalValue = numberValue < 100000 ? numberValue : nextPoSequence.current;
                if (nextPoSequence.current != finalValue) {
                    nextPoSequence.current = numberValue < 100000 ? numberValue : 1;
                    setValueChanged(valueChanged + 1);
                }
            }
        }
    }
    
    function setEmailFor(fieldName: string, value: string) {
        if (fieldName == 'warningEmail1') {
            warningEmail1.current = value;
        }
        else if (fieldName == 'warningEmail2') {
            warningEmail2.current = value;
        }
        else if (fieldName == 'warningEmail3') {
            warningEmail3.current = value;
        }
        else if (fieldName == 'criticalEmail1') {
            criticalEmail1.current = value;
        }
        else if (fieldName == 'criticalEmail2') {
            criticalEmail2.current = value;
        }
        else if (fieldName == 'criticalEmail3') {
            criticalEmail3.current = value;
        }
        setValue2Changed(value2Changed + 1);
    }

    function saveSettings(event: any) {
        event.preventDefault();

        if (inventoryCheckHours.current < 1 || inventoryCheckHours.current > 23) {
            Alerter.showInfo("Inventory check time must be between 1 and 23 hours inclusive", Alerter.DEFAULT_TIMEOUT);
            return;
        }
        if (fulfillmentCheckHours.current < 1 || fulfillmentCheckHours.current > 23) {
            Alerter.showInfo("Fulfillment time must be between 1 and 23 hours inclusive", Alerter.DEFAULT_TIMEOUT);
            return;
        }
        if (nextPoSequence.current < 1 || nextPoSequence.current > 99_999) {
            Alerter.showInfo("Next PO value must be between 1 and 99999 inclusive", Alerter.DEFAULT_TIMEOUT);
            return;
        }

        const dto = new SettingsDto(inventoryCheckHours.current,
            fulfillmentCheckHours.current, nextPoSequence.current);

        Alerter.showInfo("Attempting to save settings...", Alerter.DEFAULT_TIMEOUT)
        updateSettings(dto).then(response => {
            if (response) {
                Alerter.showSuccess("Updating settings was successful!", Alerter.DEFAULT_TIMEOUT)
            } else {
                Alerter.showError("Updating settings failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
            }
        });
    }

    const updateSettings = async (settingsDto: SettingsDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await SettingsApi.saveSettings(settingsDto, authenticatedUser);
        }
        console.log("Unable to find authenticated user to update settings!");
        return false;
    }

    function saveEmailNotifications(event: any) {
        event.preventDefault();

        if (warningEmail1.current == null || warningEmail1.current.trim().length == 0) {
            Alerter.showInfo("Warning notification Email 1 must be set", Alerter.DEFAULT_TIMEOUT);
            return;
        }
        if (criticalEmail1.current == null || criticalEmail1.current.trim().length == 0) {
            Alerter.showInfo("Critical notification Email 1 must be set", Alerter.DEFAULT_TIMEOUT);
            return;
        }

        const dto = new NotificationEmailsDto(warningEmail1.current.trim(),
            warningEmail2.current == null ? null : warningEmail2.current.trim(),
            warningEmail3.current == null ? null : warningEmail3.current.trim(),
            criticalEmail1.current.trim(),
            criticalEmail2.current == null ? null : criticalEmail2.current.trim(),
            criticalEmail3.current == null ? null : criticalEmail3.current.trim());

        Alerter.showInfo("Attempting to save notification emails...", Alerter.DEFAULT_TIMEOUT)
        updateNotifications(dto).then(response => {
            if (response) {
                Alerter.showSuccess("Updating notification emails was successful!", Alerter.DEFAULT_TIMEOUT)
            } else {
                Alerter.showError("Updating notification emails failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
            }
        });
    }

    const updateNotifications = async (notificationsDto: NotificationEmailsDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await SettingsApi.saveNotifications(notificationsDto, authenticatedUser);
        }
        console.log("Unable to find authenticated user to update notifications!");
        return false;
    }

    useEffect(() => {
        // dummy to refresh values
    }, [valueChanged, value2Changed]);

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            loadEmailNotifications(user).catch(() => {
                console.log("Error loading Email notifications")
            });
            loadSettings(user).catch(() => {
                console.log("Error loading settings")
            });
        }
    }, []);

    return (
        <div className='container'>
            <h1 className="display-page-title operation-header">Make My Cap Server Settings</h1>

            <div className="row status-block-row form-section">
                <form className="col-md-9 col-lg-7 col-xl-6 col-12 border rounded border-info" onSubmit={saveSettings}>
                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="InventoryCheckHours" className="col-form-label">Inventory check
                                (Hours)</label>
                            <input type="number" id="InventoryCheckHours" className="form-control"
                                    required
                                    min={1}
                                    max={24}
                                   value={inventoryCheckHours.current}
                                   onChange={e => setValueFor("inventoryCheckHours", e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="FulfillmentCheckHours" className="col-form-label">Fulfillment check
                                (Hours)</label>
                            <input type="number" id="FulfillmentCheckHours" className="form-control"
                                   required
                                   min={1}
                                   max={24}
                                   value={fulfillmentCheckHours.current}
                                   onChange={e => setValueFor("fulfillmentCheckHours", e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="NextPoSequence" className="col-form-label">Next PO sequence number</label>
                            <input type="number" id="NextPoSequence" className="form-control"
                                   required
                                   min={1}
                                   max={100000}
                                   value={nextPoSequence.current}
                                   onChange={e => setValueFor("nextPoSequence", e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="row mmc-form-row mmc-form-button-row form-button-row">
                        <button type="submit" className="btn btn-primary">Update settings</button>
                    </div>
                </form>
            </div>


            <div className="row status-block-row form-section">
                <form className="col-md-9 col-lg-7 col-xl-6 col-12 border rounded border-warning" onSubmit={saveEmailNotifications}>
                    <div className="row mmc-form-row form-instructions">
                        These first Email addresses are sent WARNING/STATUS Emails with information from the server that
                        may be of interest. The system is not too "chatty" but there may be a few messages sent
                        throughout a day. One Email is required and up to three will be accepted.
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="WarningEmail1" className="col-form-label">Warning Email #1
                                (Required)</label>
                            <input type="email" id="WarningEmail1" maxLength={120} className="form-control"
                                   required
                                   value={warningEmail1.current == null ? '' : warningEmail1.current}
                                   onChange={(e) => setEmailFor("warningEmail1", e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="WarningEmail2" className="col-form-label">Warning Email #2</label>
                            <input type="email" id="WarningEmail2" maxLength={120} className="form-control"
                                   value={warningEmail2.current == null ? '' : warningEmail2.current}
                                   onChange={(e) => setEmailFor("warningEmail2", e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="WarningEmail3" className="col-form-label">Warning Email #3</label>
                            <input type="email" id="WarningEmail3" maxLength={120} className="form-control"
                                   value={warningEmail3.current == null ? '' : warningEmail3.current}
                                   onChange={(e) => setEmailFor("warningEmail3", e.target.value)}
                            />
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
                            <input type="email" id="CriticalEmail1" maxLength={120} className="form-control"
                                   required
                                   value={criticalEmail1.current == null ? '' : criticalEmail1.current}
                                   onChange={(e) => setEmailFor("criticalEmail1", e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="CriticalEmail2" className="col-form-label">Critical Error Email #2</label>
                            <input type="email" id="CriticalEmail2" maxLength={120} className="form-control"
                                   value={criticalEmail2.current == null ? '' : criticalEmail2.current}
                                   onChange={(e) => setEmailFor("criticalEmail2", e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="row mmc-form-row">
                        <div>
                            <label htmlFor="CriticalEmail3" className="col-form-label">Critical Error Email #3</label>
                            <input type="email" id="CriticalEmail3" maxLength={120} className="form-control"
                                   value={criticalEmail3.current == null ? '' : criticalEmail3.current}
                                   onChange={(e) => setEmailFor("criticalEmail3", e.target.value)}
                            />
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
