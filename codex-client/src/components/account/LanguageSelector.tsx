import { observer } from "mobx-react-lite";
import React from "react";
import { useNavigate } from "react-router-dom";
import { Dropdown } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { LanguageProfileDto } from "../../app/models/dtos";
import { useStore } from "../../app/stores/store";



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
                <Dropdown.Item key={prof.languageProfileId} onClick={() => handleChange(prof)}>
                    {getLanguageName(prof.language)}
                </Dropdown.Item>
            ))
            }
        </Dropdown.Menu>
    </Dropdown>
    )
})