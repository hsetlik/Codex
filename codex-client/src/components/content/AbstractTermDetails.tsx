import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import TermDetails from "./TermDetails";
import TranslationsList from "./TranslationList";
import UserTermDetails from "./UserTermDetails";


export default observer(function AbstractTermDetails() {
    const {transcriptStore} = useStore();
    const {selectedTerm} = transcriptStore;
    let termComponent;
    if (selectedTerm?.hasUserTerm) {
        termComponent = <UserTermDetails term={selectedTerm} />
    } else {
        termComponent = <TermDetails term={selectedTerm!} />
    }
    return (
        <Container>
            {termComponent}
        </Container>
    )
});