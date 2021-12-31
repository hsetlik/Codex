import { observer } from "mobx-react-lite";
import React from "react";
import { Button } from "semantic-ui-react";
import { AbstractToUserTermDetails } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";


export default observer(function StarButton() {
    const {contentStore: {selectedTerm}, userStore: {updateUserTerm}} = useStore();

    const handleClick = () => {
        var details = AbstractToUserTermDetails(selectedTerm!);
        console.log(details);
        details.starred = !details.starred;
        updateUserTerm(details);
    }

    return (
        <Button 
        icon='star'
        size='mini'
        floated='right'
        active={selectedTerm?.starred}
        onClick={handleClick} />
    )
})