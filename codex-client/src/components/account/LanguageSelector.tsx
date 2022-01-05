import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router-dom";
import { Dropdown, ListContent } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { LanguageProfileDto } from "../../app/models/dtos";
import { useStore } from "../../app/stores/store";
import LanguageFlag1x1 from "../common/LanguageFlag1x1";
import '../styles/styles.css';




export default observer(function LanguageSelector() {
    const {userStore} = useStore();
    const navigate = useNavigate();
    const {selectedProfile, languageProfiles, setSelectedLanguage} = userStore;
    const handleChange = (profile: LanguageProfileDto) => {
        setSelectedLanguage(profile.language);
        navigate(`/feed/${profile.language}`);
    }
    return (
    <Dropdown value={getLanguageName(selectedProfile?.language!)} placeholder={getLanguageName(selectedProfile?.language!)} >
        <Dropdown.Menu>
            { languageProfiles.map(prof => (
                <Dropdown.Item key={prof.languageProfileId} onClick={() => handleChange(prof)} >
                    <ListContent>
                        {getLanguageName(prof.language)}
                        <LanguageFlag1x1 lang={prof.language} className="flag-square" />
                    </ListContent>
                </Dropdown.Item>
            ))
            }
        </Dropdown.Menu>
    </Dropdown>
    )
})