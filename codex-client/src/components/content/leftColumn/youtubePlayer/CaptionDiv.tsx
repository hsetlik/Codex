import { observer } from "mobx-react-lite";
import React from "react";
import { Container } from "react-bootstrap";
import { VideoCaptionElement } from "../../../../app/models/content";
import { useStore } from "../../../../app/stores/store";
import CaptionRow from "./CaptionRow";


interface Props {
    handleJump: (cap: VideoCaptionElement) => void;
}

export default observer(function CaptionDiv({handleJump}: Props){
    const {videoStore} = useStore();
    const {currentCaptions, currentCaptionsLoaded} = videoStore;
    if (!currentCaptionsLoaded) {
        return <div></div>
    }
    return (
        <Container>
            {currentCaptions.map(cpt => (
                <CaptionRow caption={cpt} onJump={handleJump} key={cpt.captionText}/>
            ))}
        </Container>
    )
})