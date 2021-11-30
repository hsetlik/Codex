import { observer } from "mobx-react-lite";
import React from "react";
import { Header } from "semantic-ui-react";
import { getLanguageName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";

interface Props {
    language: string;
}

export default observer(function LanguageProfilePage({language}: Props) {
    const {userStore: {user, languageProfiles}} = useStore();
    return(
        <div>
            <Header content={`${user?.displayName}'s ${getLanguageName(language)} profile`} />

        </div>
    )
})