import { Container, Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import TranscriptTerm from "./TranscriptTerm";

export default  observer(function TranscriptPage() {
    const {contentStore: {currentSectionTerms, sectionLoaded, selectedContentMetadata}} = useStore();
    
    if (!sectionLoaded){
        return (
        <Container>
            <Header as="h2" content="Loading..." />
        </Container>
        );  
    }
    const headerText = (currentSectionTerms.sectionHeader === 'none') ? selectedContentMetadata?.contentName : currentSectionTerms.sectionHeader;
    return(
        <div>
            <Header as='h2'>{headerText}</Header>
            <Container className="segment">
            {
                currentSectionTerms.abstractTerms.map(trm => (
                    <TranscriptTerm term={trm} key={trm.indexInChunk} />
                ))
            }
            </Container>
        </div>
        
    )
});