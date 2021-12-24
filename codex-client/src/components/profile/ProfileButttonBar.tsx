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
    const {profileStore: {selectedProfile,languageProfiles}} = useStore();
    return (
        <Menu>
            {languageProfiles.map(prof => (
                <Menu.Item 
                key={prof.languageProfileId}
                >
                    <Button
                    as={NavLink}
                    to={`/profiles/${username}/${prof.language}`}
                    active={prof === selectedProfile}
                    >
                        {getLanguageName(prof.language)}
                    </Button>
                </Menu.Item>
            ))

            }

        </Menu>
    )
})