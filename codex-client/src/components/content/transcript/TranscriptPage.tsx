import { Container, Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import TranscriptTerm from "./TranscriptTerm";
import { useEffect } from "react";

interface Props {
    contentId: string;
}

export default  observer(function TranscriptPage({contentId}: Props) {
    const {transcriptStore} = useStore();
    const {currentAbstractTerms, termsAreLoaded, loadContent} = transcriptStore;
    useEffect(() => {
        loadContent(contentId);
    }, [loadContent, contentId])
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
                currentAbstractTerms.map(trm => (
                    <TranscriptTerm term={trm} key={trm.indexInChunk} />
                ))
            }
        </Container>
    )
});