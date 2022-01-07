import { observer } from "mobx-react-lite";
import React from "react";
import { Loader } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import PhraseCreator from "./PhraseCreator";
import PhraseDetails from "./PhraseDetails";

export default observer(function AbstractPhraseDetails(){
    const {contentStore} = useStore();
    const {currentAbstractPhrase} = contentStore;
    
    if (currentAbstractPhrase === null) {
        return (
            <Loader active={true} />
        )
    }
    return ((currentAbstractPhrase.hasPhrase) ? (
            <PhraseDetails phrase={currentAbstractPhrase.phrase} />
        ) : (
            <PhraseCreator />
        ) 
    )
})