import { Dropdown, Header, Segment } from "semantic-ui-react";
import getLanguageName from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";

export default function FeedHeader(){
    const {userStore} = useStore();
    const langProfiles = userStore.languageProfiles;
    var idx = 0;
    return(
            <Segment>
                <Header as="h3">Content Feed</Header>
                <Dropdown  >
                    <Dropdown.Menu >
                    {
                        langProfiles.map(lang => {
                            ++idx;
                            return (<Dropdown.Item  text={getLanguageName(lang)} key={lang + userStore.user?.displayName + idx} />);
                        })
                    }
                    </Dropdown.Menu>
                </Dropdown>
            </Segment>
    )
}