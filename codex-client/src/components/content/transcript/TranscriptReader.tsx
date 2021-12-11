import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useRef } from "react";
import { Container, Grid, Ref, Sticky } from "semantic-ui-react";
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
    const ref = useRef<any>(null);
    useEffect(() => {
        ref.current?.focus();
    },[ref]);
return(        
        <Container >
           <Ref innerRef={ref}>
                <Grid>
                <   Grid.Column width='10'>
                    <TranscriptPageHeader contentId={contentId} index={index} />
                    <TranscriptPage contentUrl={contentUrl} sectionIndex={index}  />
                </Grid.Column>
                <Grid.Column width='6'>
                    <Sticky context={ref} >
                        <TermDetails />
                    </Sticky>
                </Grid.Column>
                </Grid>
            </Ref>
        </Container>
    )
})