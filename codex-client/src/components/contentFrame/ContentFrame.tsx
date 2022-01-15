import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import parse, { DOMNode } from 'html-react-parser';
import { useParams } from "react-router-dom";
import { Element } from 'domhandler/lib/node';
import '../styles/content-frame.css';
import { Loader } from "semantic-ui-react";
import CodexNode from "./CodexNode";


export default observer(function ContentFrame() {
    const {contentId} = useParams();
    const {htmlStore: {loadPage, currentPageHtml, htmlLoaded, currentPageContent}} = useStore();
    useEffect(() => {
        if (!htmlLoaded || currentPageContent === null || currentPageContent.contentId !== contentId) {
            loadPage(contentId!);
        }
    }, [ loadPage, htmlLoaded, contentId, currentPageContent]);
    var nodeIdx = 1;
    const parser = (input: string) => {
        return parse(input, {
            replace: (node: DOMNode) => { 
                if(node instanceof Element && node.attributes) {
                    ++nodeIdx;
                    if(node.attribs.class === "mw-editsection")
                        return <div></div>
                    if (node.attribs.codex_replacable === 'true' && node.children.some(c => c.type === 'text')) {
                        return <CodexNode sourceNode={node} key={`elementNode${nodeIdx}`} />
                    }
                }
            }
    });
    }
    if (currentPageHtml === null || !htmlLoaded) {
        return (
            <Loader active/>
        )
    }
    return (
        <div className="content-frame" >
            {currentPageHtml.stylesheetUrls.map(url => (
                <link rel='stylesheet' href={url} typeof="text/css" key={url} />
            ))}
            {parser(currentPageHtml.html)}
        </div>
    )

})