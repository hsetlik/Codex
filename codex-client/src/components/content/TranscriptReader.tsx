import React from "react";
import { observer } from "mobx-react-lite";
import { Container, Header, Label, Loader, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
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