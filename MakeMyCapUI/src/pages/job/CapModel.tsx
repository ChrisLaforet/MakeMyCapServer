import React, { MutableRefObject, Ref, useEffect, useImperativeHandle, useRef, useState } from 'react';
import CapStyle, { CapStyleContentHandle } from './CapStyle';
import { CapLookup } from '../../lookup/CapLookup';
import { useCapContext } from '../../context/CapContext';
import ImagePlacement, { ImagePlacementContentHandle } from './ImagePlacement';
import { JobCap } from '../../model/JobCap';
import { CapStyleContentRef, ImagePlacementContentRef } from './TargetRef';
import { JobImage } from '../../model/JobImage';
import { JobStyle } from '../../model/JobStyle';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';


export interface CapModelProps {
    capModelId: number;
    isFirst: boolean;
    setKeyToRemove: (key: number) => void;
    cloneCapHandler: (jobCap: JobCap) => void;
    authenticatedUser: AuthenticatedUser;
    values: JobCap | null;
}

export class CapModelContentHandle {
    getJobCap: () => JobCap;

    constructor(getJobCap: () => JobCap) {
        this.getJobCap = getJobCap;
    }
}

function CapModel(props: CapModelProps, ref: Ref<CapModelContentHandle>) {

    const capContextData = useCapContext();

    const capId= useRef<string>("");
    const [selectedCapId, setSelectedCapId] = useState("");

    const nextStyleId= useRef<number>(1);
    const nextImageId= useRef<number>(1);

    const [styleKeyToRemove, setStyleKeyToRemove] = useState<string | undefined>();
    const styleComponentRefs = useRef<MutableRefObject<CapStyleContentHandle | null>[]>([]);
    const styleComponents = useRef<JSX.Element[]>([])
    const [styleComponentsChanged, setStyleComponentsChanged] = useState<number>(0);

    const [imageKeyToRemove, setImageKeyToRemove] = useState<string | undefined>();
    const imageComponentRefs = useRef<MutableRefObject<ImagePlacementContentHandle | null>[]>([]);
    const imageComponents = useRef<JSX.Element[]>([])
    const [imageComponentsChanged, setImageComponentsChanged] = useState<number>(0);

    function getStyleComponentRef(): MutableRefObject<CapStyleContentHandle | null> {
        const ref = new CapStyleContentRef();
        styleComponentRefs.current.push(ref);
        return ref;
    }

    function persistNewStyleComponent(component: JSX.Element) {
        styleComponents.current.push(component);
        setStyleComponentsChanged(styleComponents.current.length);
    }

    function unloadStyleComponentAt(index: number) {
        styleComponents.current.splice(index, 1);
        styleComponentRefs.current.splice(index, 1);
        setStyleComponentsChanged(styleComponents.current.length);
    }

    function clearStyleComponents() {
        styleComponents.current.splice(0, styleComponents.current.length);
        styleComponentRefs.current.splice(0, styleComponentRefs.current.length);
        setStyleComponentsChanged(styleComponents.current.length);
    }

    function getImagePlacementComponentRef(): MutableRefObject<ImagePlacementContentHandle | null> {
        const ref = new ImagePlacementContentRef();
        imageComponentRefs.current.push(ref);
        return ref;
    }

    function persistNewImagePlacementComponent(component: JSX.Element) {
        imageComponents.current.push(component);
        setImageComponentsChanged(imageComponents.current.length);
    }

    function unloadImagePlacementComponentAt(index: number) {
        imageComponents.current.splice(index, 1);
        imageComponentRefs.current.splice(index, 1);
        setImageComponentsChanged(imageComponents.current.length);
    }

    function clearImagePlacementComponents() {
        imageComponents.current.splice(0, imageComponents.current.length);
        imageComponentRefs.current.splice(0, imageComponentRefs.current.length);
        setImageComponentsChanged(imageComponents.current.length);
    }

    useImperativeHandle(ref, () => ({
        getJobCap
    }));

    function getJobCap(): JobCap {
        const capLookup = getCapLookup();
        const cap = capLookup.getCap(capId.current);
        const jobCap = new JobCap(props.capModelId, cap!);

        // for each style prepare the styles
        styleComponentRefs.current.map((ref) => {
            if (ref.current) {
                const jobStyle = ref.current.getJobStyle();
                if (jobStyle) {
                    jobCap.styles.push(jobStyle);
                }
            }
        });

        // for each image prepare the images
        imageComponentRefs.current.map((ref) => {
            if (ref.current) {
                const image = ref.current.getImage();
                if (image) {
                    jobCap.images.push(image);
                }
            }
        });

        return jobCap;
    }

    function getCapLookup(): CapLookup {
        const capLookup = capContextData.getCapLookup();
        if (capLookup) {
            return capLookup;
        }
        return new CapLookup([]);
    }

    function isCapModelSelected(): boolean {
        return capId.current != "";
    }

    function isRemovable(): boolean {
        return !props.isFirst;
    }

    function removeCapModel(e: any) {
        e.preventDefault();

        if (!props.isFirst) {
            props.setKeyToRemove(props.capModelId);
        }
    }

    function cloneCapModel(e: any) {
        e.preventDefault();
        props.cloneCapHandler(getJobCap());
    }

    function onCapChange(event: any) {
        const value = event.target.value;
        capId.current = value ? value : "";

        clearStyleComponents();
        addStyleRow();

        if (!capId.current || capId.current == "") {
            // remove all images - nothing to attach them to!
            clearImagePlacementComponents();
        } else if (imageComponents.current.length == 0) {
            // since no image exists yet, let's add one
            addImageRow();
        }
        setSelectedCapId(value);
    }

    function getExtraStyleComponents(): JSX.Element[] {
        const components = [...styleComponents.current];
        components.splice(0, 1);
        return components;
    }

    function getImageComponents(): JSX.Element[] {
        return imageComponents.current;
    }

    function addStyleRow(values: JobStyle | null = null): void {
        const currentStyleId= nextStyleId.current;
        nextStyleId.current++;
        const ref = getStyleComponentRef();
        persistNewStyleComponent(<CapStyle key={`${props.capModelId}-style-${currentStyleId}`}
                                           capModelId={props.capModelId}
                                           capStyleId={currentStyleId} capId={capId.current}
                                           isFirst={styleComponents.current.length < 1}
                                           addStyleRow={addStyleRow}
                                           setStyleToRemove={setStyleKeyToRemove}
                                           values={values}
                                           ref={ref}/>);
    }

    function addImageRow(values: JobImage | null = null) {
        const currentImageId = nextImageId.current;
        nextImageId.current++;

        const ref = getImagePlacementComponentRef();
        persistNewImagePlacementComponent(<ImagePlacement key={`${props.capModelId}-image-${currentImageId}`}
                                                          capModelId={props.capModelId} imageId={currentImageId}
                                                          capId={capId.current}
                                                          isFirst={imageComponents.current.length < 1}
                                                          addImageRow={addImageRow}
                                                          setImageToRemove={setImageKeyToRemove}
                                                          authenticatedUser={props.authenticatedUser}
                                                          values={values}
                                                          ref={ref}/>);
    }

    useEffect(() => {
        // This is the correct/acceptable way to initialize a ref
        const ref = getStyleComponentRef();

// TODO: CML - for cloning a complete former job, the styles need to be created on the fly from the props.values
        // persistNewStyleComponent(<CapStyle key={`${props.capModelId}-style-0`}
        //                                    capModelId={props.capModelId}
        //                                    capStyleId={0}
        //                                    capId={""}
        //                                    isFirst={true}
        //                                    addStyleRow={addStyleRow}
        //                                    setStyleToRemove={setStyleKeyToRemove}
        //                                    values={null}
        //                                    ref={ref} />);

        if (props.values) {
            console.log(props.values)
            // was this a clone or pre-existing value?
            setSelectedCapId(props.values.cap.id);
            capId.current = props.values.cap.id;
            props.values.styles.forEach(style => {
                addStyleRow(style);
            });
            props.values.images.forEach(image => {
               addImageRow(image);
            });

            if (imageComponents.current.length == 0) {
                // if no image exists yet, let's add one
                addImageRow();
            }
        }

        if (styleComponents.current.length == 0) {
            addStyleRow()
        }

    }, []);


    useEffect(() => {
        if (styleKeyToRemove) {
            let styleIndex = -1;
            for (let index = 0; index < styleComponents.current.length; index++) {
                const element = styleComponents.current[index];
                if (element.key == styleKeyToRemove) {
                    styleIndex = index;
                    break;
                }
            }
            if (styleIndex >= 0) {
                unloadStyleComponentAt(styleIndex);
            }
        }
    }, [styleKeyToRemove]);

    useEffect(() => {
        if (imageKeyToRemove) {
            let imageIndex = -1;
            for (let index = 0; index < imageComponents.current.length; index++) {
                const element = imageComponents.current[index];
                if (element.key == imageKeyToRemove) {
                    imageIndex = index;
                    break;
                }
            }
            if (imageIndex >= 0) {
                unloadImagePlacementComponentAt(imageIndex);
            }
        }
    }, [imageKeyToRemove]);


    return (
        <div className="align-items-center">
            <div className='row ca-form-row'>
                <div className="col-sm-4 ca-blue-background">
                    <label htmlFor={`capmodel-${props.capModelId}`} className="col-form-label">Cap model:</label>
                    <select id={`capmodel-${props.capModelId}`} className="form-select" onChange={onCapChange} value={selectedCapId} required>
                        <option key="-1" value="" defaultValue="">Select a model</option>
                        {getCapLookup().getCapsList().map(cap => (
                            <option key={cap.id} value={cap.id}>
                                {cap.name}
                            </option>
                        ))}
                    </select>
                </div>
                {
                    styleComponents.current[0]
                }
            </div>

            {
               (styleComponents.current.length > 1) &&
               getExtraStyleComponents().map((styleComponent) =>
                    <div key={`${styleComponent.key}-div`} className='row ca-form-row'>
                        <div className="col-sm-4 ca-blue-background">
                        </div>
                        {styleComponent}
                    </div>
                )
            }

            {
                getImageComponents().map((imageComponent) => imageComponent)
            }

            {
                isCapModelSelected() &&
                <div className="row">
                    <div className='row col-sm-4 pl-2 ca-form-start-row-buttons ca-form-top-space'>
                        <span>
                            <button className="btn btn-outline-info" type="button" onClick={cloneCapModel}>Clone this cap</button>
                            &nbsp;
                            {
                                isRemovable() &&
                                <button className="btn btn-outline-danger" type="button" onClick={removeCapModel}>Remove this cap</button>
                            }
                        </span>
                    </div>
            </div>
            }

            <div className='row ca-form-row'>
                <div className="col-sm-8">
                    <hr />
                </div>
            </div>


        </div>
    );
}


export default React.forwardRef<CapModelContentHandle, CapModelProps>(CapModel);
