import React, { useEffect, useState } from 'react';
import { CapRecord } from '../../model/CapRecord';

interface CapEntryProps {
    record: CapRecord
}

export default function CapEntry({record}: CapEntryProps) {

    useEffect(() => {
        console.log("RECORD " + record)
    }, []);

    return (
        <>
            <td>{record.cap_code}</td>
            <td>{record.filename}</td>
            <td>{record.has_front_right ? 'Y' : 'N'}</td>
            <td>{record.has_front_center ? 'Y' : 'N'}</td>
            <td>{record.has_front_left ? 'Y' : 'N'}</td>
            <td>{record.has_bill_left ? 'Y' : 'N'}</td>
            <td>{record.has_bill_center ? 'Y' : 'N'}</td>
            <td>{record.has_bill_right ? 'Y' : 'N'}</td>
            <td>{record.has_side_left ? 'Y' : 'N'}</td>
            <td>{record.has_side_right ? 'Y' : 'N'}</td>
            <td>{record.has_back ? 'Y' : 'N'}</td>
            <td>{record.has_strap ? 'Y' : 'N'}</td>
            <td>{record.images_on_right ? 'Y' : 'N'}</td>
            <td>{record.styles.length}</td>
        </>
    );
}
