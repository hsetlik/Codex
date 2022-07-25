import { Col, Row } from "react-bootstrap";
import { FeedRow } from "../../app/models/feed";
import ContentColumn from "./ContentColumn";
import '../styles/feed.css';

interface Props {
    row: FeedRow
}
export default function FeedRowComponent({row}: Props) {
    return (
        <Row>
            {row.contents.length > 0 && row.contents.map(con =>( 
                <Col key={con.contentId} >
                    <ContentColumn content={con}/>
                </Col>
            ))}
        </Row>
    )
}