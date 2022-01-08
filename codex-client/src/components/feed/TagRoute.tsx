import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Grid, Header, Item, Loader } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ContentHeader from "../content/ContentHeader";

export default observer(function TagsRoute() {
    const {tag} = useParams();
    const tagValue = tag || 'null';
    const {tagStore} = useStore();
    const {currentTag, loadTag, tagContents, tagContentsLoaded} = tagStore;
    useEffect(() => {
        if (!tagContentsLoaded || currentTag?.tagValue !== tagValue) {
            loadTag(tagValue);
        }

    }, [tagContentsLoaded, currentTag, loadTag, tagValue])
    if (!tagContentsLoaded) {
        return (
            <Loader active={true} />
        )
    }
    return (
        <Grid>
            <Grid.Column width='3'>
            </Grid.Column>
            <Grid.Column width='10'>
            <Header as='h2' content={`Contents with tag '${tagValue}':`} />
                <Item>
                    <Item.Group divided>
                    {tagContents.map(content => {
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
/*
 <div>
            <Header content={`Contents with tag: '${tagValue}'`} />
            {tagContents.map(con => (
                <ContentHeader dto={con} key={con.contentId} />
            ))}
        </div> */