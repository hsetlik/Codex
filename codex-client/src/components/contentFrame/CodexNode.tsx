import { DOMNode, Text } from "html-react-parser";
import { Element } from 'domhandler/lib/node';
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import TextElement from "../content/leftColumn/commonReader/textElement/TextElement";
import '../styles/content.css';
import { Loader } from "semantic-ui-react";


interface NodeProps {
    sourceNode: DOMNode,
    className?: string
}

export default observer(function CodexNode({sourceNode, className}: NodeProps) {
    // return an empty div if this isn't a valid element
    const {htmlStore: {currentElementsMap, loadElementTerms, currentPageContent}} = useStore();
    let child = (sourceNode as Element).children[0] as Text;
    let text = child.data;
    useEffect(() => {
        if (!currentElementsMap.has(text) && sourceNode instanceof Element && text) {
            loadElementTerms({elementText: text, tag: sourceNode.tagName, contentUrl: currentPageContent?.contentUrl || 'null'})

        }
    }, [ currentElementsMap, loadElementTerms, text, currentPageContent, sourceNode])
  
    if (!(sourceNode instanceof Element) || !currentElementsMap.has(text)) {
        return (
            <div>
                <Loader active={true} />
            </div>
        )
    }
   
    switch (sourceNode.tagName) {
        case 'p':
            
            return (
                <p className={className || 'codex-element-p'} {...sourceNode.attributes}>
                   <TextElement terms={currentElementsMap.get(text)!} />
                </p>
            );
        case 'h1':
            return (
                <h1 className={className || 'codex-element-h1'}>
                    <TextElement terms={currentElementsMap.get(text)!} />
                </h1>
            )
        case 'h2':
            return (
                <h2 className={className || 'codex-element-h2'}>
                    <TextElement terms={currentElementsMap.get(text)!} />
                </h2>
            )
        case 'h3':
            return (
                <h3 className={className || 'codex-element-h3'}>
                    <TextElement terms={currentElementsMap.get(text)!} />
                </h3>
            )
        case 'a':
            return (
                <span className={className || ''}>
                    <TextElement terms={currentElementsMap.get(text)!} />
                </span>
            )
        default:
            console.log(`Node with tag ${sourceNode.tagName} not rendered!`);
            break;
    }
    return (
        <>

        </>
    )
})