import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { Container, Segment } from "semantic-ui-react";
import { useParams } from "react-router";
import LanguageProfilePage from "./LanguageProfilePage";
import ProfileButttonBar from "./ProfileButttonBar";

export default observer(function ProfilesRoute() {
    const {username, lang} = useParams();
    return (
            <Container>
                <ProfileButttonBar />
                <LanguageProfilePage language={lang!} />
            </Container>
    );

})