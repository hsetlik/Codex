import { observer } from "mobx-react-lite";
import React from "react";
import ReactPlayer from "react-player";
import { useStore } from "../../../../app/stores/store";

export default observer(function YoutubePlayerDiv() {
    const {contentStore} = useStore();
    const {selectedContentUrl} = contentStore;
    return (
        <div>
           <ReactPlayer url={selectedContentUrl} controls={true} /> 
        </div>
    )
})