import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { Container } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { useStore } from "../../../../app/stores/store";
import CaptionDiv from "./CaptionDiv";
import YoutubePlayerDiv from "./YoutubePlayerDiv";

export default observer(function VideoRoute() {
    const {contentId} = useParams();
    const {contentStore} = useStore();
    const {selectedContentMetadata, selectContentWithId} = contentStore;
    useEffect(() => {
        if (selectedContentMetadata?.contentId !== contentId) {
            selectContentWithId(contentId!);
        }
    }, [selectedContentMetadata, selectContentWithId, contentId]);
    return (
        <Container>
            {(selectedContentMetadata !== null) && (
                <>
                    <YoutubePlayerDiv />
                    <CaptionDiv />
                </>
            )}
        </Container>
    )
})