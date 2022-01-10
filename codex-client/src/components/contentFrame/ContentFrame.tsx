import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import parse from 'html-react-parser';
import { useParams } from "react-router-dom";



export default observer(function ContentFrame() {
    const {contentId} = useParams();
    const {htmlStore: {loadPage, currentHtml, htmlLoaded, currentPageContent}} = useStore();
    useEffect(() => {
        if (!htmlLoaded || currentPageContent === null || currentPageContent.contentId !== contentId) {
            loadPage(contentId!);
        }
    }, [ loadPage, htmlLoaded, contentId]);
    return (
        <div>
            { parse(currentHtml)}
        </div>
    )

})