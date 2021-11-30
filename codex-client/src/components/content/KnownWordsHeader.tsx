import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

interface Props {
    contentId: string
}

export default observer(function KnownWordsHeader({contentId}: Props) {
    const {contentStore} = useStore();
    const {headerKnownWords} = contentStore;
    return (
        <div>
            { headerKnownWords.get(contentId) !== undefined ? (
                <Header as='h4' >
                    {`${headerKnownWords.get(contentId)?.knownWords} known of ${headerKnownWords.get(contentId)?.totalWords} total words`}
                </Header>
            ) : (
                <Header as='h4'>
                    Loading...
                </Header>
            )
            }
        </div>
    )
})