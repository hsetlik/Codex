
import { Dropdown, Header, Segment } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function FeedHeader(){
    const {userStore} = useStore();
    const langProfiles = userStore.languageProfiles;
    var idx = 0;
    return(
            <Segment>
                <Header as="h3">Content Feed</Header>
                <Dropdown text={getLanguageName(userStore.selectedLanguage)}>
                    <Dropdown.Menu >
                    {
                        langProfiles.map(lang => {
                            ++idx;
                            return (<Dropdown.Item  
                                text={getLanguageName(lang)} 
                                key={lang + userStore.user?.displayName + idx}
                                onClick={() => userStore.setSelectedLanguage(lang)} />);
                        })
                    }
                    </Dropdown.Menu>
                </Dropdown>
            </Segment>
    )
})