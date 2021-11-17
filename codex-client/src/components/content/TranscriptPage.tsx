import { Container, Grid, Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import TranscriptTerm from "./TranscriptTerm";
import { observer } from "mobx-react-lite";

export default  observer(function TranscriptPage() {
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
            {
                currentAbstractTerms.map(trm => {
                    if (trm.trailingCharacters.length < 1) {
                        return <TranscriptTerm term={trm} key={trm.indexInChunk} />
                    }
                    else {
                        return (
                            <TranscriptTerm term={trm} key={trm.indexInChunk} />
                        )
                    }
                })
            }
        </Container>
    )
});