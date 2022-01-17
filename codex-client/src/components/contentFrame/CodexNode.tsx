import { DOMNode, Text } from "html-react-parser";
import { Element } from 'domhandler/lib/node';
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import TextElement from "../content/leftColumn/commonReader/textElement/TextElement";
import '../styles/content.css';
import { ElementType } from "htmlparser2";
import { useInView } from "react-intersection-observer";


interface NodeProps {
    sourceNode: DOMNode,
    className?: string
}

const fullInnerText = (node: DOMNode & Element): string => {
    var text = '';
    for(let child of node.children) {
        if (child.type === ElementType.Text)
            text += (child as Text).data;
        else if(child instanceof Element) {
            text += fullInnerText(child);
        }
    }
    return text;
}
export default observer(function CodexNode({sourceNode, className}: NodeProps) {
    const {ref, inView} = useInView({threshold: 0.2});
    // return an empty div if this isn't a valid element
    let text = fullInnerText(sourceNode as Element);
    const {htmlStore: {currentElementsMap, loadElementTerms, currentPageContent}} = useStore();
    useEffect(() => {
        if (!currentElementsMap.has(text) && sourceNode instanceof Element && text.length > 1 && inView) {
            loadElementTerms({elementText: text, tag: sourceNode.tagName, contentUrl: currentPageContent?.contentUrl || 'null'})

        }
    }, [ currentElementsMap, loadElementTerms, text, currentPageContent, sourceNode, inView])
    if (!(sourceNode instanceof Element)) {
        return (<></>);
    }
   const contentNode = (sourceNode instanceof Element && currentElementsMap.has(text)) ? 
    (<TextElement terms={currentElementsMap.get(text)!} />) : 
    (<></>); 
    
    switch (sourceNode.tagName) {
        case 'p' || 'b':
            
            return (
                <div className={className || 'codex-element-p'} {...sourceNode.attributes} ref={ref}>
                    {contentNode}
                </div>
            );
        case 'h1':
            return (
                <h1 className={className || 'codex-element-h1'}ref={ref}>
                    {contentNode}
                </h1>
            )
        case 'h2':
            return (
                <h2 className={className || 'codex-element-h2'}ref={ref}>
                    {contentNode}
                </h2>
            )
        case 'h3':
            return (
                <h3 className={className || 'codex-element-h3'}ref={ref}>
                    {contentNode}
                </h3>
            )
        case 'a':
            return (
                <div className={className || 'codex-element-div'} ref={ref}>
                    {contentNode}
                </div>
            )
        case 'div':
            return (
                <div className={className || 'codex-element-div'}ref={ref}>
                    {contentNode}
                </div>
            )
        case 'span':
            return (
                <span className={className || 'codex-element-span'}ref={ref}>
                    {contentNode}
                </span>
            )
        case 'li':
            return (
                <span className={className || 'codex-element-span'}ref={ref}>
                    {contentNode}
                </span>
            )
        default:
            console.log(`Node with tag ${sourceNode.tagName} not rendered!`);
            return (
                <div className={className || 'codex-element-div'}ref={ref}>
                    {contentNode}
                </div>
            )
    }
})