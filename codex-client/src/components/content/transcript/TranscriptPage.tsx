import { Container, Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import TranscriptTerm from "./TranscriptTerm";
import { useEffect } from "react";

interface Props {
    sectionIndex: number;
    contentUrl: string;
}

export default  observer(function TranscriptPage({sectionIndex, contentUrl}: Props) {
    const {contentStore: {loadSection, currentSectionTerms: currentSectionTerms, sectionLoaded}} = useStore();
    useEffect(() => {
        loadSection(contentUrl, sectionIndex);
    }, [contentUrl, loadSection, sectionIndex])
    if (!sectionLoaded){
        return (
        <Container>
            <Header as="h2" content="Loading..." />
        </Container>
        );  
    }
    return(
        <Container className="segment">
            {
                currentSectionTerms.abstractTerms.map(trm => (
                    <TranscriptTerm term={trm} key={trm.indexInChunk} />
                ))
            }
        </Container>
    )
});