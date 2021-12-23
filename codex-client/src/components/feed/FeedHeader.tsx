
import { Segment } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import LanguageSelector from "../account/LanguageSelector";

export default observer(function FeedHeader(){
    return(
            <Segment>
                <LanguageSelector />
            </Segment>
    )
})