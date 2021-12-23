import { observer } from "mobx-react-lite";
import React from "react";
import { Dropdown } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";

export default observer(function LanguageSelector() {
    const {userStore} = useStore();
    const {selectedProfile, languageProfiles, setSelectedProfile} = userStore;
    
    return (
    <Dropdown placeholder={getLanguageName(selectedProfile?.language!)} >
        
    </Dropdown>
    )
})