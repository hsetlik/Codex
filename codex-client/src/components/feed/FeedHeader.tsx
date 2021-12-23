
import { Dropdown, Header, Segment } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";
import LanguageSelector from "../account/LanguageSelector";

export default observer(function FeedHeader(){
    return(
            <Segment>
                <LanguageSelector />
            </Segment>
    )
})