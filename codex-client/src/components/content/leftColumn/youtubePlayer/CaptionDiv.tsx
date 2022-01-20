import { observer } from "mobx-react-lite";
import React from "react";
import { Container } from "react-bootstrap";
import { useStore } from "../../../../app/stores/store";
import CaptionRow from "./CaptionRow";


export default observer(function CaptionDiv(){
    const {videoStore} = useStore();
    const {currentCaptions, currentCaptionsLoaded} = videoStore;
    if (!currentCaptionsLoaded) {
        return <div></div>
    }
    return (
        <Container>
            {currentCaptions.map(cpt => (
                <CaptionRow caption={cpt}/>
            ))}
        </Container>
    )
})