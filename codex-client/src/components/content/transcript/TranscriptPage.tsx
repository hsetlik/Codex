import { Container, Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import TranscriptTerm from "./TranscriptTerm";
import { useEffect } from "react";

interface Props {
    paragraphIndex: number;
    contentUrl: string;
}

export default  observer(function TranscriptPage({paragraphIndex, contentUrl}: Props) {
    const {contentStore: {loadParagraph, currentParagraphTerms, paragraphLoaded}} = useStore();
    useEffect(() => {
        loadParagraph(contentUrl, paragraphIndex);
    }, [contentUrl, loadParagraph, paragraphIndex])
    if (!paragraphLoaded){
        return (
        <Container>
            <Header as="h2" content="Loading..." />
        </Container>
        );  
    }
    return(
        <Container className="segment">
            {
                currentParagraphTerms.abstractTerms.map(trm => (
                    <TranscriptTerm term={trm} key={trm.indexInChunk} />
                ))
            }
        </Container>
    )
});