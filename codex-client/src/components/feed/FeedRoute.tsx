import { Grid, Item, Loader } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";
import { observer } from "mobx-react-lite";
import FeedHeader from "./FeedHeader";
import { useParams } from "react-router";
import { useEffect } from "react";

export default observer(function FeedRoute(){
    var {contentStore, commonStore} = useStore();
    const {loadedContents: loadedHeaders, loadMetadata: loadHeaders, headersLoaded} = contentStore;
    const {appLoaded} = commonStore;

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