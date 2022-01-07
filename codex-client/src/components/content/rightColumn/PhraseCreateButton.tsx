
import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Loader } from "semantic-ui-react";
import { PhraseCreateQuery } from "../../../app/models/phrase";
import { useStore } from "../../../app/stores/store";

export default observer(function PhraseCreateButton() {
    const {contentStore} = useStore();
    const {currentAbstractPhrase, createPhrase} = contentStore;
    const currentQuery: PhraseCreateQuery = {
        language: currentAbstractPhrase?.language || 'en',
        value: currentAbstractPhrase?.value || 'null',
        firstTranslation: currentAbstractPhrase?.reccomendedTranslation || 'null'
    }
    const handleClick = () => {createPhrase(currentQuery)}
    return (
        (currentAbstractPhrase === null) ? (
            <Loader active={true} />
        ) : (
            <Button onClick={handleClick}>
                {currentAbstractPhrase.reccomendedTranslation}
            </Button>
        )
    )
})