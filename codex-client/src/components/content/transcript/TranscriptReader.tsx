import { observer } from "mobx-react-lite";
import { Container, Grid } from "semantic-ui-react";
import TermDetails from "../termDetails/AbstractTermDetails";
import TranscriptPage from "./TranscriptPage";
import TranscriptPageHeader from "./TranscriptPageHeader";

interface Props {
    contentUrl: string;
    index: number;
    contentId: string;
}

export default observer(function TranscriptReader({contentUrl, index, contentId}: Props){
    //TODO: useEffect to make sure content is loaded
return(        
        <Container >
            <Grid>
                <Grid.Column width='10'>
                    <TranscriptPageHeader contentId={contentId} index={index} />
                    <TranscriptPage contentUrl={contentUrl} sectionIndex={index}  />
                </Grid.Column>
                <Grid.Column width='6'>
                    <TermDetails />
                </Grid.Column>
            </Grid>
        </Container>
    )
})