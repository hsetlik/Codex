import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { Container, Header, List } from "semantic-ui-react";
import AddTranslationForm from "../reader/AddTranslationForm";
import RatingButtonGroup from "../termDetails/RatingButtonGroup"
import { useStore } from "../../../app/stores/store";
import Translation from "./Translation";

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
                       <Translation term={selectedTerm!} value={t} key={t} /> 
                    ))}
                </List>
                <RatingButtonGroup />
                <Header as='h3' content='Add Translation' />
                <AddTranslationForm term={selectedTerm!} />
            </Container>
    )

})