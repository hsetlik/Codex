import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Header, Segment, Button } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";

interface Props {
    index: number;
    contentId: string;
}


export default observer (function TranscriptPageHeader({index, contentId}: Props) {
    const nextSectionPath = `../content/${contentId}/${index + 1}`;
    const prevSectionPath =  `../content/${contentId}/${index - 1}`;
    const {contentStore} = useStore();
    const {selectedContentMetadata} = contentStore;
    return(
        <Segment>
            <Header>
            Section {index + 1} of {selectedContentMetadata?.numSections}
            </Header>
            <Button basic content='Previous' disabled={index === 0} as={Link} to={prevSectionPath} />
            <Button basic content='Next' disabled={index >= selectedContentMetadata?.numSections! - 1} as={Link} to={nextSectionPath}/>
        </Segment>
   )
});