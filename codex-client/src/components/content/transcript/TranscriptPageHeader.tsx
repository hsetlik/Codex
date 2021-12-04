import { observer } from "mobx-react-lite";
import React from "react";
import { useNavigate } from "react-router";
import { Link } from "react-router-dom";
import { Header, Segment, Button } from "semantic-ui-react";
import { ParagraphQueryDto } from "../../../app/api/agent";
import ContentStore from "../../../app/stores/contentStore";
import { useStore } from "../../../app/stores/store";



export default observer (function TranscriptPageHeader({contentUrl, index}: ParagraphQueryDto) {
    const nextParagraphPath = () => {
        return `../content/${contentUrl}/${index + 1}`;
    }
    const prevParagraphPath = () => {
        return `../content/${contentUrl}/${index - 1}`;
    }
    const {contentStore} = useStore();
    const {selectedContentParagraphCount} = contentStore;
    return(
        <Segment>
            <Header>
            Paragraph {index} of {selectedContentParagraphCount}
            </Header>
            <Button basic content='Previous' disabled={index === 0} as={Link} to={prevParagraphPath()} />
            <Button basic content='Next' disabled={index >= selectedContentParagraphCount - 1} as={Link} to={nextParagraphPath()}/>
        </Segment>
   )
});