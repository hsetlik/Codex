import { observer } from "mobx-react-lite";
import React from "react";
import ReactPlayer from "react-player/youtube";
import { useStore } from "../../../../app/stores/store";

export default observer(function YoutubePlayerDiv() {
    const {contentStore} = useStore();
    const {selectedContentUrl, highlightedElement, setHighlightedElement, elementAtSeconds} = contentStore;
    const handleSeek = (seconds: number) => {

    }
    const handleProgress = (state: {
        played: number;
        playedSeconds: number;
        loaded: number;
        loadedSeconds: number; 
    }) => {
        let current = elementAtSeconds(state.playedSeconds);
        if (highlightedElement !== current) {
            setHighlightedElement(current);
            console.log(`${current.contentUrl} at seconds ${current.startSeconds}`);
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