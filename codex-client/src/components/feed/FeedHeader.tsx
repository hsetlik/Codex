
import { Dropdown, Header, Segment } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";

export default observer(function FeedHeader(){
    const {userStore} = useStore();
    const {languageProfiles} = userStore;
    const navigate = useNavigate();
    var idx = 0;
    return(
            <Segment>
                <Header as="h3">Content Feed</Header>
                <Dropdown text={getLanguageName(userStore.selectedLanguage)} >
                    <Dropdown.Menu >
                    {
                        languageProfiles.map(lang => {
                            ++idx;
                            return (<Dropdown.Item  
                                text={getLanguageName(lang)} 
                                key={lang + userStore.user?.displayName + idx}
                                onClick={() => {
                                    userStore.setSelectedLanguage(lang);
                                    navigate(`/feed/${lang}`);
                                }}
                                 />);
                        })
                    }
                    </Dropdown.Menu>
                </Dropdown>
            </Segment>
    )
})