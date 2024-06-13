import CapEntry from './CapEntry';
import React, { useEffect, useState } from 'react';
import { useSharedContext } from '../../context/SharedContext';
import { CapApi } from '../../api/CapApi';
import { CapRecord } from '../../model/CapRecord';
import { Modal } from 'react-responsive-modal';
import { Alerter } from '../../layout/Alerter';
import { CapDto } from '../../api/dto/CapDto';
import "./EditCaps.css";
import { StyleRecord } from '../../model/StyleRecord';
import StyleEntry from './StyleEntry';
import { CapStyleDto } from '../../api/dto/CapStyleDto';


export default function EditCaps() {

    const sharedContextData = useSharedContext();

    const [capRecords, setCapRecords] = useState<CapRecord[]>(() => []);
    const [editCapOpen, setEditCapOpen] = useState<boolean>(false);
    const [editStyleOpen, setEditStyleOpen] = useState<boolean>(false);
    const [capsChanged, setCapsChanged] = useState<number>(0);

    const [capRecord, setCapRecord] = useState<CapRecord | null>(null);
    const [title, setTitle] = useState<string>('');
    const [capId, setCapId] = useState<number | null>(null);
    const [capCode, setCapCode] = useState<string>('');
    const [filename, setFilename] = useState<string>('');
    const [frontLeft, setFrontLeft] = useState<boolean>(false);
    const [frontCenter, setFrontCenter] = useState<boolean>(false);
    const [frontRight, setFrontRight] = useState<boolean>(false);
    const [billLeft, setBillLeft] = useState<boolean>(false);
    const [billCenter, setBillCenter] = useState<boolean>(false);
    const [billRight, setBillRight] = useState<boolean>(false);
    const [leftSide, setLeftSide] = useState<boolean>(false);
    const [rightSide, setRightSide] = useState<boolean>(false);
    const [back, setBack] = useState<boolean>(false);
    const [strap, setStrap] = useState<boolean>(false);
    const [imagesRight, setImagesRight] = useState<boolean>(false);

    const [styles, setStyles] = useState<StyleRecord[]>([]);
    const [editStyleMode, setEditStyleMode] = useState<boolean>(false);
    const [styleRecord, setStyleRecord] = useState<StyleRecord | null>(null);
    const [styleId, setStyleId] = useState<number | null>(null);
    const [styleName, setStyleName] = useState<string>('');
    const [artboardName, setArtboardName] = useState<string>('');

    const loadCapRecords = async () => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            const records = await CapApi.loadCapRecords(authenticatedUser);
            if (records) {
                setCapRecords(records);
            }
        }
    }

    function openEditCapModal(toEdit: CapRecord | null) {
        setCapRecord(toEdit);

        setTitle("Artwork Generator: " + (toEdit ? "View/Edit Cap" : "Add New Cap"));

        setCapId(toEdit ? toEdit.id : null);
        setCapCode(toEdit ? toEdit.cap_code : '');
        setFilename(toEdit ? toEdit.filename : '');
        setFrontLeft(toEdit ? toEdit.has_front_left : false);
        setFrontCenter(toEdit ? toEdit.has_front_center : false);
        setFrontRight(toEdit ? toEdit.has_front_right : false);
        setBillLeft(toEdit ? toEdit.has_bill_left : false);
        setBillCenter(toEdit ? toEdit.has_bill_center : false);
        setBillLeft(toEdit ? toEdit.has_bill_left : false);
        setLeftSide(toEdit ? toEdit.has_side_left : false);
        setRightSide(toEdit ? toEdit.has_side_right : false);
        setBack(toEdit ? toEdit.has_back : false);
        setStrap(toEdit ? toEdit.has_strap : false);
        setImagesRight(toEdit ? toEdit.images_on_right : false);

        setEditCapOpen(true);
    }

    function closeEditCapModal() {
        setEditCapOpen(false);
    }

    function openEditStyleModal(cap: CapRecord) {
        setCapRecord(cap);
        setEditStyleMode(false)

        setTitle("Artwork Generator: View/Edit Cap Styles");

        setCapId(cap.id);
        setCapCode(cap.cap_code);
        setFilename(cap.filename);
        setStyles(cap.styles)

        setEditStyleOpen(true);
    }

    function closeEditStyleModal() {
        setEditStyleOpen(false);
    }

    function submitEditCap(e: any) {
        e.preventDefault();

        let isValid = true;

        if (capCode.length == 0) {
            Alerter.showWarning("Cap code must be provided before saving cap");
            isValid = false;
        }
        if (filename.length == 0) {
            Alerter.showWarning("Filename must be provided before saving cap");
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        const dto = new CapDto(capCode, filename, frontRight, frontCenter, frontLeft, billRight, billCenter, billLeft, leftSide, rightSide, back, strap, imagesRight, capId);
        Alerter.showInfo("Attempting to save cap record...", Alerter.DEFAULT_TIMEOUT)
        if (capRecord) {
            updateCap(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Updating cap record was successful!", Alerter.DEFAULT_TIMEOUT)
                    setCapsChanged(capsChanged => capsChanged + 1);
                    closeEditCapModal();
                } else {
                    Alerter.showError("Updating cap failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
                }
            });
        } else {
            saveNewCap(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Saving new cap was successful!", Alerter.DEFAULT_TIMEOUT)
                    setCapsChanged(capsChanged => capsChanged + 1);
                    closeEditCapModal();
                } else {
                    Alerter.showError("Saving new cap failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
                }
            });
        }
    }

    const saveNewCap = async (cap: CapDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await CapApi.createNewCap(cap, authenticatedUser);
        }
        console.log("Unable to find authenticated user to save new cap!");
        return false;
    }

    const updateCap = async (cap: CapDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await CapApi.updateCap(cap, authenticatedUser);
        }
        console.log("Unable to find authenticated user to update cap!");
        return false;
    }

    function showEditStyleSection(toEdit: StyleRecord | null) {
        setStyleRecord(toEdit);

        setStyleId(toEdit ? toEdit.id : null);
        setCapId(capRecord!.id)
        setStyleName(toEdit ? toEdit.style_name : '');
        setArtboardName(toEdit ? toEdit.artboard_name : '');

        setEditStyleMode(true);
    }

    function submitEditStyle(e: any) {
        e.preventDefault();

        let isValid = true;

        if (styleName.length == 0) {
            Alerter.showWarning("Style name must be provided before saving style for cap");
            isValid = false;
        }
        if (artboardName.length == 0) {
            Alerter.showWarning("Artboard name must be provided before saving style for cap");
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        const dto = new CapStyleDto(capId, styleName, artboardName, styleId);
        Alerter.showInfo("Attempting to save style record...", Alerter.DEFAULT_TIMEOUT)
        if (styleRecord) {
            updateStyle(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Updating style record was successful!", Alerter.DEFAULT_TIMEOUT)
//setStylesChanged(stylesChanged => stylesChanged + 1);
                    setEditStyleMode(false);
                } else {
                    Alerter.showError("Updating style failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
                }
            });
        } else {
            saveNewStyle(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Saving new style was successful!", Alerter.DEFAULT_TIMEOUT)
//setStylesChanged(stylesChanged => stylesChanged + 1);
                    setEditStyleMode(false);
                } else {
                    Alerter.showError("Saving new style failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
                }
            });
        }
    }

    const saveNewStyle = async (style: CapStyleDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await CapApi.createNewStyle(style, authenticatedUser);
        }
        console.log("Unable to find authenticated user to save new cap!");
        return false;
    }

    const updateStyle = async (style: CapStyleDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await CapApi.updateStyle(style, authenticatedUser);
        }
        console.log("Unable to find authenticated user to update cap!");
        return false;
    }


    useEffect(() => {
        loadCapRecords().catch(console.error);
    }, [capsChanged]);

    return (
        <div>
            <h2><span className="ca-red">Edit</span> <span className="ca-blue">Caps</span></h2>
            {!capRecords &&
                <div className="ca-spinner-box spinner-border text-primary"><span className="sr-only"></span></div>}
            {capRecords &&
                <div>
                    <div className="ca-operation-primary-button">
                        <button className="btn btn-outline-primary"
                                onClick={() => openEditCapModal(null)}>
                            Add new cap
                        </button>
                    </div>
                    <div className="ca_tabular-data-container">
                        <table className="table table-striped table-hover">
                            <thead>
                            <tr>
                                <th>Code</th>
                                <th>Filename</th>
                                <th>Fr Rt</th>
                                <th>Fr Ctr</th>
                                <th>Fr Lt</th>
                                <th>B Rt</th>
                                <th>B Ctr</th>
                                <th>B Lt</th>
                                <th>Left</th>
                                <th>Right</th>
                                <th>Back</th>
                                <th>Strap</th>
                                <th>Imgs Rt</th>
                                <th># Styles</th>
                                <th>Operations</th>
                            </tr>
                            </thead>
                            <tbody>
                            {
                                (capRecords).map(function (cap) {
                                    return (
                                        <tr>
                                            <CapEntry record={cap}/>
                                            <td>
                                                <button className="btn btn-sm btn-outline-primary"
                                                        onClick={() => openEditCapModal(cap)}>Show/Edit Cap
                                                </button>
                                                <button className="btn btn-sm btn-outline-primary ca-operation-buttons"
                                                        onClick={() => openEditStyleModal(cap)}>Styles
                                                </button>
                                            </td>
                                        </tr>
                                    )
                                        ;
                                })
                            }
                            </tbody>
                        </table>
                        {/*// DONE 1. show list of existing caps*/}
                        {/*// Option: permit show styles for a cap*/}
                        {/*// Option: permit editing styles for a cap*/}
                        {/*// Option: permit editing positions for a cap*/}
                        {/*// Option: add a new style for a cap*/}
                        {/*// DONE 2. permit adding a new cap*/}
                    </div>

                    <div>
                        <Modal open={editCapOpen}
                               onClose={closeEditCapModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'customCapModal',
                               }}>
                            <form onSubmit={submitEditCap}>
                                <div><h2>{title}</h2></div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="capcode" className="col-form-label">Cap Code:</label>
                                        <input type="text" id="capcode" className="form-control" name="capcode"
                                               required={true}
                                               value={capCode}
                                               maxLength={20}
                                               onChange={e => setCapCode(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="filename" className="col-form-label">Filename:</label>
                                        <input type="text" id="filename" className="form-control" name="filename"
                                               required={true}
                                               value={filename}
                                               maxLength={255}
                                               onChange={e => setFilename(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>&nbsp;</div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label className="col-form-label">Locations:</label>
                                        <br/>
                                        <input type="checkbox" name="frl" checked={frontLeft}
                                               onChange={e => setFrontLeft(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Front Left</span>

                                        <input type="checkbox" name="frc" checked={frontCenter}
                                               onChange={e => setFrontCenter(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Front Center</span>

                                        <input type="checkbox" name="frr" checked={frontRight}
                                               onChange={e => setFrontRight(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Front Right</span>

                                        <input type="checkbox" name="bl" checked={billLeft}
                                               onChange={e => setBillLeft(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Bill Left</span>

                                        <input type="checkbox" name="bc" checked={billCenter}
                                               onChange={e => setBillCenter(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Bill Center</span>

                                        <input type="checkbox" name="br" checked={billRight}
                                               onChange={e => setBillRight(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Bill Right</span>
                                        <br/>

                                        <input type="checkbox" name="left" checked={leftSide}
                                               onChange={e => setLeftSide(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Left Side</span>

                                        <input type="checkbox" name="right" checked={rightSide}
                                               onChange={e => setRightSide(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Right Side</span>

                                        <input type="checkbox" name="back" checked={back}
                                               onChange={e => setBack(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Back</span>

                                        <input type="checkbox" name="strap" checked={strap}
                                               onChange={e => setStrap(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Strap</span>
                                    </div>
                                </div>

                                <div className='row ca-form-row'>&nbsp;</div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label className="col-form-label">Artwork artifact side (generally
                                            left):</label>
                                        <br/>
                                        <input type="checkbox" name="imgside" checked={imagesRight}
                                               onChange={e => setImagesRight(e.target.checked)}/>
                                        <span className="modal-checkbox-title">Images on Right of Artboard</span>
                                    </div>
                                </div>

                                <div className="row ca-form-button-row">
                                    <div className='modal-navigation-row justify-content-end'>
                                        <button className="btn btn-outline-primary" type="submit">Save changes</button>
                                        &nbsp;
                                        <button className="btn btn-outline-secondary"
                                                type="button"
                                                onClick={closeEditCapModal}>Cancel
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </Modal>
                    </div>

                    <div>
                        <Modal open={editStyleOpen}
                               onClose={closeEditStyleModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'customStyleModal',
                               }}>
                            <div><h2>{title}</h2></div>
                            <div className='cap-label-header'>
                                <div className="cap-label">Cap Code:</div>
                                <div className="cap-value">{capCode}</div>
                                <div className="cap-label">Filename:</div>
                                <div className="cap-value">{filename}</div>
                            </div>

                            {
                                editStyleMode &&
                                <form onSubmit={submitEditStyle}>
                                    <div className='row ca-form-row'>
                                        <div>
                                            <label htmlFor="stylename" className="col-form-label">Style name:</label>
                                            <input type="text" id="stylename" className="form-control" name="stylename"
                                                   required={true}
                                                   value={styleName}
                                                   maxLength={45}
                                                   onChange={e => setStyleName(e.target.value)}/>
                                        </div>
                                    </div>
                                    <div className='row ca-form-row'>
                                        <div>
                                            <label htmlFor="artboardname" className="col-form-label">Artboard name:</label>
                                            <input type="text" id="artboardname" className="form-control" name="artboardname"
                                                   required={true}
                                                   value={artboardName}
                                                   maxLength={45}
                                                   onChange={e => setArtboardName(e.target.value)}/>
                                        </div>
                                    </div>

                                    <div className="row ca-form-button-row">
                                        <div className='modal-navigation-row justify-content-end'>
                                            <button className="btn btn-outline-primary" type="submit">Save changes
                                            </button>
                                            &nbsp;
                                            <button className="btn btn-outline-secondary"
                                                    type="button"
                                                    onClick={e => setEditStyleMode(false)}>Cancel
                                            </button>
                                        </div>
                                    </div>
                                </form>
                            }
                            {
                                !editStyleMode &&
                                <>
                                    <div className="ca-operation-primary-button">
                                        <button className="btn btn-outline-primary"
                                                onClick={e => showEditStyleSection(null)}>
                                            Add new style
                                        </button>
                                    </div>

                                    <div className="ca_tabular-data-container">
                                        <table className="table table-striped table-hover">
                                            <thead>
                                            <tr>
                                                <th>Style name</th>
                                                <th>Artboard name</th>
                                                <th>Operations</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            {
                                                (styles).map(function (style) {
                                                    return (
                                                        <tr>
                                                            <StyleEntry record={style}/>
                                                            <td>
                                                                <button className="btn btn-sm btn-outline-primary"
                                                                        onClick={e => showEditStyleSection(style)}>
                                                                    Edit Style
                                                                </button>
                                                            </td>
                                                        </tr>
                                                    );
                                                })
                                            }
                                            </tbody>
                                        </table>
                                    </div>

                                    <div className="row ca-form-button-row">
                                        <div className='modal-navigation-row justify-content-end'>
                                            <button className="btn btn-outline-secondary"
                                                    type="button"
                                                    onClick={closeEditStyleModal}>Close
                                            </button>
                                        </div>
                                    </div>
                                </>
                            }
                        </Modal>
                    </div>
                </div>
            }
        </div>
    );
}
