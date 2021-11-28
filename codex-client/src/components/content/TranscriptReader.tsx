import { observer } from "mobx-react-lite";
import { Container, Segment, Grid } from "semantic-ui-react";
import TermDetails from "./AbstractTermDetails";
import TranscriptPage from "./TranscriptPage";

interface Props {
    contentId: string;
}

export default observer(function TranscriptReader({contentId}: Props){
    return(
        <Container >
            <Grid>
                <Grid.Column width='10'>
                    <TranscriptPage  contentId={contentId} />
                </Grid.Column>
                <Grid.Column width='6'>
                    <TermDetails />
                </Grid.Column>
            </Grid>
        </Container>
    )
})