import { observer } from "mobx-react-lite";
import React from "react";
import ReactPlayer from "react-player/youtube";
import { sectionMsRange } from "../../../../app/stores/contentStore";
import { useStore } from "../../../../app/stores/store";

export default observer(function YoutubePlayerDiv() {
    const {contentStore} = useStore();
    const {selectedContentUrl, highlightedElement, setHighlightedElement, elementAtMs, currentSection} = contentStore;
    const handleSeek = (seconds: number) => {

    }
    const handleProgress = (state: {
        played: number;
        playedSeconds: number;
        loaded: number;
        loadedSeconds: number; 
    }) => {
        const playedMs = state.playedSeconds * 1000;
        let current = elementAtMs(playedMs);
        if (highlightedElement !== current) {
            setHighlightedElement(current);
            console.log(`${current.contentUrl} at seconds ${current.startMs / 1000}`);
        }
        const range = sectionMsRange(currentSection!);
        if (range.start > playedMs || range.end <= playedMs) {
           //TODO: load the correct section for playedMs- make sure to use buffer if possible and handle loading time if not 
        }
    }
    return (
        <div>
           <ReactPlayer 
           url={selectedContentUrl} 
           controls={true}
           onSeek={handleSeek}
           onProgress={handleProgress}
           /> 

        </div>
    )
})