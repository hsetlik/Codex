
import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Loader } from "semantic-ui-react";
import { PhraseCreateQuery } from "../../../app/models/phrase";
import { useStore } from "../../../app/stores/store";

export default observer(function PhraseCreateButton() {
    const {termStore, contentStore} = useStore();
    const {selectedAbstractPhrase } = termStore;
    const currentQuery: PhraseCreateQuery = {
        language: selectedAbstractPhrase?.language || 'en',
        value: selectedAbstractPhrase?.value || 'null',
        firstTranslation: selectedAbstractPhrase?.reccomendedTranslation || 'null'
    }
    const handleClick = () => {contentStore.createPhrase(currentQuery)}
    return (
        (selectedAbstractPhrase === null) ? (
            <Loader active={true} />
        ) : (
            <Button onClick={handleClick}>
                {selectedAbstractPhrase.reccomendedTranslation}
            </Button>
        )
    )
})