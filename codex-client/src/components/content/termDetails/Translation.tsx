import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Header, Icon, List, Segment } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";

interface Props {
    value: string,
    term: AbstractTerm
}

export default observer(function Translation({value, term}: Props) {
    const {userStore: {deleteTranslation}} = useStore();
    return (
        <List.Item>
            <List.Content>
                <Button floated='right' icon='close' size='mini' 
                onClick={() => deleteTranslation({value: value, userTermId: term.userTermId})}
                disabled={term.translations.length < 2} />
            </List.Content>
            <List.Content>
                <List.Header as='h3' >{value}</List.Header>
            </List.Content>
        </List.Item>
    )

})