
import { Segment } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import LanguageSelector from "../account/LanguageSelector";
interface Props {
    lang: string
}

export default observer(function FeedHeader({lang}: Props){
    return(
            <Segment>
                <LanguageSelector />
            </Segment>
    )
})