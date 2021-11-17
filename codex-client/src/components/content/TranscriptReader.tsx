import { observer } from "mobx-react-lite";
import { Container, Segment, Grid } from "semantic-ui-react";
import TermDetails from "./AbstractTermDetails";
import TranscriptPage from "./TranscriptPage";

export default observer(function TranscriptReader(){
    return(
        <Container >
            <Grid>
                <Grid.Column width='10'>
                    <TranscriptPage />
                </Grid.Column>
                <Grid.Column>
                    <TermDetails />
                </Grid.Column>
            </Grid>
        </Container>
    )
})