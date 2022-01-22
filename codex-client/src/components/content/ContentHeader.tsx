import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Header, Label, Segment } from "semantic-ui-react";
import { ContentMetadata } from "../../app/models/content";
import { useStore } from "../../app/stores/store";
import AddToCollection from "../common/AddToCollection";
import ContentSaveButton from "./ContentSaveButton";
import KnownWordsLabel from "./KnownWordsLabel";
import TagsList from "./TagsList";

interface Props{
    dto: ContentMetadata
}

export default observer(function ContentHeader({dto}: Props)
{
    const {termStore} = useStore();
    console.log("Content ID is: " + dto.contentUrl);
    return (
            <Segment>
                <Header >{dto.contentName}</Header>
                <div style={{padding: '10px'}}>
                    <Label >{dto.contentType}</Label>
                    <Label >Section {dto.bookmark + 1} of {dto.numSections}</Label>
                    <KnownWordsLabel contentId={dto.contentId} />
                    <ContentSaveButton contentUrl={dto.contentUrl} />
                </div>
                <TagsList content={dto} />
                <div style={{padding: '10px'}}>
                    <Button as={Link}
                    color='twitter' 
                    to={`../viewer/${dto.contentId}/`} 
                    onClick={() => termStore.selectContentByIdAsync(dto.contentId)}>
                        View
                    </Button>
                    <AddToCollection content={dto} />
                </div>
            </Segment>
    )
})