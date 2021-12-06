import { observer } from "mobx-react-lite";
import React from "react";
import { NavLink } from "react-router-dom";
import { Menu, Button } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";
interface Props {
    username: string;
}

export default observer(function ProfileButtonBar({username}: Props) {
    const {userStore: {selectedLanguage, languageProfiles}} = useStore();
    return (
        <Menu>
            {languageProfiles.map(prof => (
                <Menu.Item 
                key={prof}
                >
                    <Button
                    as={NavLink}
                    to={`/profiles/${username}/${prof}`}
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