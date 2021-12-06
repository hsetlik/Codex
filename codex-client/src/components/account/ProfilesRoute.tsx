import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { Container } from "semantic-ui-react";
import { useParams } from "react-router";
import LanguageProfilePage from "./LanguageProfilePage";
import ProfileButttonBar from "./ProfileButttonBar";

export default observer(function ProfilesRoute() {
    const {username, lang} = useParams();
    const {userStore} = useStore();
    return (
            <Container>
                <ProfileButttonBar username={username!} />
                <LanguageProfilePage language={lang!} username={username!} />
            </Container>
    );

})