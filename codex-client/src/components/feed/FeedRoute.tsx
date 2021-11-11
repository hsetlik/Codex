import { Grid, Item } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";
import { observer } from "mobx-react-lite";
import FeedHeader from "./FeedHeader";

export default observer(function FeedRoute(){
    var {contentStore} = useStore();
    //TODO: fix this to actually use the selected language from userStore
    if (contentStore.loadedHeaders.length < 1) {
        contentStore.loadHeaders({language: "ru"});
    }
    var contents = contentStore.loadedHeaders;
    console.log("Contents retreived");
    for(const c in contents)
    {
        console.log(c);
    }
    return (
        <Grid>
            <Grid.Column width='3'>

            </Grid.Column>
            <Grid.Column width='10'>
                <FeedHeader />
                <Item>
                    <Item.Group divided>
                    {contents.map(content => {
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