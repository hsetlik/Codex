import { observer } from "mobx-react-lite";
import React from "react";
import { SavedContentDto } from "../../app/models/dtos";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";

interface Props {
    dto: SavedContentDto
}
export default observer(function SavedContentHeader({dto}: Props) {
    const {feedStore: {allContents}} = useStore();
    const metadata = allContents.find(c => c.contentUrl === dto.contentUrl);
    if (!metadata) {
        return (
            <div>
            </div>
        )
    }
    return (
        <ContentHeader dto={metadata} />
    )
})