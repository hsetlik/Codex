import { Container, Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import TranscriptTerm from "./TranscriptTerm";

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
        <Container className="segment">
            {
                currentAbstractTerms.entries
            }
        </Container>
    )
});