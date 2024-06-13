import React, { Ref, useEffect, useImperativeHandle, useRef, useState } from 'react';
import { useCapContext } from '../../context/CapContext';
import { Cap } from '../../model/Cap';
import { Position } from '../../model/Position';
import { Thread } from '../../model/Thread';
import { JobImage } from '../../model/JobImage';
import { JobPosition } from '../../model/JobPosition';
import { JobThread } from '../../model/JobThread';
import { SearchSelect, SearchSelectable } from '../../components/control/SearchSelect';
import { FileSelector } from '../../components/control/FileSelector';
import { User } from '../../security/auth/User';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';

export interface ImagePlacementProps {
    capModelId: number;
    imageId: number;
    isFirst: boolean;
    addImageRow: () => void;
    setImageToRemove: (key: string) => void;
    capId: string;
    authenticatedUser: AuthenticatedUser;
    values: JobImage | null;
}

export class ImagePlacementContentHandle {
    getImage: () => JobImage | null;

    constructor(getImage: () => JobImage | null) {
        this.getImage = getImage;
    }
}

function ImagePlacement(props: ImagePlacementProps, ref: Ref<ImagePlacementContentHandle>) {

    const capContextData = useCapContext();

    const [imageFilename, setImageFilename] = useState<string | null>(null);
    //const imagePathname= useRef<string | null>();
    const [imagePathname, setImagePathname] = useState<string | null>(null);
    // maybe later we can use https://developer.mozilla.org/en-US/docs/Web/API/Window/showDirectoryPicker  - supported in Chromium BUT NOT Safari currently

    const [stitchCount, setStitchCount] = useState<number | undefined>(undefined);
    const [type, setType] = useState<string>("");
    const [position1, setPosition1] = useState<string | null>();
    const [position2, setPosition2] = useState<string | null>();
    const [thread1, setThread1] = useState<string | null>(null);
    const [thread2, setThread2] = useState<string | null>(null);
    const [thread3, setThread3] = useState<string | null>(null);
    const [thread4, setThread4] = useState<string | null>(null);
    const [thread5, setThread5] = useState<string | null>(null);

    const [cap, setCap] = useState<Cap | null>(null);
    const [threads, setThreads] = useState<Thread[] | null>(null);

    const [forceRefresh, setForceRefresh] = useState<number | null>(null);


    useImperativeHandle(ref, () => ({
        getImage
    }));

    function getImage(): JobImage | null {

        if (!imageFilename || imageFilename == '' || !imagePathname || imagePathname == '') {
            return null;
        }

        const positions: JobPosition[] = [];
        if (position1) {
            positions.push(new JobPosition(position1));
        }
        if (position2) {
            positions.push(new JobPosition(position2));
        }
        if (positions.length == 0) {
            return null;
        }

        const threads: JobThread[] = []
        if (thread1) {
            threads.push(new JobThread(thread1));
        }
        if (thread2) {
            threads.push(new JobThread(thread2));
        }
        if (thread3) {
            threads.push(new JobThread(thread3));
        }
        if (thread4) {
            threads.push(new JobThread(thread4));
        }
        if (thread5) {
            threads.push(new JobThread(thread5));
        }

        const jobImage = new JobImage(imagePathname, imageFilename, type, stitchCount == null ? null : stitchCount);
        jobImage.positions.push(...positions);
        jobImage.threads.push(...threads);
        return jobImage;
    }

    function getPositionsForCap(): Position[] {
        if (cap) {
            return cap.positions;
        }
        return [];
    }

    function getThreads(): Thread[] {
        if (threads) {
            return threads;
        }
        return [];
    }

    function onImageFilenameChange(filename: string) {
        setImageFilename(filename);
    }

    function onImagePathnameChange(e: any) {
        setImagePathname(e.target.value);
    }

    function getImagePathname(): string {
        return imagePathname == null ? '' : imagePathname;
    }

    function addNewImage(e: any) {
        e.preventDefault();
        props.addImageRow();
    }

    function removeImage(e: any) {
        e.preventDefault();
        const key = e.currentTarget.getAttribute("data-value");
        props.setImageToRemove(key);
    }

    function onStitchCountChange(e: any) {
        setStitchCount(e.target.value);
    }

    function getStitchCount() {
        return stitchCount;
    }

    function onTypeChange(event: any) {
        setType(event.target.value);
    }

    function onPositionChange(event: any, index: number) {
        const value = event.target.value;
        switch (index) {
            case 1:
                setPosition1(value);
                break;
            case 2:
                setPosition2(value);
                break;
        }
    }

    function getPosition(index: number) {
        switch (index) {
            case 1:
                return position1 == null ? '' : position1;

            case 2:
                return position2 == null ? '' : position2;

        }
        return '';
    }

    function getThreadSelectableItems(): SearchSelectable[] {
        const items: SearchSelectable[] = [];
        getThreads().map(thread => {
            items.push(new SearchSelectable(thread.code.toString(), `${thread.code} - ${thread.colorName}`));
        });
        return items;
    }

    function onThread1Change(value: SearchSelectable | null) {
        setThread1(value ? value.value : null);
    }

    function onThread2Change(value: SearchSelectable | null) {
        setThread2(value ? value.value : null);
    }

    function onThread3Change(value: SearchSelectable | null) {
        setThread3(value ? value.value : null);
    }

    function onThread4Change(value: SearchSelectable | null) {
        setThread4(value ? value.value : null);
    }

    function onThread5Change(value: SearchSelectable | null) {
        setThread5(value ? value.value : null);
    }

    function setThreadValueFor(index: number, value: string) {
        switch (index) {
            case 1:
                setThread1(value);
                break;

            case 2:
                setThread2(value);
                break;

            case 3:
                setThread3(value);
                break;

            case 4:
                setThread4(value);
                break;

            case 5:
                setThread5(value);
                break;

        }
    }

    function getKeyPrefix(): string {
        return `capmodel-${props.capModelId}-image-${props.imageId}`;
    }

    useEffect(() => {
        const capLookup = capContextData.getCapLookup();
        if (capLookup) {
            const cap = capLookup.getCap(props.capId);
            if (cap) {
                setCap(cap);
            }
        }

        const threadLookup = capContextData.getThreadLookup();
        if (threadLookup) {
            setThreads(threadLookup);
        }

        let gotPosition1 = false;
        if (props.values != null) {
            setImageFilename(props.values.filename);
            setImagePathname(props.values.path);

            let index = 1;
            props.values.threads.forEach(thread => {
                if (thread != null && thread.color != null) {
                    setThreadValueFor(index++, thread.color);
                }
            })

            if (props.values.positions[0] != null) {
                setPosition1(props.values.positions[0].code);
                gotPosition1 = true;
            }
            if (props.values.positions[1] != null) {
                setPosition2(props.values.positions[1].code);
            }

            if (props.values.stitchCount != null) {
                setStitchCount(props.values.stitchCount);
            }

            setForceRefresh(1);
        } else if (props.authenticatedUser.artifactPath != null) {
            setImagePathname(props.authenticatedUser.artifactPath);
        }

        if (!gotPosition1) {
            setPosition1("FrontCenter");
        }
    }, []);

    useEffect(() => {
        // force a refresh following pushed values
    }, [forceRefresh]);

    return (
        <div>
            <div className='row ca-form-row'>
                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-selector`} className="col-form-label">Select image:</label>
                    <FileSelector id={`${getKeyPrefix()}-selector`}
                                  externallySelectedValue={imageFilename}
                                  onChange={onImageFilenameChange} />
                </div>
                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-path`} className="col-form-label">Path to image file:</label>
                    <input type="text" id={`${getKeyPrefix()}-path`} className="form-control" name="serial" required
                           onChange={onImagePathnameChange} value={getImagePathname()} />
                </div>
            </div>

            <div className='row ca-form-row'>
                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-type`} className="col-form-label">Type:</label>
                    <select id={`${getKeyPrefix()}-type`} className="form-select" onChange={onTypeChange}>
                        <option key={`${getKeyPrefix()}-flat`} value="FlatEmbroidered" defaultValue="FlatEmbroidered">Flat embroidered</option>
                        <option key={`${getKeyPrefix()}-3d`} value="3DEmbroidered">3D embroidered</option>
                        <option key={`${getKeyPrefix()}-patch`} value="Patch">Patch</option>
                    </select>
                </div>

                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-thread1`} className="col-form-label">Thread color:</label>
                    <SearchSelect id={`${getKeyPrefix()}-thread1`} onChange={onThread1Change} promptText="Select a color"
                                  selectableItems={getThreadSelectableItems()}
                                  externallySelectedValue={thread1} />
                </div>
            </div>

            <div className='row ca-form-row'>
                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-position1`} className="col-form-label">Position:</label>
                    <select id={`${getKeyPrefix()}-position1`} className="form-select" required
                            value={getPosition(1)}
                            onChange={(event) => onPositionChange(event, 1)}>
                        {getPositionsForCap().map(position => (
                            <option key={position.code} value={position.code}>
                                {position.code}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-thread2`} className="col-form-label">Thread color:</label>
                    <SearchSelect id={`${getKeyPrefix()}-thread2`} onChange={onThread2Change} promptText="Select a color"
                                  selectableItems={getThreadSelectableItems()}
                                  externallySelectedValue={thread2} />
                </div>
            </div>

            <div className='row ca-form-row'>
                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-position2`} className="col-form-label">Position:</label>
                    <select id={`${getKeyPrefix()}-position2`} className="form-select"
                            value={getPosition(2)}
                            onChange={(event) => onPositionChange(event, 2)}>
                        <option value="" defaultValue="">Select a position</option>
                        {getPositionsForCap().map(position => (
                            <option key={position.code} value={position.code}>
                                {position.code}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-thread3`} className="col-form-label">Thread color:</label>
                    <SearchSelect id={`${getKeyPrefix()}-thread3`} onChange={onThread3Change} promptText="Select a color"
                                  selectableItems={getThreadSelectableItems()}
                                  externallySelectedValue={thread3} />
                </div>
            </div>

            <div className='row ca-form-row'>
                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-stitches`} className="col-form-label"># stitches:</label>
                    <input id={`${getKeyPrefix()}-stitches`} className="form-control" type="number"
                           value={getStitchCount()}
                           onChange={onStitchCountChange}/>
                </div>

                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-thread4`} className="col-form-label">Thread color:</label>
                    <SearchSelect id={`${getKeyPrefix()}-thread4`} onChange={onThread4Change} promptText="Select a color"
                                  selectableItems={getThreadSelectableItems()}
                                  externallySelectedValue={thread4} />
                </div>
            </div>

            <div className='row ca-form-row'>
                <div className="col-sm-4 pl-2 ca-form-start-row-buttons ca-red-background">
                    <br />
                    <div className="ca-button-box">
                        <button id={`add-${getKeyPrefix()}`}
                                className='btn btn-outline-primary ca-form-button' onClick={addNewImage}>Add another image
                        </button>
                        {
                            !props.isFirst &&
                            <button id={`remove-${getKeyPrefix()}`}
                                className='btn btn-outline-danger ca-form-next-button' data-value={`${props.capModelId}-image-${props.imageId}`} onClick={removeImage}>Remove image
                            </button>
                        }
                    </div>
                </div>

                <div className="col-sm-4 ca-red-background">
                    <label htmlFor={`${getKeyPrefix()}-thread5`} className="col-form-label">Thread color:</label>
                    <SearchSelect id={`${getKeyPrefix()}-thread5`} onChange={onThread5Change} promptText="Select a color"
                                  selectableItems={getThreadSelectableItems()}
                                  externallySelectedValue={thread5} />
                </div>
            </div>
        </div>
    )
}

export default React.forwardRef<ImagePlacementContentHandle,ImagePlacementProps>(ImagePlacement);
