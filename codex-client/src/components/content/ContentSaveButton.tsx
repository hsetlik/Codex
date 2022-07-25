import { observer } from "mobx-react-lite";
import React from "react";
import { Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

interface Props {
    contentUrl: string
}

export default observer(function ContentSaveButton({contentUrl}: Props) {
    const {contentStore: {toggleContentSaved, savedContents}} = useStore();

    const handleClick = () => {
        toggleContentSaved(contentUrl);
    }

    const isSaved = savedContents.some(c => c.contentUrl === contentUrl);
    const text = (isSaved) ? 'Unsave' : 'Save';

    return (
        <Button className="label" onClick={handleClick} content={text} />
    )
})