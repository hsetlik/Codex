import { observer } from "mobx-react-lite";
import { Container, Segment } from "semantic-ui-react";
import TranscriptPage from "./TranscriptPage";

export default observer(function TranscriptReader(){
    return(
        <Container >
            <Segment>
                <TranscriptPage />
            </Segment>
        </Container>
    )
})