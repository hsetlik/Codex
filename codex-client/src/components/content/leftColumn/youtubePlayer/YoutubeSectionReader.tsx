import { observer } from "mobx-react-lite";
import React from "react";
import { useStore } from "../../../../app/stores/store";
import CaptionElementDiv from "./CaptionElementDiv";

export default observer(function YoutubeSectionReader() {
    /*TODO- this needs to: 
    1. get the current timecode on the player
    2. determine which captionElement needs to be focused 
    3. parent component needs to automatically load the correct SectionAbstractTerms for the timecode
    */
    const {contentStore} = useStore();
    const {currentSectionTerms} = contentStore;
    return (
        <div>
            {currentSectionTerms.elementGroups.map(cpt => (
                <CaptionElementDiv terms={cpt} key={cpt.index} />
            ))}
        </div>
    )
})