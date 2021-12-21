import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Grid, Loader, Sticky } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import SectionNavigator from "./reader/section/SectionNavigator";
import SectionReader from "./reader/section/SectionReader";
import AbstractTermDetails from "./termDetails/AbstractTermDetails";

export default observer(function ContentRoute() {
    const {contentId, index} = useParams();
    const {contentStore} = useStore();
    const {loadSectionById, currentSectionTerms, sectionLoaded} = contentStore;
    useEffect(() => {
        loadSectionById(contentId!, parseInt(index!));
    }, [loadSectionById, contentId, index]);

    return (
        <div>
            <Grid>
                <Grid.Column width={10}>
                    {
                        sectionLoaded? 
                        (
                            <div>
                                <SectionNavigator contentId={contentId!} currentIndex={parseInt(index!)} />
                                <SectionReader section={currentSectionTerms} />
                            </div>
                            
                         ) : (
                            <Loader />
                         )
                    }
                    
                </Grid.Column>
                <Grid.Column width={6}>
                    <Sticky offset={35} pushing={false} >
                        <AbstractTermDetails  />
                    </Sticky>
                </Grid.Column>
            </Grid>
            

        </div>
    )
})