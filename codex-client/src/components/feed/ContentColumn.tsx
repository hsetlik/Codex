import { observer } from "mobx-react-lite";
import { Col, Container, Row } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { CssPallette } from "../../app/common/uiColors";
import { ContentMetadata } from "../../app/models/content";
import { useStore } from "../../app/stores/store";
import AddTagPopup from "../content/AddTagPopup";
import KnownWordsLabel from "../content/KnownWordsLabel";
import '../styles/feed.css';
import '../styles/flex.css';

interface Props {
    content: ContentMetadata
}
export default observer(function ContentColumn({content}: Props) {
    const navigate = useNavigate();
    const maxNameLength = 60;
    const name = (content.contentName.length > maxNameLength) ? 
    content.contentName.substring(0, maxNameLength - 3) + '...' : 
    content.contentName;
    const isVideo = content.contentType === 'Youtube';
    const contentPath = (isVideo) ? `/video/${content.contentId}` : `/viewer/${content.contentId}`;
    const {termStore} = useStore();
    const handleOpenClick = () => {
        navigate(contentPath);
    }
    const handleTagClick = (tag: string) => {
        navigate(`/tags/${tag}`)
    }

    return (
        <Container fluid className='feed-item-container' style={CssPallette.Surface}>
            <Row  className='header-row'>
                <Col>
                    <h3 style={CssPallette.Primary} className="title-text" onClick={handleOpenClick}>{name}</h3>
                    <label>{content.contentType}</label>
                    <div>
                        {content.creatorUsername && (
                            <label>{`Uploaded by: ${content.creatorUsername}`}</label>
                        )}
                    </div>
                    {content.description && (
                        <p className="description-p">{content.description}</p>
                    )}
                    <KnownWordsLabel contentId={content.contentId} />
                </Col>
            </Row>
            <Row>
                {content.contentTags &&  (
                    <Col className="tag-list">
                            <h3 className="feed-h3">
                                Tags:
                            </h3>
                       {content.contentTags.map(tag => (
                           <div key={tag}>
                                <button 
                                key={tag}
                                className="tag-button"
                                onClick={() => handleTagClick(tag)}
                                style={CssPallette.Secondary}
                                 >
                                    {tag}
                                </button>
                           </div>
                          
                       ))}
                       <AddTagPopup content={content} />
                    </Col>
                )}
            </Row> 
        </Container>
    )
})