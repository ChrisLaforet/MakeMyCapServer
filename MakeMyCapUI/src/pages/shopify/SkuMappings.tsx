import { useEffect, useRef, useState } from 'react';
import { DistributorSkusDto } from '../../api/dto/DistributorSkusDto';
import { useSharedContext } from '../../context/SharedContext';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';
import { ShopifySupportApi } from '../../api/ShopifySupportApi';
import { DistributorDto } from '../../api/dto/DistributorDto';
import { Modal } from 'react-responsive-modal';
import { SkuDto } from '../../api/dto/SkuDto';
import { Alerter } from '../../layout/Alerter';
import "./Shopify.css";


export default function SkuMappings() {

    const sharedContextData = useSharedContext();

    const distributors = useRef<DistributorDto[] | null>(null);
    const selectedDistributor = useRef<DistributorDto | null>(null);
    const [distributorSkus, setDistributorSkus] = useState<DistributorSkusDto[]>([]);

    const [createOrEditSkuOpen, setCreateOrEditSkuOpen] = useState<boolean>(false);
    const [deleteSkuOpen, setDeleteSkuOpen] = useState<boolean>(false);

    const [selectedSku, setSelectedSku] = useState<SkuDto | null>(null);
    const [mmcSku, setMmcSku] = useState<string>("");
    const [distributorSku, setDistributorSku] = useState<string>("");
    const [brand, setBrand] = useState<string>("");
    const [styleCode, setStyleCode] = useState<string>("");
    const [partId, setPartId] = useState<string>("");
    const [color, setColor] = useState<string>("");
    const [colorCode, setColorCode] = useState<string>("");
    const [sizeCode, setSizeCode] = useState<string>("");

    const [valueChanged, setValueChanged] = useState(0);

    const loadDistributors = async (user: AuthenticatedUser) => {
        const lookup = await ShopifySupportApi.getDistributors(user);
        if (lookup) {
            distributors.current = lookup;
        }
    }

    const loadDistributorSkus = async (user: AuthenticatedUser) => {
        if (distributors.current == null) {
            return;
        }
        for (const distributor of distributors.current) {
            await loadDistributorSkusFor(distributor.code, user).then(() => {
                if (selectedDistributor.current == null) {
                    selectedDistributor.current = distributor;
                }
                selectedDistributor.current = distributors.current![0];
            })
            .catch(() => {
                console.log(`Error loading distributor skus for ${ distributor.code }`)
            });
        }
        setValueChanged(valueChanged + 1);
    }

    const loadDistributorSkusFor = async (distributorCode: string, user: AuthenticatedUser) => {
        const lookup = await ShopifySupportApi.getSkusForDistributor(distributorCode, user);
        if (lookup) {
            distributorSkus.push(lookup);
        }
    }

    function selectDistributor(code: string) {
        const selection = distributors.current!.find((distributor) => distributor.code == code);
        if (selection) {
            selectedDistributor.current = selection;
            setValueChanged(valueChanged + 1);
        }
    }

    function getSkusForCurrentDistributor(): SkuDto[] {
        if (selectedDistributor.current != null) {
            const selection = distributorSkus.find((distributorSku) => distributorSku.distributor.code == selectedDistributor.current!.code);
            if (selection) {
                return selection.skus.sort((a,b) => a.sku.localeCompare(b.sku));
            }
        }
        return [];
    }

    function openCreateSkuModal() {
        setSelectedSku(null);
        setMmcSku("");
        setDistributorSku("");
        setBrand("");
        setStyleCode("")
        setPartId("");
        setColor("")
        setColorCode("");
        setSizeCode("");

        setCreateOrEditSkuOpen(true);
    }

    function checkAndSetMmcSku(sku: string) {
        if (sku == '') {
            Alerter.showWarning("Please enter a valid sku", Alerter.DEFAULT_TIMEOUT);
            return;
        }
        const match = findMatchingSkuDto(sku);
        if (match) {
            if ((selectedSku != null && match === selectedSku) ||
                selectedSku == null) {
                Alerter.showWarning("Please enter a valid sku - this one exists already", Alerter.DEFAULT_TIMEOUT);
                return;
            }
        }

        setMmcSku(sku);
    }

    function findMatchingSkuDto(sku: string): SkuDto | null {
        let match = null;
        distributorSkus.forEach((distributor) => {
            const possible = distributor.skus.find((skuDto) => skuDto.sku.toUpperCase() == sku.toUpperCase());
            if (possible) {
                match = possible;
            }
        });
        return match;
    }

    function closeCreateSkuModal() {
        setCreateOrEditSkuOpen(false);
    }

    function openEditSkuModal(sku: SkuDto) {
        setSelectedSku(sku);
        setMmcSku(sku.sku);
        setDistributorSku(sku.distributorSku == null ? '' : sku.distributorSku);
        setBrand(sku.brand == null ? '' : sku.brand);
        setStyleCode(sku.styleCode == null ? '' : sku.styleCode);
        setPartId(sku.partId == null ? '' : sku.partId);
        setColor(sku.color == null ? '' : sku.color);
        setColorCode(sku.colorCode == null ? '' : sku.colorCode);
        setSizeCode(sku.sizeCode == null ? '' : sku.sizeCode);

        setCreateOrEditSkuOpen(true);
    }

    function isFieldRequired(fieldName: string) {
        if (selectedDistributor.current == null) {
            return false;
        }
        const distributorCode = selectedDistributor.current.code;
        switch (fieldName) {
            case 'mmcSku':
                return true;

            case 'distributorSku':
                return distributorCode == 'SS';

            case 'brand':
                return ['SS', 'SM'].includes(distributorCode);

            case 'styleCode':
                return ['SS', 'CA', 'SM'].includes(distributorCode);

            case 'partId':
                return distributorCode == 'CA';

            case 'color':
                return ['SS', 'CA', 'SM'].includes(distributorCode);

            case 'colorCode':
                return false;

            case 'sizeCode':
                return false;
    }
        return false;
    }

    function submitSku(event: any) {
        event.preventDefault();

        if (selectedDistributor.current == null) {
            Alerter.showError("Something went wrong - A distributor must be selected before creating/editing a sku", Alerter.DEFAULT_TIMEOUT);
            return;
        }
        const distributorCode = selectedDistributor.current.code;

        if (mmcSku == null || mmcSku == '') {
            Alerter.showInfo("The MMC Sku cannot be empty - it must be a unique value", Alerter.DEFAULT_TIMEOUT);
            return;
        }

        const distributorSkuDto = distributorSkus.find((distributorSku) => distributorSku.distributor.code == distributorCode);
        if (distributorSkuDto == null) {
            Alerter.showWarning(`Cannot find the distributor's skus for ${distributorCode} - changes will not reflect in the listing until you reload the Skus`, Alerter.DEFAULT_TIMEOUT);
        }

        const skuDto = new SkuDto(mmcSku, distributorCode, distributorSku, brand, styleCode, partId, color, colorCode, sizeCode);

        if (selectedSku != null) {
            const originalSku = selectedSku.sku;
            updateSku(originalSku, skuDto).then(response => {
                if (response) {
                    Alerter.showSuccess("Sku was successfully updated!", Alerter.DEFAULT_TIMEOUT);
                    if (distributorSkuDto != null) {
                        const index = distributorSkuDto.skus.indexOf(selectedSku);
                        if (index >= 0) {
                            distributorSkuDto.skus.splice(index, 1);
                        }
                        distributorSkuDto.skus.push(skuDto);
                    }
                    closeCreateSkuModal();
                    setValueChanged(valueChanged + 1);
                } else {
                    Alerter.showError("Saving new sku failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT);
                }
            });
        } else {
            saveNewSku(skuDto).then(response => {
                if (response) {
                    Alerter.showSuccess("Saving new sku was successful!", Alerter.DEFAULT_TIMEOUT);
                    distributorSkuDto?.skus.push(skuDto);
                    closeCreateSkuModal();
                    setValueChanged(valueChanged + 1);
                } else {
                    Alerter.showError("Saving new sku failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT);
                }
            });
        }
    }

    const saveNewSku = async (skuDto: SkuDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await ShopifySupportApi.createSku(skuDto, authenticatedUser);
        }
        console.log("Unable to find authenticated user to save new sku!");
        return false;
    }

    const updateSku = async (originalSku: string, skuDto: SkuDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await ShopifySupportApi.updateSku(originalSku, skuDto, authenticatedUser);
        }
        console.log("Unable to find authenticated user to update sku!");
        return false;
    }

    function requestDeleteSkuModal(sku: SkuDto) {
        setSelectedSku(sku);
        setDeleteSkuOpen(true);
    }

    function closeDeleteSkuModal() {
        setDeleteSkuOpen(false);
    }

    function submitDeleteSku(event: any) {
        event.preventDefault();

        if (selectedDistributor.current == null) {
            Alerter.showError("Something went wrong - A distributor must be selected before deleting a sku", Alerter.DEFAULT_TIMEOUT);
            return;
        }
        const distributorCode = selectedDistributor.current.code;

        const distributorSkuDto = distributorSkus.find((distributorSku) => distributorSku.distributor.code == distributorCode);
        if (distributorSkuDto == null) {
            Alerter.showWarning(`Cannot find the distributor's skus for ${distributorCode} - changes will not reflect in the listing until you reload the Skus`, Alerter.DEFAULT_TIMEOUT);
        }

        if (selectedSku == null) {
            return
        }

        deleteSku(selectedSku.sku).then(response => {
            if (response) {
                Alerter.showSuccess("Sku was successfully deleted!", Alerter.DEFAULT_TIMEOUT);
                if (distributorSkuDto != null) {
                    const index = distributorSkuDto.skus.indexOf(selectedSku);
                    if (index >= 0) {
                        distributorSkuDto.skus.splice(index, 1);
                    }
                }
                closeDeleteSkuModal();
                setValueChanged(valueChanged + 1);
            } else {
                Alerter.showError("Deleted sku failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT);
            }
        });
    }

    const deleteSku = async (sku: string): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await ShopifySupportApi.deleteSku(sku, authenticatedUser);
        }
        console.log("Unable to find authenticated user to delete sku!");
        return false;
    }


    useEffect(() => {
        // for forcing redraw
    }, [valueChanged]);

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            loadDistributors(user)
                .then(() => {
                    loadDistributorSkus(user).catch(() => {
                        console.log("Error loading distributors")
                    })
                })
                .catch(() => {
                    console.log("Error loading distributors")
            });
        }
    }, []);

    return (
        <div>
            <h2><span className="ca-blue">Sku Mappings</span></h2>
            {distributorSkus.length == 0 &&
                <div className="mmc-spinner-box spinner-border text-primary"><span className="sr-only"></span></div>}
            {distributorSkus.length > 0 &&
                <div>
                    <div>
                        <div className="distributor-selection">
                            <label htmlFor="SelectDistributor" className="col-form-label">Select distributor:</label>
                            <select id="SelectDistributor" className="form-control"
                                    value={selectedDistributor.current == null ? '' : selectedDistributor.current.code}
                                    onChange={(e) => selectDistributor(e.target.value)}>
                                {
                                    (distributors.current!).map((distributor: DistributorDto) => {
                                        return (
                                            <option key={distributor.code} value={distributor.code}>
                                                {distributor.name}
                                            </option>
                                        );
                                    })
                                }
                            </select>
                        </div>
                    </div>

                    <div className="mmc-operation-primary-button">
                        <button className="btn btn-outline-primary"
                                onClick={() => openCreateSkuModal()}>
                            Add new sku
                            to {selectedDistributor.current == null ? "Distributor" : selectedDistributor.current.name}
                        </button>
                    </div>
                    <div className="mmc-tabular-data-container">
                        <table className="table table-striped table-hover">
                            <thead className="table-dark">
                            <tr>
                                <th>MMC Sku</th>
                                <th>Dist</th>
                                <th>Dist Sku</th>
                                <th>Brand</th>
                                <th>Style Code</th>
                                <th>Part Id</th>
                                <th>Color</th>
                                <th>Color Code</th>
                                <th>Size Code</th>
                                <th>Operations</th>
                            </tr>
                            </thead>
                            <tbody>
                            {
                                (getSkusForCurrentDistributor()).map(function (sku) {
                                    return (
                                        <tr key={sku.sku}>
                                            <td>{sku.sku}</td>
                                            <td>{selectedDistributor.current!.name}</td>
                                            <td>{sku.distributorSku}</td>
                                            <td>{sku.brand}</td>
                                            <td>{sku.styleCode}</td>
                                            <td>{sku.partId}</td>
                                            <td>{sku.color}</td>
                                            <td>{sku.colorCode}</td>
                                            <td>{sku.sizeCode}</td>
                                            <td>
                                                <button className="btn btn-sm btn-outline-primary"
                                                        onClick={() => openEditSkuModal(sku)}>Edit
                                                </button>
                                                <button className="btn btn-sm btn-outline-primary mmc-operation-buttons"
                                                        onClick={() => requestDeleteSkuModal(sku)}>Delete
                                                </button>
                                            </td>
                                        </tr>
                                    );
                                })
                            }
                            </tbody>
                        </table>
                    </div>

                    <div>
                        <Modal open={createOrEditSkuOpen}
                               onClose={closeCreateSkuModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'customEditSkuModal',
                               }}>
                            <form onSubmit={submitSku}>
                                <div><h2>{`Create Sku for ${selectedDistributor.current!.name}`}</h2></div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="mmcSku" className="col-form-label">MMC Sku:</label>
                                        <input type="text" id="mmcSku" className="form-control"
                                               required={isFieldRequired("mmcSku")}
                                               value={mmcSku}
                                               maxLength={30}
                                               onChange={e => checkAndSetMmcSku(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="distributorSku" className="col-form-label">Distributor's Sku:</label>
                                        <input type="text" id="distributorSku" className="form-control"
                                               required={isFieldRequired("distributorSku")}
                                               value={distributorSku}
                                               maxLength={30}
                                               onChange={e => setDistributorSku(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="brand" className="col-form-label">Brand:</label>
                                        <input type="text" id="brand" className="form-control"
                                               required={isFieldRequired("brand")}
                                               value={brand}
                                               maxLength={100}
                                               onChange={e => setBrand(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="styleCode" className="col-form-label">Style Code:</label>
                                        <input type="text" id="styleCode" className="form-control"
                                               required={isFieldRequired("styleCode")}
                                               value={styleCode}
                                               maxLength={50}
                                               onChange={e => setStyleCode(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="partId" className="col-form-label">Part Id:</label>
                                        <input type="text" id="partId" className="form-control"
                                               required={isFieldRequired("partId")}
                                               value={partId}
                                               maxLength={255}
                                               onChange={e => setPartId(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="color" className="col-form-label">Color (name):</label>
                                        <input type="text" id="color" className="form-control"
                                               required={isFieldRequired("color")}
                                               value={color}
                                               maxLength={100}
                                               onChange={e => setColor(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="colorCode" className="col-form-label">Color code:</label>
                                        <input type="text" id="colorCode" className="form-control"
                                               required={isFieldRequired("colorCode")}
                                               value={colorCode}
                                               maxLength={10}
                                               onChange={e => setColorCode(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="sizeCode" className="col-form-label">Size code:</label>
                                        <input type="text" id="sizeCode" className="form-control"
                                               required={isFieldRequired("sizeCode")}
                                               value={sizeCode}
                                               maxLength={20}
                                               onChange={e => setSizeCode(e.target.value)}/>
                                    </div>
                                </div>

                                <div className="row mmc-form-button-row">
                                    <div className='modal-navigation-row justify-content-end'>
                                        <button className="btn btn-outline-primary" type="submit">Save changes</button>
                                        &nbsp;
                                        <button className="btn btn-outline-secondary"
                                                type="button"
                                                onClick={closeCreateSkuModal}>Cancel
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </Modal>
                    </div>

                    <div>
                        <Modal open={deleteSkuOpen}
                               onClose={closeDeleteSkuModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'deleteCustomSkuModal',
                               }}>
                            <form onSubmit={submitDeleteSku}>
                                <div><h2>{`Delete Sku for ${selectedDistributor.current!.name}`}</h2></div>
                                <div className='mmc-label-header'>
                                    <div className="mmc-label">MMC Sku:</div>
                                    <div className="mmc-value">{selectedSku?.sku}</div>
                                </div>
                                <div className='mmc-label-row'>
                                    <div className="mmc-label">Distributor Sku:</div>
                                    <div className="mmc-value">{selectedSku?.distributorSku}</div>
                                </div>
                                <div className='mmc-label-row'>
                                    <div className="mmc-label">Brand:</div>
                                    <div className="mmc-value">{selectedSku?.brand}</div>
                                    <div className="mmc-label">Style Code:</div>
                                    <div className="mmc-value">{selectedSku?.styleCode}</div>
                                </div>
                                <div className='mmc-label-row'>
                                    <div className="mmc-label">Part Id:</div>
                                    <div className="mmc-value">{selectedSku?.partId}</div>
                                    <div className="mmc-label">Color:</div>
                                    <div className="mmc-value">{selectedSku?.color}</div>
                                </div>
                                <div className='mmc-label-row'>
                                    <div className="mmc-label">Color Code:</div>
                                    <div className="mmc-value">{selectedSku?.colorCode}</div>
                                    <div className="mmc-label">Size Code:</div>
                                    <div className="mmc-value">{selectedSku?.sizeCode}</div>
                                </div>

                                <div className="row mmc-form-button-row">
                                    <div className='modal-navigation-row justify-content-end'>
                                        <button className="btn btn-outline-danger" type="submit">Yes, Delete This!</button>
                                        &nbsp;
                                        <button className="btn btn-outline-secondary"
                                                type="button"
                                                onClick={closeDeleteSkuModal}>Cancel
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </Modal>
                    </div>

                </div>
            }
        </div>
    )
        ;
}
