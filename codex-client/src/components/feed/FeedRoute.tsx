import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect } from "react";
import { Container } from "react-bootstrap";
import '../styles/feed.css';
import FeedRowComponent from "./FeedRowComponent";
import { FeedTypeNames } from "../../app/models/feed";

export default observer(function FeedRoute(){
    const {lang} = useParams();
    const language = lang || 'null';
    const {feedStore, userStore: {selectedProfile, setSelectedLanguage}} = useStore();
    // if we don't have a lang param, we need to navigate to the correct language
    const navigate = useNavigate();
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
        <Container fluid>
                { currentFeed.rows.map(row => (
                    <div key={currentFeed.rows.indexOf(row) || 0}>
                        <h2>{FeedTypeNames.find(n => n.value === row.feedType)?.display}</h2>
                        <FeedRowComponent row={row} />
                    </div>
                ))
                }
        </Container>
    )
})