import { useEffect, useRef, useState } from 'react';
import { InventoryDto } from '../../api/dto/InventoryDto';
import { useSharedContext } from '../../context/SharedContext';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';
import { InventoryApi } from '../../api/InventoryApi';
import { Modal } from 'react-responsive-modal';
import { Alerter } from '../../layout/Alerter';
import { AvailableSkuDto } from '../../api/dto/AvailableSkuDto';
import "./Inventory.css";
import { DistributorDto } from '../../api/dto/DistributorDto';
import { SkuDto } from '../../api/dto/SkuDto';
import { ShopifySupportApi } from '../../api/ShopifySupportApi';


export default function Inventory() {

    const sharedContextData = useSharedContext();

    const [inHouseItems, setInHouseItems] = useState<InventoryDto[] | null>(null);
    const [availableSkus, setAvailableSkus] = useState<AvailableSkuDto[] | null>(null);

    const [createInventoryOpen, setCreateInventoryOpen] = useState<boolean>(false);
    const selectedSku = useRef<string | null>(null);
    const [onHand, setOnHand] = useState<number>(0);
    const [valueChanged, setValueChanged] = useState(0);

    const loadInHouseInventory = async (user: AuthenticatedUser) => {
        const lookup = await InventoryApi.getInHouseInventory(user);
        if (lookup) {
            setInHouseItems(lookup);
        } else {
            Alerter.showError('Error encountered while loading in-house inventory', Alerter.DEFAULT_TIMEOUT);
            setInHouseItems([]);
        }
    }

    const loadAvailableSkus = async (user: AuthenticatedUser) => {
        const lookup = await InventoryApi.getAvailableSkus(user);
        if (lookup) {
            setAvailableSkus(lookup);
        } else {
            Alerter.showError('Error encountered while loading available skus', Alerter.DEFAULT_TIMEOUT);
            setAvailableSkus([]);
        }
    }

    function openCreateInventoryModal() {
        selectedSku.current = availableSkus != null ? availableSkus[0].sku : null;
        setOnHand(0);
        setCreateInventoryOpen(true);
    }

    function closeInventoryModal() {
        setCreateInventoryOpen(false);
    }

    function setOnHandValue(value: string) {
        const onHandValue = parseInt(value, 10);
        if (!Number.isNaN(onHandValue)) {
            setOnHand(onHandValue);
        }
    }

    function selectSku(value: string) {
        selectedSku.current = value;
        console.log("Selected sku: " + selectedSku.current);
        setValueChanged(valueChanged + 1);
    }

    function submitNewInventory(event: any) {
        event.preventDefault();

        if (selectedSku.current == null || selectedSku.current == '') {
            Alerter.showInfo("The Sku cannot be empty - it must be a unique value", Alerter.DEFAULT_TIMEOUT);
            return;
        }
        const skuDto = availableSkus?.find((availableSku) => availableSku.sku == selectedSku.current);
        if (skuDto == null) {
            Alerter.showError("The Sku cannot be found in the available list so it cannot be saved", Alerter.DEFAULT_TIMEOUT);
            return;
        }

        const quantityOnHand = onHand >= 0 ? onHand : 0;

        saveNewInventory(skuDto.sku, quantityOnHand).then(response => {
            if (response) {
                Alerter.showSuccess("Saving new in-house inventory was successful!", Alerter.DEFAULT_TIMEOUT);
                inHouseItems?.push(new InventoryDto(skuDto.sku, skuDto.description, quantityOnHand, 0, skuDto.distributorCode, skuDto.distributorName));
                if (availableSkus != null) {
                    const index = availableSkus.indexOf(skuDto);
                    if (index >= 0) {
                        availableSkus.splice(index, 1);
                    }
                }
                closeInventoryModal();
                setValueChanged(valueChanged + 1);
            } else {
                Alerter.showError("Saving new in-house inventory failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT);
            }
        });
    }

    const saveNewInventory = async (sku: string, onHand: number): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await InventoryApi.createInHouseInventory(sku, onHand, authenticatedUser);
        }
        console.log("Unable to find authenticated user to save new in-house inventory!");
        return false;
    }

    useEffect(() => {
        // for forcing redraw
    }, [valueChanged]);

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            loadAvailableSkus(user)
                .catch(() => {
                console.log("Error loading available skus")
            });

            loadInHouseInventory(user)
                .catch(() => {
                    console.log("Error loading inventory")
                });
        }
    }, []);

    return (
        <div>
            <h2><span className="ca-blue">In-House Inventory</span></h2>
            {
                inHouseItems == null && availableSkus == null &&
                <div className="mmc-spinner-box spinner-border text-primary"><span className="sr-only"></span></div>
            }
            {
                inHouseItems != null && availableSkus != null &&
                <div>
                    {
                        availableSkus.length > 0 &&
                        <div className="mmc-operation-primary-button">
                            <button className="btn btn-outline-primary"
                                    onClick={() => openCreateInventoryModal()}
                            >
                                Add new in-house Inventory
                            </button>
                        </div>
                    }
                    <div className="mmc-tabular-data-container">
                        <table className="table table-striped table-hover">
                            <thead className="table-dark">
                            <tr>
                                <th>MMC Sku</th>
                                <th>Dist Code</th>
                                <th>Dist Name</th>
                                <th>Description</th>
                                <th>On Hand Qty</th>
                                <th>Last Use Qty</th>
                                <th>Operations</th>
                            </tr>
                            </thead>
                            <tbody>
                            {
                                (inHouseItems).map(function (item) {
                                    return (
                                        <tr key={item.sku}>
                                            <td>{item.sku}</td>
                                            <td>{item.distributorCode}</td>
                                            <td>{item.distributorName}</td>
                                            <td>{item.description}</td>
                                            <td>{item.onHand}</td>
                                            <td>{item.lastUsage}</td>
                                            <td>
                                                <button className="btn btn-sm btn-outline-primary">Update</button>
                                            </td>
                                        </tr>
                                    );
                                })
                            }
                            </tbody>
                        </table>
                    </div>

                    <div>
                        <Modal open={createInventoryOpen}
                               onClose={closeInventoryModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'customCreateInventoryModal',
                               }}>
                            <form onSubmit={submitNewInventory}>
                                <div><h2>Create In-House Inventory</h2></div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="sku" className="col-form-label">Select Sku:</label>
                                        <select id="sku" className="form-control"
                                                required
                                                value={selectedSku.current == null ? '' : selectedSku.current}
                                                onChange={(e) => selectSku(e.target.value)}>
                                            {
                                                (availableSkus!).map((sku: AvailableSkuDto) => {
                                                    return (
                                                        <option key={sku.sku} value={sku.sku}>
                                                            {`${sku.sku}\t(${sku.distributorName}) - ${sku.description}`}
                                                        </option>
                                                    );
                                                })
                                            }
                                        </select>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="onHand" className="col-form-label">Quantity on hand:</label>
                                        <input type="number" id="onHand" className="form-control onhand-control"
                                               required
                                               value={onHand}
                                               onChange={e => setOnHandValue(e.target.value)}
                                        />
                                    </div>
                                </div>
                                <div className="row mmc-form-button-row">
                                    <div className='modal-navigation-row justify-content-end'>
                                        <button className="btn btn-outline-primary" type="submit">Save inventory
                                        </button>
                                        &nbsp;
                                        <button className="btn btn-outline-secondary"
                                                type="button"
                                                onClick={closeInventoryModal}>Cancel
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </Modal>
                    </div>
                </div>
            }
        </div>
    );
}
