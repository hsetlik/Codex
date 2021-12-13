import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Header, Label, Segment } from "semantic-ui-react";
import { ContentMetadataDto } from "../../app/api/agent";
import { useStore } from "../../app/stores/store";
import KnownWordsLabel from "./KnownWordsLabel";

interface Props{
    dto: ContentMetadataDto
}

export default observer(function ContentHeader({dto}: Props)
{
    const {contentStore} = useStore();
    console.log("Content ID is: " + dto.contentUrl);
    return (
            <Segment>
                <Header >{dto.contentName}</Header>
                <div style={{padding: '10px'}}>
                    <Label >{dto.contentType}</Label>
                    <Label >Section {dto.bookmark} of {dto.numSections}</Label>
                    <KnownWordsLabel contentId={dto.contentId} />
                </div>
                <div style={{padding: '10px'}}>
                    <Button as={Link}
                    color='twitter' 
                    to={`../content/${dto.contentId}/${dto.bookmark}`} 
                    onClick={() => contentStore.setSelectedContent(dto.contentUrl)}>
                        View
                    </Button>
                </div>
            </Segment>
    )
})