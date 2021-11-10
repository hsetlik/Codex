import React from "react";
import { Container, Label, Segment } from "semantic-ui-react";
import { ContentHeaderDto } from "../../app/api/agent";

export default function ContentHeader(props: ContentHeaderDto)
{
    return (
        <Container>
            <Segment>
                <Label as="h1">{props.contentName}</Label>
                <Label as="h2">{props.contentType}</Label>
            </Segment>
        </Container>
    )

}