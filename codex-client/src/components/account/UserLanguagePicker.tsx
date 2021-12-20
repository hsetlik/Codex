import React from "react";
import { Dropdown } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

export default function UserLanguagePicker(){
    const {userStore} = useStore();
    var dropdownItems: {}[] = [];
    for (const i in userStore.languageProfileStrings) {
        const item = {
            key: i,
            text: i,
            value: i
        }
        console.log(item);
        dropdownItems.push(item);
    }
    return(
        <Dropdown defaultValue={'en'}
        fluid
        selection
        options={dropdownItems}
        onChange={ ({}) => {
            
        }}>
        </Dropdown>
    )
}