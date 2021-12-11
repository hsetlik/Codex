import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Header, Label, Segment } from "semantic-ui-react";
import { ContentMetadataDto } from "../../app/api/agent";
import { useStore } from "../../app/stores/store";
import KnownWordsHeader from "./KnownWordsHeader";

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
                <KnownWordsHeader contentId={dto.contentUrl} />
                <Label as="h2">{dto.contentType}</Label>
                <Button as={Link} className='label'
                color='twitter' 
                to={`../content/${dto.contentId}/0`} 
                onClick={() => contentStore.setSelectedContent(dto.contentUrl)}>
                    View
                </Button>
            </Segment>
    )
})