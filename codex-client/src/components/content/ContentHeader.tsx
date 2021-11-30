import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Header, Label, Segment } from "semantic-ui-react";
import { ContentHeaderDto } from "../../app/api/agent";
import { useStore } from "../../app/stores/store";
import KnownWordsHeader from "./KnownWordsHeader";

interface Props{
    dto: ContentHeaderDto
}

export default observer(function ContentHeader({dto}: Props)
{
    const {userStore} = useStore();
   
    console.log("Content ID is: " + dto.contentId);
    return (
            <Segment>
                <Header >{dto.contentName}</Header>
                <KnownWordsHeader contentId={dto.contentId} />
                <Label as="h2">{dto.contentType}</Label>
                <Button as={Link} className='label'
                color='twitter' 
                to={`../content/${dto.contentId}/0`} 
                onClick={() => userStore.setSelectedContent(dto.contentId)}>
                    View
                </Button>
            </Segment>
    )
})