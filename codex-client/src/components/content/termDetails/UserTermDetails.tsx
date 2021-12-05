import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { Container, Header, List } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import AddTranslationForm from "../transcript/AddTranslationForm";
import RatingButtonGroup from "../termDetails/RatingButtonGroup"
import { useStore } from "../../../app/stores/store";

interface Props {
    term: AbstractTerm
}

//NOTE: this is an observer because its props are from a store object, even though this component doesn't call useStore() itself
export default observer(function UserTermDetails() {
    const {contentStore: {selectedTerm, translationsLoaded, loadSelectedTermTranslations}} = useStore();
    useEffect(() => {
        if (!translationsLoaded) {
            loadSelectedTermTranslations();
        }
    }, [translationsLoaded, loadSelectedTermTranslations]);
    return (
            <Container className="segment">
                <Header as='h3' content='Translations' />
                <List >
                    {selectedTerm!.translations.map(t => (
                       <List.Item key={t}>{t}</List.Item>
                    ))}
                </List>
                <RatingButtonGroup />
                <Header as='h3' content='Add Translation' />
                <AddTranslationForm term={selectedTerm!} />
            </Container>
    )

})