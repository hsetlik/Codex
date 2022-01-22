import { observer } from "mobx-react-lite";
import React from "react";
import { Col, Row } from "react-bootstrap";
import { Icon } from "semantic-ui-react";
import { CssPallette } from "../../../../app/common/uiColors";
import { ElementAbstractTerms, VideoCaptionElement } from "../../../../app/models/content";
import { useStore } from "../../../../app/stores/store";
import AbstractTermComponent from "../commonReader/term/AbstractTermComponent";


interface Props {
    caption: VideoCaptionElement,
    onJump: (cap: VideoCaptionElement) => void
}

export default observer(function CaptionRow({caption, onJump}: Props){
    const {videoStore} = useStore();
    const {currentTerms, highlightedCaption} = videoStore;
    const isHighlighted = caption === highlightedCaption;
    const terms = currentTerms.get(caption.captionText);
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