import { observer } from "mobx-react-lite";
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
    const {elementTermMap: elements} = termStore;
    const {highlightedCaption} = videoStore;
    const isHighlighted = caption === highlightedCaption;
    const terms = elements.get(caption.captionText);
    if (!terms) {
        return <div></div>
    }
    const rowStyle = (isHighlighted) ? CssPallette.SecondaryLight : CssPallette.Surface;
    return (
        <Row style={rowStyle}>
            <Col>
                <Icon name='play circle' link onClick={() => onJump(caption)} />
                {terms.abstractTerms.map(trm => (
                        <AbstractTermComponent term={trm} tag='p' key={trm.indexInChunk} />
                    
                ))}
            </Col>
        </Row>
    )
})