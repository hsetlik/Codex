import { observer } from "mobx-react-lite";
import React from "react";
import { SectionAbstractTerms } from "../../../../app/models/content";

interface Props {
   terms: SectionAbstractTerms
}

export default observer(function YoutubeSectionReader({terms}: Props) {
    /*TODO- this needs to: 
    1. get the current timecode on the player
    2. determine which captionElement needs to be focused 
    3. parent component needs to automatically load the correct SectionAbstractTerms for the timecode
    */
    return (
        <div>

        </div>
    )
})