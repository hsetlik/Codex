import { observer } from "mobx-react-lite";
import React from "react";
import ReactPlayer from "react-player/youtube";
import { useStore } from "../../../../app/stores/store";

export default observer(function YoutubePlayerDiv() {
    const {contentStore} = useStore();
    const {selectedContentUrl} = contentStore;
    const handleSeek = (seconds: number) => {

    }
    const handleProgress = (state: {
        played: number;
        playedSeconds: number;
        loaded: number;
        loadedSeconds: number; 
    }) => {
        //TODO: this needs to check what the current caption should be and update it accordingly
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