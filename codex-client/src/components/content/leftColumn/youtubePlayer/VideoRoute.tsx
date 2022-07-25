import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useStore } from "../../../../app/stores/store";
import SelectionDetails from "../../rightColumn/SelectionDetails";
import YoutubePlayerDiv from "./YoutubePlayerDiv";
import "../../../styles/video.css";

export default observer(function VideoRoute() {
    const {contentId} = useParams();
    const {termStore: {metadataLoaded, selectedContent, selectContentByIdAsync}, userStore: {selectedProfile, setSelectedLanguage}} = useStore();
    useEffect(() => {
        if (selectedContent.contentId !== contentId) {
            selectContentByIdAsync(contentId!);
        }
        if (selectedProfile?.language !== selectedContent.language) {
            setSelectedLanguage(selectedContent.language);
        }
    }, [contentId, selectedContent, selectContentByIdAsync, selectedProfile, setSelectedLanguage])
    
    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-9">
                <h1>{selectedContent.contentName}</h1>
                {(metadataLoaded) && (
                    <YoutubePlayerDiv />
                )}
                </div>
                <div className="col-3">
                    <SelectionDetails  />
                </div>
            </div>
           
        </div>
    )
})