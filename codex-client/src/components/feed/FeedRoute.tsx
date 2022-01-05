import { Grid, Header, Item, Loader } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import { useEffect } from "react";
import { getLanguageName } from "../../app/common/langStrings";

export default observer(function FeedRoute(){
    const {lang} = useParams();
    var {contentStore, commonStore, collectionStore} = useStore();
    const {loadedContents: loadedHeaders, loadMetadata } = contentStore;
    const {currentCollectionsLanguage, loadCollectionsForLanguage} = collectionStore;
    const {appLoaded} = commonStore;
    useEffect(() => {
        loadMetadata(lang!);
        if (lang !== currentCollectionsLanguage)
            loadCollectionsForLanguage(lang!);
    }, [loadMetadata, lang, currentCollectionsLanguage, loadCollectionsForLanguage])
    if (!appLoaded) {
        return (
            <Loader />
        )
    }
    return (
        <Grid>
            <Grid.Column width='3'>

            </Grid.Column>
            <Grid.Column width='10'>
            <Header as='h2' content={getLanguageName(lang!)} />
                <Item>
                    <Item.Group divided>
                    {loadedHeaders.map(content => {
                        return <ContentHeader dto={content} key={content.contentUrl}/>
                        })
                    }
                    </Item.Group>
                </Item>
            </Grid.Column>
            <Grid.Column width='3'>
                
            </Grid.Column>
            
       </Grid>
    )
})