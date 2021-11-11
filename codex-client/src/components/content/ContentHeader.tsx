import { Header, Label, Segment } from "semantic-ui-react";
import { ContentHeaderDto } from "../../app/api/agent";

interface Props{
    dto: ContentHeaderDto
}

export default function ContentHeader({dto}: Props)
{
    return (
            <Segment>
                <Header as="h1">{dto.contentName}</Header>
                <Label as="h2">{dto.contentType}</Label>
            </Segment>
    )

}