import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { Loader } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import PhraseCreator from "./PhraseCreator";
import PhraseDetails from "./PhraseDetails";

export default observer(function AbstractPhraseDetails(){
    const {termStore} = useStore();
    const {selectedAbstractPhrase, updatePhraseAsync} = termStore;
    useEffect(() => {
        updatePhraseAsync();
    },[updatePhraseAsync])
    
    if (selectedAbstractPhrase === null) {
        return (
            <Loader active={true} />
        )
    }
    return ((selectedAbstractPhrase.hasPhrase && selectedAbstractPhrase.phrase) ? (
            <PhraseDetails phrase={selectedAbstractPhrase.phrase} />
        ) : (
            <PhraseCreator />
        ) 
    )
})