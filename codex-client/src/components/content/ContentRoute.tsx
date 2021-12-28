import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Grid, Loader } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import SectionNavigator from "./leftColumn/reader/SectionNavigator";
import AbstractTermDetails from "./rightColumn/AbstractTermDetails";
import '../styles/styles.css';
import SectionReader from "./leftColumn/reader/SectionReader";
import LeftColumnDiv from "./leftColumn/LeftColumnDiv";

export default observer(function ContentRoute() {
    const {contentId, index} = useParams();
    const {contentStore} = useStore();
    const {loadSectionById, currentSectionTerms, sectionLoaded} = contentStore;
    

  useEffect(() => { 
    loadSectionById(contentId!, parseInt(index!));
  }, [loadSectionById, contentId, index]);
  const url = currentSectionTerms.contentUrl;

    return (
        <div>
            <Grid>
                <Grid.Column width={10} key={"left"}>
                    {
                        sectionLoaded? 
                        (
                           <LeftColumnDiv contentId={contentId!} index={parseInt(index!)} /> 
                            
                         ) : (
                            <Loader />
                         )
                    }
                    
                </Grid.Column >
                <Grid.Column width={6} key={"right"} >
                        <div className="codex-term-details">
                            <AbstractTermDetails />
                        </div>
                </Grid.Column>
            </Grid>
            

        </div>
    )
})