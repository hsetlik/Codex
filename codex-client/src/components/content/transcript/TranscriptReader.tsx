import { observer } from "mobx-react-lite";
import { Container, Grid, Ref, Sticky } from "semantic-ui-react";
import TermDetails from "../termDetails/AbstractTermDetails";
import TranscriptPage from "./TranscriptPage";
import TranscriptPageHeader from "./TranscriptPageHeader";

interface Props {
    index: number;
    contentId: string;
}

export default observer(function TranscriptReader({index, contentId}: Props){
   
return(        
        <Container >
                <Grid>
                <   Grid.Column width='10'>
                    <TranscriptPageHeader contentId={contentId} index={index} />
                    <TranscriptPage  />
                </Grid.Column>
                <Grid.Column width='6'>
                    <Sticky offset={65} >
                        <TermDetails />
                    </Sticky>
                </Grid.Column>
                </Grid>
        </Container>
    )
})