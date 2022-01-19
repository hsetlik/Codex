import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import { useEffect } from "react";
import { Container, Row } from "react-bootstrap";
import ContentColumn from "./ContentColumn";
import '../styles/feed.css';

export default observer(function FeedRoute(){
    const {lang} = useParams();
    const language = lang || 'null';
    const {feedStore, userStore: {selectedProfile, setSelectedLanguage}} = useStore();
    const {feedLoaded, currentFeed, loadFeed} = feedStore;
    useEffect(() => {
        if (selectedProfile?.language !== language) {
            setSelectedLanguage(language);
        } else if (!feedLoaded || currentFeed?.languageProfileId !== selectedProfile.languageProfileId) {
            loadFeed(selectedProfile.languageProfileId);
        }
    }, [
        language, 
        selectedProfile, 
        setSelectedLanguage,
        feedLoaded,
        loadFeed,
        currentFeed]);
    
    if (!feedLoaded || currentFeed === null) {
        return (
            <Container>
            </Container>
        )
    }
    
    return (
        <Container fluid className='feed-container'>
            { currentFeed.rows.map(row => (
                <Row className='feed-row'>
                    {row.contents.map(con => (
                        <ContentColumn content={con} />
                    ))}
                </Row>
            ))
            }
        </Container>
    )
})