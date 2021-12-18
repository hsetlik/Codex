import { Grid, Item, Loader } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";
import { observer } from "mobx-react-lite";
import FeedHeader from "./FeedHeader";
import { useParams } from "react-router-dom";
import { useEffect } from "react";

export default observer(function FeedRoute(){
    const {lang} = useParams();
    var {contentStore, commonStore} = useStore();
    const {loadedContents: loadedHeaders, loadMetadata } = contentStore;
    const {appLoaded} = commonStore;
    useEffect(() => {
        loadMetadata(lang!);
    }, [loadMetadata, lang])
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
                <FeedHeader />
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