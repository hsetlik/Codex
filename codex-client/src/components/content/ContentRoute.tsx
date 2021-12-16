import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Grid } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import SectionReader from "./reader/section/SectionReader";
import AbstractTermDetails from "./termDetails/AbstractTermDetails";

export default observer(function ContentRoute() {
    const {contentId, index} = useParams();
    const {contentStore} = useStore();
    const {loadSectionById, currentSectionTerms} = contentStore;
    useEffect(() => {
        loadSectionById(contentId!, parseInt(index!));
    }, [loadSectionById, contentId, index]);

    return (
        <div>
            <Grid>
                <Grid.Column width={10}>
                    <SectionReader section={currentSectionTerms} />
                </Grid.Column>
                <Grid.Column width={6}>
                    <AbstractTermDetails />
                </Grid.Column>
            </Grid>
            

        </div>
    )
})