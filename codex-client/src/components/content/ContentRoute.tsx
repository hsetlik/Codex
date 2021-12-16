import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import SectionReader from "./reader/section/SectionReader";

export default observer(function ContentRoute() {
    const {contentId, index} = useParams();
    const {contentStore} = useStore();
    const {loadSectionById, currentSectionTerms} = contentStore;
    useEffect(() => {
        loadSectionById(contentId!, parseInt(index!));
    }, [loadSectionById, contentId, index]);

    return (
        <div>
            <SectionReader section={currentSectionTerms} />

        </div>
    )
})