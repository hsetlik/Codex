import { Grid, Item } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";
import { observer } from "mobx-react-lite";
import FeedHeader from "./FeedHeader";

export default observer(function FeedRoute(){
    var {contentStore} = useStore();
    //TODO: fix this to actually use the selected language from userStore
    console.log("Contents retreived");
    return (
        <Grid>
            <Grid.Column width='3'>

            </Grid.Column>
            <Grid.Column width='10'>
                <FeedHeader />
                <Item>
                    <Item.Group divided>
                    {contentStore.loadedHeaders.map(content => {
                        return <ContentHeader dto={content} key={content.contentName}/>
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