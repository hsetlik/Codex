import { observer } from "mobx-react-lite";
import React from "react";
import { Button, List } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import "../../styles/details.css";

interface Props {
    value: string,
    term: AbstractTerm
}

export default observer(function Translation({value, term}: Props) {
    console.log(`Translation has value ${value}`);
    const {userStore: {deleteTranslation}} = useStore();
    return (
        <List.Item>
            <List.Content>
                <Button floated='right' icon='close' size='mini'  
                onClick={() => deleteTranslation({value: value, userTermId: term.userTermId})}
                disabled={term.translations.length < 2} />
                <List.Header as='h3' className="details-h2" >{value}</List.Header>
            </List.Content>
        </List.Item>
    )

})