import React from 'react';
import { CapRecord } from '../../model/CapRecord';
import { StyleRecord } from '../../model/StyleRecord';

interface StyleEntryProps {
    record: StyleRecord
}

export default function StyleEntry({record}: StyleEntryProps) {

    return (
        <>
            <td>{record.style_name}</td>
            <td>{record.artboard_name}</td>
        </>
    );
}
