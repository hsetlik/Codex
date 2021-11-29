import { Container, Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import TranscriptTerm from "./TranscriptTerm";
import { useEffect } from "react";

interface Props {
    chunkIndex: number;
}

export default  observer(function TranscriptPage({chunkIndex}: Props) {
    const {transcriptStore: {currentAbstractTerms, termsAreLoaded, loadTermsForChunk, transcriptChunkIds}} = useStore();
    useEffect(() => {
        const chunkId = transcriptChunkIds.find(t => t.index === chunkIndex);
        console.log(`Loading Chunk: ${chunkId} at index ${chunkIndex}`);
        if (chunkId !== undefined) {
            loadTermsForChunk(chunkId.chunkId);
        }
    }, [loadTermsForChunk, transcriptChunkIds, chunkIndex])
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