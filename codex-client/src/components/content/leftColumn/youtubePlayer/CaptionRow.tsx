import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Col, Row } from "react-bootstrap";
import { Icon } from "semantic-ui-react";
import { CssPallette } from "../../../../app/common/uiColors";
import { VideoCaptionElement } from "../../../../app/models/content";
import { useStore } from "../../../../app/stores/store";
import AbstractTermComponent from "../commonReader/term/AbstractTermComponent";


interface Props {
    caption: VideoCaptionElement,
    onJump: (cap: VideoCaptionElement) => void
}

export default observer(function CaptionRow({caption, onJump}: Props){
    const {videoStore, termStore} = useStore();
    const {elementTermMap, loadElementAsync} = termStore;
    const {highlightedCaption} = videoStore;
    const isHighlighted = caption === highlightedCaption;
    useEffect(() => {
        if (!elementTermMap.has(caption.captionText))
            loadElementAsync(caption.captionText, 'caption');
    }, [elementTermMap, caption, loadElementAsync]);
    const terms = elementTermMap.get(caption.captionText);
    if (!terms) {
        console.log(`No terms for caption: ${caption.captionText}`);
        return <div></div>
    }
    const rowStyle = (isHighlighted) ? CssPallette.SecondaryLight : CssPallette.Surface;
    const handleClick = () => { onJump(caption) }
    return (
        <Row style={rowStyle}>
            <Col>
                <Icon name='play circle' link onClick={handleClick} />
                {terms.abstractTerms.map(trm => (
                        <AbstractTermComponent term={trm} tag='p' key={trm.indexInChunk} />
                ))}
            </Col>
        </Row>
    )
})