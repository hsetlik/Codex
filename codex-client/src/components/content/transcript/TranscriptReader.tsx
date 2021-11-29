import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Container, Segment, Grid } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import TermDetails from "../termDetails/AbstractTermDetails";
import TranscriptPage from "./TranscriptPage";
import TranscriptPageHeader from "./TranscriptPageHeader";

interface Props {
    contentId: string;
    chunkIndex: number;
}

export default observer(function TranscriptReader({contentId, chunkIndex}: Props){
    const {transcriptStore} = useStore();
    const {loadContent} = transcriptStore;
    useEffect(() => {

        loadContent(contentId);

    }, [loadContent, contentId])
    return(
        
        <Container >
            <Grid>
                <Grid.Column width='10'>
                    <TranscriptPageHeader chunkIndex={chunkIndex} />
                    <TranscriptPage chunkIndex={chunkIndex}  />
                </Grid.Column>
                <Grid.Column width='6'>
                    <TermDetails />
                </Grid.Column>
            </Grid>
        </Container>
    )
})