import { observer } from "mobx-react-lite";
import React from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";



const rangeOptions: DropdownItemProps[] = [
    {
        key: "Last 7 days",
        text: "Last 7 days",
        value: 7
    },
    {
        key: "Last 14 days",
        text: "Last 14 days",
        value: 14
    },
    {
        key: "Last 30 days",
        text: "Last 30 days",
        value: 30
    }
]

export default observer( function NumDaysDropdown() {
     const {profileStore: {setCurrentNumDays}} = useStore();
    return (
        <Dropdown
        options={rangeOptions}
        fluid
        selection={true}
        onChange={(e, data) => {
            setCurrentNumDays(data.value as number);
        }}
        >
        </Dropdown>
    )

})