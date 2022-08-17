import { observer } from "mobx-react-lite";
import React from "react";
import { Header, List, Segment } from "semantic-ui-react";
import { getLanguageName } from "../../../app/common/langStrings";
import { UserTerm } from "../../../app/models/userTerm";

interface Props {
    term: UserTerm;
}


export default observer (function VocabWord({term}: Props) {
    return (
        <Segment>
            <Header as='h2'>{term.termValue}</Header>
            <List>
                <List.Item key='language'>
                    Language: {getLanguageName(term.language)}
                </List.Item>
                <List.Item key='timesSeen'>
                   Seen {term.timesSeen} times
                </List.Item>
                <List.Item key='translations'>
                    <List>
                    { term.translations.map(tran => {
                       return <List.Item key={tran}>{tran}</List.Item>
                    })
                    }
                    </List>
                </List.Item>
            </List>
        </Segment>
    );
})