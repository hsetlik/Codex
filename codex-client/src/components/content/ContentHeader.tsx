import { Link } from "react-router-dom";
import { Button, Header, Label, Segment } from "semantic-ui-react";
import { ContentHeaderDto } from "../../app/api/agent";
import { useStore } from "../../app/stores/store";

interface Props{
    dto: ContentHeaderDto
}

export default function ContentHeader({dto}: Props)
{
    const {userStore} = useStore();
    console.log("Content ID is: " + dto.contentId);
    return (
            <Segment>
                <Header >{dto.contentName}</Header>
                <Label as="h2">{dto.contentType}</Label>
                <Button as={Link} className='label'
                color='twitter' 
                to={`../content/${dto.contentId}`} 
                onClick={() => userStore.setSelectedContent(dto.contentId)}>
                    View
                </Button>
            </Segment>
    )

}