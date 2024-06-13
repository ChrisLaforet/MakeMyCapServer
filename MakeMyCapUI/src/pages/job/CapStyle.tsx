import React, { Ref, useEffect, useImperativeHandle, useState } from 'react';
import {  useCapContext } from '../../context/CapContext';
import { Style } from '../../model/Style';
import { Cap } from '../../model/Cap';
import { JobStyle } from '../../model/JobStyle';


export interface CapStyleProps {
    capModelId: number;
    capStyleId: number;
    isFirst: boolean;
    addStyleRow: () => void;
    setStyleToRemove: (key: string) => void;
    capId: string;
    values: JobStyle | null;
}

export class CapStyleContentHandle {
    getJobStyle: () => JobStyle | null;

    constructor(getJobStyle: () => JobStyle | null) {
        this.getJobStyle = getJobStyle;
    }
}


function CapStyle(props: CapStyleProps, ref: Ref<CapStyleContentHandle>) {

    const capContextData = useCapContext();

    const [capStyle, setCapStyle] = useState<string | null>(null);

    const [cap, setCap] = useState<Cap | null>(null);

    useImperativeHandle(ref, () => ({
        getJobStyle
    }));

    function getJobStyle(): JobStyle | null {
        if (capStyle) {
            if (cap) {
                const style = cap.styles.find((style) => style.name == capStyle);
                if (style) {
                    return new JobStyle(style);
                }
            }
        }
        return null;
    }

    function getStylesForCap(): Style[] {
        if (cap) {
            return cap.styles;
        }
        return [];
    }

    function addNewStyle(e: any) {
        e.preventDefault();
        props.addStyleRow();
    }

    function removeStyle(e: any) {
        e.preventDefault();
        const key = e.currentTarget.getAttribute("data-value");
        props.setStyleToRemove(key);
    }

    function onStyleChange(event: any) {
        const value = event.target.value;
        setCapStyle(value);
    }

    function getKeyPrefix(): string {
        return `cap-${props.capModelId}-style-${props.capStyleId}`;
    }

    useEffect(() => {
        const capLookup = capContextData.getCapLookup();
        if (capLookup)
        {
            const cap = capLookup.getCap(props.capId);
            if (cap) {
                setCap(cap);
            }
        }

        if (props.values != null) {
            setCapStyle(props.values.style.name);
        }

    }, []);


    return (
        <span className="ca-form-row-extension p-0">
            <div className="col-sm-4 ca-blue-background">
                <label htmlFor={getKeyPrefix()} className="col-form-label">Style:</label>
                <select id={getKeyPrefix()} className="form-select" required={props.isFirst}
                        value={capStyle == null ? '' : capStyle}
                        onChange={onStyleChange} >
                    <option key="-1" value="" defaultValue="">Select a style</option>
                    {getStylesForCap().map(style => (
                        <option key={style.id} value={style.name}>
                            {style.name}
                        </option>
                    ))}
                </select>
            </div>
            <div className="col-sm-2 pl-2 ca-form-row-buttons ca-blue-background">
                <span>
                    { cap != null &&
                        <button id={`add-${getKeyPrefix()}`} className={'btn btn-outline-primary ca-form-button'} onClick={addNewStyle}>Add</button>
                    }
                    { cap != null && !props.isFirst &&
                        <button id={`remove-${getKeyPrefix()}`} className={'btn btn-outline-danger ca-form-next-button'} data-value={`${props.capModelId}-style-${props.capStyleId}`} onClick={removeStyle}>Remove</button>
                    }
                </span>
            </div>
        </span>
    )
}

export default React.forwardRef<CapStyleContentHandle, CapStyleProps>(CapStyle);
