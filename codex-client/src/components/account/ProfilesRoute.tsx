import { observer } from "mobx-react-lite";
import { Container } from "semantic-ui-react";
import { useParams } from "react-router";
import LanguageProfilePage from "./LanguageProfilePage";
import ProfileButttonBar from "./ProfileButttonBar";

export default observer(function ProfilesRoute() {
    const {username, lang} = useParams();
    return (
            <Container>
                <ProfileButttonBar username={username!} />
                <LanguageProfilePage language={lang!} username={username!} />
            </Container>
    );

})