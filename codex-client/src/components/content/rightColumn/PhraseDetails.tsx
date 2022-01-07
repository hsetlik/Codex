import { observer } from "mobx-react-lite";
import React from "react";
import { useStore } from "../../../app/stores/store";

export default observer(function PhraseDetails(){
    const {contentStore} = useStore();
    const {phraseTerms} = contentStore;
    
    return (
        <div>

        </div>
    )
})