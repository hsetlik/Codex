import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

interface Props {
    contentId: string
}

export default observer(function KnownWordsHeader({contentId}: Props) {
    const {knownWordsStore: {knownWords, loadKnownWordsFor}} = useStore();
    useEffect(() => {
        if (!knownWords.has(contentId)) {
            loadKnownWordsFor(contentId);
        }
    }, [knownWords, loadKnownWordsFor, contentId]);
    const knownWordsData = knownWords.get(contentId);
    return (
        <div>
            <Header as='h3'>
                {knownWordsData?.knownWords} of {knownWordsData?.totalWords} words known
            </Header>
        </div>
    )
})