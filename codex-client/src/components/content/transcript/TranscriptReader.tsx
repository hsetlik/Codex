import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Container, Segment, Grid } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import TermDetails from "../termDetails/AbstractTermDetails";
import TranscriptPage from "./TranscriptPage";
import TranscriptPageHeader from "./TranscriptPageHeader";

interface Props {
    contentUrl: string;
    index: number;
}

export default observer(function TranscriptReader({contentUrl, index}: Props){
    //TODO: useEffect to make sure content is loaded
return(        
        <Container >
            <Grid>
                <Grid.Column width='10'>
                    <TranscriptPageHeader contentUrl={contentUrl} index={index} />
                    <TranscriptPage paragraphIndex={index}  />
                </Grid.Column>
                <Grid.Column width='6'>
                    <TermDetails />
                </Grid.Column>
            </Grid>
        </Container>
    )
})