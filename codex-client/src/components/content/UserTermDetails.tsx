import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Header, List } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";
import AddTranslationForm from "./AddTranslationForm";

interface Props {
    term: AbstractTerm
}

//NOTE: this is an observer because its props are from a store object, even though this component doesn't call useStore() itself
export default observer(function UserTermDetails({term}: Props) {

    return (
            <Container className="segment">
                <Header as='h3' content='Translations' />
                <List>
                    {term.translations.map(t => (
                       <List.Item key={t}>{t}</List.Item>
                    ))}
                </List>
                <Header as='h3' content='Add Translation' />
                <AddTranslationForm term={term} />
            </Container>
    )

})