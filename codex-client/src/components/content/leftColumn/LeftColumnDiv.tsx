import { observer } from "mobx-react-lite";
import React from "react";
import { ContentType, getContentType } from "../../../app/models/content";
import { useStore } from "../../../app/stores/store";
import SectionNavigator from "./reader/SectionNavigator";
import SectionReader from "./reader/SectionReader";
import YoutubePlayerDiv from "./youtubePlayer/YoutubePlayerDiv";
import YoutubeSectionReader from "./youtubePlayer/YoutubeSectionReader";

interface Props {
    contentId: string,
    index: number
}


export default observer(function LeftColumnDiv({contentId, index}: Props) {
    const {contentStore} = useStore();
    const {selectedContentMetadata, currentSectionTerms} = contentStore;
    const contentType = getContentType(selectedContentMetadata?.contentUrl!);
    if (contentType === ContentType.Wikipedia || contentType === ContentType.Article) {
        return (
            <div>
                <SectionNavigator contentId={contentId!} currentIndex={index} />
                <SectionReader section={currentSectionTerms} />
            </div>               
        )
    } else { //TODO: youtube viewer goes here
        return (
            <div>
                <YoutubePlayerDiv />
                <YoutubeSectionReader />
            </div>
        )
    }
})