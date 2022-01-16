import React from "react";
import { useParams } from "react-router-dom";
import { Col, Container, Row } from "react-bootstrap";
import ContentFrame from "./ContentFrame";
import '../styles/content.css';
import SelectionDetails from "../content/rightColumn/SelectionDetails";

export default function ViewerRoute() {
    const {contentId} = useParams();
    const safeId = contentId || 'null';  


    return (
        <Container >
            <Row>
                <Col xs={9}>
                    <ContentFrame contentId={safeId} />
                </Col>
                <Col xs={3} >
                    <SelectionDetails />
                </Col>
            </Row>
        </Container>
    )
}