import { observer } from "mobx-react-lite";
import React from "react";
import { useNavigate } from "react-router";
import { Link } from "react-router-dom";
import { Header, Segment, Button } from "semantic-ui-react";
import { SectionQueryDto } from "../../../app/api/agent";
import ContentStore from "../../../app/stores/contentStore";
import { useStore } from "../../../app/stores/store";

interface Props {
    index: number;
    contentId: string;
}


export default observer (function TranscriptPageHeader({index, contentId}: Props) {
    const nextSectionPath = `../content/${contentId}/${index + 1}`;
    const prevSectionPath =  `../content/${contentId}/${index - 1}`;
    const {contentStore} = useStore();
    const {selectedContentSectionCount} = contentStore;
    return(
        <Segment>
            <Header>
            Section {index} of {selectedContentSectionCount}
            </Header>
            <Button basic content='Previous' disabled={index === 0} as={Link} to={prevSectionPath} />
            <Button basic content='Next' disabled={index >= selectedContentSectionCount - 1} as={Link} to={nextSectionPath}/>
        </Segment>
   )
});