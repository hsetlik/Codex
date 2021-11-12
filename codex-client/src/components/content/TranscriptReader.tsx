import React from "react";
import { observer } from "mobx-react-lite";
import { Container, Label, Loader } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

export default observer(function TranscriptReader(){
    const {transcriptStore} = useStore();
    const {currentAbstractTerms, termsAreLoaded} = transcriptStore;
    if (!termsAreLoaded){
        return <Loader />;
    }
    return(
        <Container >
            {
                currentAbstractTerms.map(term => {
                    <Label>{term.termValue}</Label>
                })
            }
        </Container>
    )
})