import React, { MutableRefObject, useEffect, useRef, useState } from "react";
import { CapApi } from '../../api/CapApi';
import CapModel, { CapModelContentHandle, CapModelProps } from './CapModel';
import { CapContext } from '../../context/CapContext';
import { CapLookup } from '../../lookup/CapLookup';
import { useSharedContext } from '../../context/SharedContext';
import { CapContextData } from '../../context/CapContextData';
import { Thread } from '../../model/Thread';
import { ThreadApi } from '../../api/ThreadApi';
import { JobRequest } from '../../model/JobRequest';
import { CapModelContentRef } from './TargetRef';
import { JobApi } from '../../api/JobApi';
import { useNavigate } from 'react-router-dom';
import { Alerter } from '../../layout/Alerter';
import { Modal } from 'react-responsive-modal';
import { JobCap } from '../../model/JobCap';


const capContextData = new CapContextData();


export default function RequestNewJob()  {

    const sharedContextData = useSharedContext();

    const [nextCapModelId, setNextCapModelId] = useState(1);
    const [capLookup, setCapLookup] = useState<CapLookup | undefined>(undefined);
    const [threadLookup, setThreadLookup] = useState<Thread[] | undefined>(undefined);

    const [customerName, setCustomerName] = useState<string>();
    const [startSequence, setStartSequence] = useState<number>()

    const capComponentRefs = useRef<MutableRefObject<CapModelContentHandle | null>[]>([]);
    const [capKeyToRemove, setCapKeyToRemove] = useState<number>(-1);
    const capComponents = useRef<JSX.Element[]>([]);
    const [capComponentsChanged, setCapComponentsChanged] = useState<number>(0);

    const [cloneSource, setCloneSource] = useState<JobCap | undefined>()
    const [cloneCapId, setCloneCapId] = useState<string>('');
    const [clonerOpen, setClonerOpen] = useState<boolean>(false);

    const navigate = useNavigate();

    function getComponentRef(): MutableRefObject<CapModelContentHandle | null> {
        const ref = new CapModelContentRef();
        capComponentRefs.current.push(ref);
        return ref;
    }

    function persistNewCapComponent(component: JSX.Element) {
        capComponents.current.push(component);
        setCapComponentsChanged(capComponents.current.length);
    }

    function unloadCapComponentAt(index: number) {
        capComponents.current.splice(index, 1);
        capComponentRefs.current.splice(index, 1);
        setCapComponentsChanged(capComponents.current.length);
    }

    function cloneCapHandler(sourceJobCap: JobCap) {
        setCloneSource(sourceJobCap);
        setCloneCapId('');
        setClonerOpen(true);
    }

    function closeCloneRequestModal() {
        setClonerOpen(false);
    }

    function onCloneCapChange(event: any) {
        const value = event.target.value;
        setCloneCapId(value ? value : '');
    }

    function submitCloneRequest(event: any) {
        event.preventDefault()

        if (cloneCapId == '' || cloneSource == null) {
            Alerter.showWarning('You must select a cap model to use for cloned copy', Alerter.DEFAULT_TIMEOUT);
            return;
        }

        const cap = getCapLookup().getCapsList().filter(cap => cap.id == cloneCapId);
        const clone = cloneSource.cloneFor(cap[0].id, cap[0].name);
        if (clone.images.length == 0) {
            Alerter.showWarning('You must select a file and path for the image before you can clone a copy', Alerter.DEFAULT_TIMEOUT);
            return;
        }

        createNewCap(clone);

        closeCloneRequestModal();
    }

    function getCapLookup(): CapLookup {
        const capLookup = capContextData.getCapLookup();
        if (capLookup) {
            return capLookup;
        }
        return new CapLookup([]);
    }

    function addNewCap(e: any) {
        e.preventDefault();
        createNewCap();
    }

    function createNewCap(values: JobCap | null = null): number {
        const nextId = nextCapModelId;
        setNextCapModelId(() => nextId + 1);
        const ref = getComponentRef();
        persistNewCapComponent(<CapModel
            capModelId={nextId}
            key={nextId}
            isFirst={false}
            setKeyToRemove={setCapKeyToRemove}
            ref={ref}
            cloneCapHandler={cloneCapHandler}
            authenticatedUser={sharedContextData.getAuthenticatedUser()!}
            values={values} />);
        return nextId;
    }

    const loadCapLookup = async () => {
        if (!capContextData.getCapLookup()) {
            const authenticatedUser = sharedContextData.getAuthenticatedUser();
            if (authenticatedUser) {
                setStartSequence(authenticatedUser.nextSequence);
                const lookup = await CapApi.loadCapLookup(authenticatedUser);
                if (lookup) {
                    capContextData.setCapLookup(lookup);
                    setCapLookup(lookup);
                }
            }
        } else {
            setCapLookup(capContextData.getCapLookup() as CapLookup);
        }
    }

    const loadThreadLookup = async () => {
        if (!capContextData.getThreadLookup()) {
            const authenticatedUser = sharedContextData.getAuthenticatedUser();
            if (authenticatedUser) {
                setStartSequence(authenticatedUser.nextSequence);
                const threads = await ThreadApi.loadThreadsLookup(authenticatedUser);
                if (threads) {
                    capContextData.setThreadLookup(threads);
                    setThreadLookup(threads);
                }
            }
        } else {
            setThreadLookup(capContextData.getThreadLookup() as Thread[]);
        }
    }

    const saveJobRequest = async (request: JobRequest): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            const response = await JobApi.createNewJob(request, authenticatedUser);
            let totalSequences = 0;
            request.caps.forEach(cap => totalSequences += cap.styles.length)
            sharedContextData.updateNextSequence(request.serial + totalSequences);      // mirror changes done on server!
            return response;
        }
        console.log("Unable to find authenticated user to save new job!");
        return false;
    }

    function submitJob(e: any) {
        e.preventDefault();

        const authenticatedUser = sharedContextData.getAuthenticatedUser();

        const request = new JobRequest(customerName!, startSequence!, authenticatedUser!.extractUser());

        capComponentRefs.current.map((ref) => {
            if (ref.current) {
                request.caps.push(ref.current.getJobCap());
            }
        });

        saveJobRequest(request).then(response => {
            if (response) {
                Alerter.showSuccess("Saving new job was successful!", Alerter.DEFAULT_TIMEOUT)
                navigate('/MyJobs');
            } else {
                Alerter.showError("Saving new job failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
            }
        });
    }

    useEffect(() => {
        // This is the correct/acceptable way to initialize a ref
        const ref = getComponentRef();

        if (sharedContextData.getAuthenticatedUser()) {
            setStartSequence(sharedContextData.getAuthenticatedUser()?.nextSequence);
        }

        let isFirst = true;
        if (sharedContextData.getJobRequest() != null) {
            const jobRequest = sharedContextData.getJobRequest();
            sharedContextData.clearJobRequest();

            setCustomerName(jobRequest?.customer);

            let nextId = nextCapModelId;
            jobRequest?.caps.forEach(cap => {
                persistNewCapComponent(<CapModel capModelId={cap.capModelId}
                                                 key={cap.capModelId}
                                                 isFirst={isFirst}
                                                 setKeyToRemove={setCapKeyToRemove}
                                                 ref={ref}
                                                 authenticatedUser={sharedContextData.getAuthenticatedUser()!}
                                                 cloneCapHandler={cloneCapHandler} values={cap}/>);
                isFirst = false;
                if (cap.capModelId >= nextId) {
                    nextId = cap.capModelId + 1;
                }
            })

            if (!isFirst) {
                setNextCapModelId(() => nextId);
            }
        }

        if (isFirst) {
            persistNewCapComponent(<CapModel capModelId={0}
                                             key={0}
                                             isFirst={true}
                                             setKeyToRemove={setCapKeyToRemove}
                                             ref={ref}
                                             authenticatedUser={sharedContextData.getAuthenticatedUser()!}
                                             cloneCapHandler={cloneCapHandler} values={null}/>);
        }

        loadCapLookup().catch(console.error);
        loadThreadLookup().catch(console.error);
    }, []);


    useEffect(() => {
        if (capKeyToRemove) {
            let capIndex = -1;
            for (let index = 0; index < capComponents.current.length; index++) {
                const element = capComponents.current[index];
                if (element.key == capKeyToRemove.toString()) {
                    capIndex = index;
                    break;
                }
            }
            if (capIndex >= 0) {
                unloadCapComponentAt(capIndex);
            }
        }
    }, [capKeyToRemove])


    return (
        <div>
            <h2><span className="ca-red">Request a</span> <span className="ca-blue">New Job</span></h2>
            {!capLookup && !threadLookup && <div className="ca-spinner-box spinner-border text-primary"><span className="sr-only"></span></div>}
            {capLookup && threadLookup &&
                <CapContext.Provider value={capContextData}>
                    <form onSubmit={submitJob}>
                        <div className='row ca-form-row'>
                            <div className="col-sm-4">
                                <label htmlFor="customer" className="col-form-label">Customer:</label>
                                <input type="text" id="customer" className="form-control" name="customer"
                                       required={true}
                                       placeholder="Enter customer name"
                                       value={customerName}
                                       onChange={e => setCustomerName(e.target.value)}/>
                            </div>
                            <div className="col-sm-2">
                                <label htmlFor="serial" className="col-form-label">Serial #:</label>
                                <input type="number" id="serial" className="form-control" name="serial" required={true}
                                       value={startSequence == null ? '1' : startSequence.toString()}
                                       onChange={e => setStartSequence(parseInt(e.target.value))}/>
                            </div>
                        </div>

                        {
                            capComponents.current.map(component => component)
                        }

                        <div className="row">
                            <div className='row col-2 p-3'>
                                <button className="btn btn-outline-primary" type="button" onClick={addNewCap}>Add
                                    another cap
                                </button>
                            </div>
                        </div>

                        <div className="row">
                            <div className='row col-2 p-3'>
                                <button className="btn btn-outline-secondary" type="submit">Submit job</button>
                            </div>
                        </div>

                        <div>
                            <Modal open={clonerOpen}
                                   onClose={closeCloneRequestModal}
                                   closeOnEsc={false}
                                   closeOnOverlayClick={false}
                                   showCloseIcon={false}
                                   classNames={{
                                       overlay: 'customOverlay',
                                       modal: 'customCloneRequestModal',
                                   }}>

                                <form>
                                    <div><h2>Clone Cap Request</h2></div>
                                    <div className='row ca-form-row'>
                                        <div>This option permits you to create a new cap entry based upon another.</div>
                                    </div>

                                    <div className='row ca-form-row'>
                                        <div className="">
                                            <label htmlFor={'capmodel-clone'}
                                                   className="col-form-label">New Cap model:</label>
                                            <select id={'capmodel-clone'}
                                                    className="form-select"
                                                    onChange={e => onCloneCapChange(e)}
                                                    value={cloneCapId}
                                                    required>
                                                <option key="-1" defaultValue="">Select a model</option>
                                                {getCapLookup().getCapsList().map(cap => (
                                                    <option key={cap.id} value={cap.id}>
                                                        {cap.name}
                                                    </option>
                                                ))}
                                            </select>
                                        </div>
                                    </div>

                                    <div className="row ca-form-button-row">
                                        <div className='modal-navigation-row justify-content-end'>
                                            <button className="btn btn-outline-primary"
                                                    type="button"
                                                    onClick={submitCloneRequest}>Clone Cap!
                                            </button>
                                            &nbsp;
                                            <button className="btn btn-outline-secondary"
                                                    type="button"
                                                    onClick={closeCloneRequestModal}>Cancel
                                            </button>
                                        </div>
                                    </div>
                                </form>
                            </Modal>
                        </div>
                    </form>
                </CapContext.Provider>
            }
        </div>
    );
}

