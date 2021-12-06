import { observer } from "mobx-react-lite";
import React from "react";
import { Header } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";

interface Props {
    language: string;
    username: string;
}

export default observer(function LanguageProfilePage({language, username}: Props) {
    const {userStore} = useStore();
    const {user} = userStore;
    return(
        <div>
            <Header content={`${user?.displayName}'s ${getLanguageName(language)} profile`} />

        </div>
    )
})