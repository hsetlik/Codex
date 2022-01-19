import { observer } from "mobx-react-lite";
import { Col } from "react-bootstrap";
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

    return (
        <Col className='feed-item-container'>
            <h1
            onClick={handleOpenClick}
            className="feed-h1"
            >{name}</h1>
                {(content.contentTags && content.contentTags.length > 0) && (
                <div className="feed-subdiv">
                    {(content.contentTags && content.contentTags.length > 0) && (<h3 className="feed-h3">Tags:</h3>)}
                    {content.contentTags && content.contentTags.map(tag => (
                        <button className="tag-button" onClick={() => navigate(`/tags/${tag}`)}>
                            {tag}
                        </button>
                    ))}
                </div>
                )}
                {content.description && (
                <div className="feed-subdiv">
                    <h3 className='feed-h3'>Description:</h3>
                    <p className='description-p'>{content.description}</p>
               </div>
            )}
        </Col>
    )
})