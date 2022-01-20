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
}

export default observer(function CaptionRow({caption}: Props){
    const {videoStore} = useStore();
    const {currentTerms, highlightedCaption} = videoStore;
    const isHighlighted = caption === highlightedCaption;
    const terms = currentTerms.get(caption.captionText);
    if (!terms) {
        return <div></div>
    }
    const rowColor = (isHighlighted) ? CssPallette.PrimaryLight : CssPallette.Primary;
    return (
        <Row style={rowColor}>
            <Col>
                <Icon name='play circle' link />
            </Col>
            {terms.abstractTerms.map(trm => (
                <Col>
                    <AbstractTermComponent term={trm} tag='p' />
                </Col>
            ))}
        </Row>
    )
})