import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Segment, Button, Header } from "semantic-ui-react";
import { useStore } from "../../../../app/stores/store";

interface Props {
    contentId: string,
    currentIndex: number
}

export default observer(function SectionNavigator({contentId, currentIndex}: Props) {
    const {contentStore} = useStore();
    const {currentSectionTerms, selectedContentMetadata} = contentStore;
    const nextSectionPath = `/content/${contentId}/${currentIndex + 1}`;
    const prevSectionPath = `/content/${contentId}/${currentIndex - 1}`;
    return (
        <div>
            <Header as='h2'>{currentSectionTerms.sectionHeader}</Header>
            <Header as='h3'>Section {currentIndex + 1} of {selectedContentMetadata?.numSections!}</Header>
            <Segment>
                <Button as={Link} to={prevSectionPath} active={(currentIndex - 1) > 0} disabled={currentIndex < 1}>Prev</Button>
                <Button as={Link} to={nextSectionPath} disabled={currentIndex >= selectedContentMetadata?.numSections!}>Next</Button>
            </Segment>
        </div>
    )
})