import { observer } from "mobx-react-lite";
import { Col, Container, Row } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { ContentMetadata } from "../../app/models/content";
import '../styles/feed.css';
import '../styles/flex.css';

interface Props {
    content: ContentMetadata
}
export default observer(function ContentColumn({content}: Props) {
    const navigate = useNavigate();
    const maxNameLength = 30;
    const name = (content.contentName.length > maxNameLength) ? 
    content.contentName.substring(0, maxNameLength - 3) + '...' : 
    content.contentName;
    const isVideo = content.contentUrl.startsWith('https://youtube');
    const contentPath = (isVideo) ? `/content/${content.contentId}/0` : `/viewer/${content.contentId}`;
    const handleOpenClick = () => {
        navigate(contentPath);
    }
    const handleTagClick = (tag: string) => {
        navigate(`/tags/${tag}`);
    }

    return (
        <Container fluid className='feed-item-container'>
            <Row>
                <h3 className="title-text" onClick={handleOpenClick}>{name}</h3>
            </Row>
            <Row>
                {content.contentTags && content.contentTags.length > 0 && (
                    <Col className="tag-list">
                        <h3 className="feed-h3">
                            Tags:
                        </h3>
                       {content.contentTags.map(tag => (
                           <div>
                                <button 
                                className="tag-button"
                                onClick={() => handleTagClick(tag)}
                                 >
                                    {tag}
                                </button>
                           </div>
                          
                       ))}
                    </Col>
                )}
            </Row> 
        </Container>
    )
})