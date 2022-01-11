import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import parse, { DOMNode } from 'html-react-parser';
import { useParams } from "react-router-dom";
import { Element } from 'domhandler/lib/node';
import '../styles/content-frame.css';
import { Loader } from "semantic-ui-react";



export default observer(function ContentFrame() {
    const {contentId} = useParams();
    const {htmlStore: {loadPage, currentHtml, htmlLoaded, currentPageContent}} = useStore();
    useEffect(() => {
        if (!htmlLoaded || currentPageContent === null || currentPageContent.contentId !== contentId) {
            loadPage(contentId!);
        }
    }, [ loadPage, htmlLoaded, contentId, currentPageContent]);
    const parser = (input: string) => {
        return parse(input, {
            replace: (node: DOMNode) => { 
                if(node instanceof Element && node.attributes) {
                    if(node.attribs.class === "mw-editsection")
                        return <div></div>
                }
            }
    });
    }
    if (currentHtml.length < 1 || !htmlLoaded) {
        return (
            <Loader active/>
        )
    }
    return (
        <div className="content-frame" >
            {parse(currentHtml) }
        </div>
    )

})