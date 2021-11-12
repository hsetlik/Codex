import { Grid, Item } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";
import { observer } from "mobx-react-lite";
import FeedHeader from "./FeedHeader";
import { useEffect } from "react";

export default observer(function FeedRoute(){
    var {contentStore, userStore} = useStore();
    console.log("Contents retreived");
    useEffect(() => {
        contentStore.loadHeaders(userStore.selectedLanguage);
    }, [contentStore, userStore])
    return (
        <Grid>
            <Grid.Column width='3'>

            </Grid.Column>
            <Grid.Column width='10'>
                <FeedHeader />
                <Item>
                    <Item.Group divided>
                    {contentStore.loadedHeaders.map(content => {
                        return <ContentHeader dto={content} key={content.contentId}/>
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