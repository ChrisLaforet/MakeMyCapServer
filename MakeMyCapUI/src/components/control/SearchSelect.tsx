import React, { useEffect, useState } from 'react';
import './SearchSelect.css';

export interface SearchSelectProps {
    id: string;
    promptText: string | null;
    selectableItems: SearchSelectable[];
    externallySelectedValue: string | null;
    onChange: (selected: SearchSelectable | null) => void;
}

export class SearchSelectable {
    public readonly value: string;
    public readonly display: string;

    constructor(value: string, display: string) {
        this.value = value;
        this.display = display;
    }
}

export function SearchSelect(props: SearchSelectProps) {

    const [selected, setSelected] = useState<string | null>(props.externallySelectedValue);
    const [filter, setFilter] = useState<string>('');
    const [selections, setSelections] = useState<SearchSelectable[]>(props.selectableItems);

    const onFilterChange = (event: any) => {
        setFilter(event.target.value);
    }

    const onSelectionChange = (event: any) => {
        const value = event.target.value;
        const match = props.selectableItems.filter((item: SearchSelectable) => item.value == value);
        setSelected(match.length >= 1 ? match[0].value : null);
        props.onChange(match.length >= 1 ? match[0] : null);
    }

    const getSelectedValue = (): string => {
        return selected != null ? selected : '';
    }

    useEffect(() => {
        setSelected(props.externallySelectedValue);
    }, [props.externallySelectedValue]);

    useEffect(() => {
        const matcher = filter.toUpperCase();
        const items = props.selectableItems.filter((item) => item.display.toUpperCase().indexOf(matcher) >= 0);
        if (selected != null) {
            let found = false;
            for (const item of items) {
                if (item.value == selected) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                const selectedItem = props.selectableItems.filter((item) => item.value == selected)
                if (selectedItem.length > 0) {
                    items.unshift(selectedItem[0]);
                }
            }
        }
        setSelections(items);
    }, [filter, props.selectableItems]);

    return (
            <span id={props.id} className="form-control search-select-wrapper">
                <input id={props.id + "1"} className="form-control search-select-filter" type="text" onChange={onFilterChange} maxLength={7} placeholder="Filter" />
                <select id={props.id + "-2"} className="form-select search-select-selector"
                        value={getSelectedValue()}
                        onChange={onSelectionChange}>
                    {props.promptText != null &&
                        <option value="" defaultValue="">{props.promptText}</option>
                    }
                    {selections.map(selectable => (
                        <option key={selectable.value} value={selectable.value}>
                        {selectable.display}
                        </option>
                    ))}
                </select>

            </span>
    );
}
