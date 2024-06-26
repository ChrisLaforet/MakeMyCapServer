import { useEffect, useRef, useState } from 'react';
import { DistributorSkusDto } from '../../api/dto/DistributorSkusDto';
import { useSharedContext } from '../../context/SharedContext';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';
import { ShopifySupportApi } from '../../api/ShopifySupportApi';
import { DistributorDto } from '../../api/dto/DistributorDto';
import { Modal } from 'react-responsive-modal';
import { SkuDto } from '../../api/dto/SkuDto';
import "./Shopify.css";

export default function SkuMappings() {

    const sharedContextData = useSharedContext();

    const distributors = useRef<DistributorDto[] | null>(null);
    const selectedDistributor = useRef<DistributorDto | null>(null);
    const [distributorSkus, setDistributorSkus] = useState<DistributorSkusDto[]>([]);

    const [createSkuOpen, setCreateSkuOpen] = useState<boolean>(false);
    const [editSkuOpen, setEditSkuOpen] = useState<boolean>(false);
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

    function loadDistributorSkus(user: AuthenticatedUser) {
        if (distributors.current == null) {
            return;
        }
        distributors.current.forEach(distributor => {
            loadDistributorSkusFor(distributor.code, user).then(() => {
                if (selectedDistributor.current == null) {
                    selectedDistributor.current = distributor;
                }
                selectedDistributor.current = distributors.current![0];
                setValueChanged(valueChanged + 1);
            })
            .catch(() => {
                console.log(`Error loading distributor skus for ${ distributor.code }`)
            });
        });
    }

    const loadDistributorSkusFor = async (distributorCode: string, user: AuthenticatedUser) => {
        const lookup = await ShopifySupportApi.getSkusForDistributor(distributorCode, user);
        if (lookup) {
            distributorSkus.push(lookup);
            console.log("Got distributor skus for " + distributorCode);
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
                return selection.skus;
            }
        }
        return [];
    }


    function openCreateSkuModal() {
        setMmcSku("");
        setDistributorSku("");
        setBrand("");
        setStyleCode("")
        setPartId("");
        setColor("")
        setColorCode("");
        setSizeCode("");
        setEditSkuOpen(false);
        setCreateSkuOpen(true);
    }

    function closeCreateSkuModal() {
        setCreateSkuOpen(false);
        setEditSkuOpen(false);
    }

    function openEditSkuModal(sku: SkuDto) {
        setCreateSkuOpen(false);
        setEditSkuOpen(true);
    }

    function submitCreateSku() {

    }

    function requestDeleteSkuModal(sku: SkuDto) {

    }

    useEffect(() => {

    }, [valueChanged]);

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            loadDistributors(user)
                .then(() => {
                    loadDistributorSkus(user);
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
                        <Modal open={createSkuOpen}
                               onClose={closeCreateSkuModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'customSkuModal',
                               }}>
                            <form onSubmit={submitCreateSku}>
                                <div><h2>{`Create Sku for ${selectedDistributor.current!.name}`}</h2></div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="mmcSku" className="col-form-label">MMC Sku:</label>
                                        <input type="text" id="mmcSku" className="form-control"
                                               required={true}
                                               value={mmcSku}
                                               maxLength={20}
                                               onChange={e => setMmcSku(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="distributorSku" className="col-form-label">Distributor's Sku:</label>
                                        <input type="text" id="distributorSku" className="form-control"
                                               required={true}
                                               value={distributorSku}
                                               maxLength={255}
                                               onChange={e => setDistributorSku(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="brand" className="col-form-label">Brand:</label>
                                        <input type="text" id="brand" className="form-control"
                                               required={true}
                                               value={brand}
                                               maxLength={255}
                                               onChange={e => setBrand(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="styleCode" className="col-form-label">Style Code:</label>
                                        <input type="text" id="styleCode" className="form-control"
                                               required={true}
                                               value={styleCode}
                                               maxLength={255}
                                               onChange={e => setStyleCode(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="partId" className="col-form-label">Part Id:</label>
                                        <input type="text" id="partId" className="form-control"
                                               required={true}
                                               value={partId}
                                               maxLength={255}
                                               onChange={e => setPartId(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="color" className="col-form-label">Color (name):</label>
                                        <input type="text" id="color" className="form-control"
                                               required={true}
                                               value={color}
                                               maxLength={255}
                                               onChange={e => setColor(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="colorCode" className="col-form-label">Color code:</label>
                                        <input type="text" id="colorCode" className="form-control"
                                               required={true}
                                               value={colorCode}
                                               maxLength={255}
                                               onChange={e => setColorCode(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="sizeCode" className="col-form-label">Size code:</label>
                                        <input type="text" id="sizeCode" className="form-control"
                                               required={true}
                                               value={sizeCode}
                                               maxLength={255}
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
                </div>
            }
        </div>
    )
        ;
}
