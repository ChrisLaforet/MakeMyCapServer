import { useEffect, useRef, useState } from 'react';
import './FileSelector.css';


export interface FileSelectorProps {
    id: string;
    externallySelectedValue: string | null;
    onChange: (filename: string) => void;
}

export function FileSelector(props: FileSelectorProps) {

    const [selectionValue, setSelectionValue] = useState<string>('');
    const chooserButton = useRef<HTMLInputElement>(null);

    function getFileValue() {
        return props.externallySelectedValue == null ? '' : props.externallySelectedValue;
    }

    function setFileValue(event: any) {
        const rawValue = event.target.value;
        let value = rawValue;
        if (rawValue.indexOf('\\') >= 0) {
            const parts = rawValue.split('\\');
            value = parts[parts.length - 1];
        } else if (rawValue.indexOf('/') >= 0) {
            const parts = rawValue.split('/');
            value = parts[parts.length - 1];
        }
        setSelectionValue(value);
        props.onChange(value);
    }

    function triggerChooser(event: any) {
        event.preventDefault();
        chooserButton.current?.click();
    }

    useEffect(() => {
        if (props.externallySelectedValue != null) {
            setSelectionValue(props.externallySelectedValue);
        }
    }, [props.externallySelectedValue]);

    return (
        <span id={props.id} className="form-control file-select-wrapper">
            <button id={`{props.id}-chooseButton`} type="button" className="form-control file-select-button" onClick={e => triggerChooser(e)}>Choose File</button>
            <input id={`{props.id}-filename`} type="text" className="form-control file-select-value" value={selectionValue} required />
            <input id={`{props.id}-chooser`} ref={chooserButton} type="file" hidden={true}
                   onChange={(e) => setFileValue(e)}/>
        </span>
    );
}
