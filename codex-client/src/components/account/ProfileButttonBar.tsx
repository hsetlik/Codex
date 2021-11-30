import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Menu, Button } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";


export default observer(function ProfileButtonBar() {
    const {userStore: {selectedLanguage, languageProfiles, user, setSelectedLanguage}} = useStore();
    return (
        <Menu>
            {languageProfiles.map(prof => (
                <Menu.Item 
                key={prof}
                >
                    <Button
                    as={Link} to={`/profiles/${user?.username}/${selectedLanguage}`}
                    onClick={() => setSelectedLanguage(prof)}
                    active={selectedLanguage === prof}
                    >
                        {getLanguageName(prof)}
                    </Button>
                  
                </Menu.Item>
            ))

            }

        </Menu>
    )
})