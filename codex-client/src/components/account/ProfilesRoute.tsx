import { observer } from "mobx-react-lite";
import { Container } from "semantic-ui-react";
import { useParams } from "react-router";
import ProfileButttonBar from "./ProfileButttonBar";
import MetricView from "../profile/MetricView";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";

export default observer(function ProfilesRoute() {
    const {username, lang} = useParams();
    const {userStore: {setSelectedLanguage}} = useStore();
    useEffect(() => {
        setSelectedLanguage(lang!);
    }, [lang, setSelectedLanguage])
    return (
            <Container>
                <ProfileButttonBar username={username!} />
                <MetricView />
            </Container>
    );

})