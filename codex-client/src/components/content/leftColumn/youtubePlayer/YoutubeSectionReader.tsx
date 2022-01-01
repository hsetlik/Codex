import { observer } from "mobx-react-lite";
import React from "react";
import { useStore } from "../../../../app/stores/store";
import CaptionElementDiv from "./CaptionElementDiv";

export default observer(function YoutubeSectionReader() {
   
    const {contentStore} = useStore();
    const {currentSectionTerms, highlightedElement} = contentStore;
    return (
        <div>
            {currentSectionTerms.elementGroups.map(cpt => (
                <CaptionElementDiv terms={cpt} key={cpt.index} isHighlighted={highlightedElement?.index === cpt.index} />
            ))}
        </div>
    )
})