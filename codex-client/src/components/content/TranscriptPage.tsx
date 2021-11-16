import React from "react";
import { Container, Grid, Header, Item, Label, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import TranscriptTerm from "./TranscriptTerm";

export default function TranscriptPage() {
    const {transcriptStore} = useStore();
    const {currentAbstractTerms, termsAreLoaded} = transcriptStore;
    if (!termsAreLoaded){
        return (
        <Container>
            <Header as="h2" content="Loading..." />
        </Container>
        );  
    }
    return(
        <Container>
            <Grid as='p'> 
                   {
                       currentAbstractTerms.map(trm => {
                           return <TranscriptTerm term={trm} key={trm.indexInChunk} />
                       })
                   }
            </Grid>
        </Container>
    )
}