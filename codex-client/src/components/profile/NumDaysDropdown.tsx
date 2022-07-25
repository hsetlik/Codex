import { observer } from "mobx-react-lite";
import React from "react";
import { Dropdown, } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";



const rangeOptions = [
    7,
    14,
    30,
    90
]

export default observer( function NumDaysDropdown() {
    const {dailyDataStore: {setCurrentNumDays, currentNumDays}} = useStore();
    const stringForDays = (days: number) => {
        return `Last ${days} days`;
    }
    return (
        <Dropdown
        value={stringForDays(currentNumDays)}
        placeholder={stringForDays(currentNumDays)}
        style={
            {
                'marginTop': '10px'
            }
        }>
            <Dropdown.Menu>
                {rangeOptions.map(opt => (
                    <Dropdown.Item key={opt} onClick={() => setCurrentNumDays(opt)}>
                        {stringForDays(opt)}
                    </Dropdown.Item>
                ))}
            </Dropdown.Menu>
        </Dropdown>
    )

})