import { observer } from "mobx-react-lite";
import React from "react";
import { useParams } from "react-router-dom";
import { useStore } from "../../../../app/stores/store";

export default observer(function VideoRoute() {
    const {contentId} = useParams();
    const {contentStore} = useStore();
    const {selectedContentMetadata, setSelectedContent, } = contentStore;
    return (
        <div>

        </div>
    )
})