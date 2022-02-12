import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { Col, Container, Row } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { useStore } from "../../../../app/stores/store";
import SelectionDetails from "../../rightColumn/SelectionDetails";
import YoutubePlayerDiv from "./YoutubePlayerDiv";

export default observer(function VideoRoute() {
    const {contentId} = useParams();
    const {termStore: {metadataLoaded, selectedContent, selectContentByIdAsync}} = useStore();
    useEffect(() => {
        if (selectedContent.contentId !== contentId) {
            selectContentByIdAsync(contentId!);
        }
    }, [contentId, selectedContent, selectContentByIdAsync])
    
    return (
        <Container>
            <Row>
                <Col xs={9}>
                {(metadataLoaded) && (
                    <YoutubePlayerDiv />
                )}
                </Col>
                <Col xs={3}>
                    <span style={{position: "fixed"}}>
                        <SelectionDetails  />
                    </span>
                </Col>
            </Row>
           
        </Container>
    )
})