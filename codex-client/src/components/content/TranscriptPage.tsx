import React from "react";
import { Container, Header, Item, Label, Segment } from "semantic-ui-react";
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
        <Segment>
           <Item>
               <Item.Group divided>
                   {
                       currentAbstractTerms.map(trm => {
                           return <TranscriptTerm term={trm} key={trm.termValue} />
                       })
                   }
               </Item.Group>
           </Item>

        </Segment>
    )
}