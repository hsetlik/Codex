import { observer } from "mobx-react-lite";
import { Container, Loader } from "semantic-ui-react";
import { useParams } from "react-router";
import MetricView from "../profile/MetricView";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import SavedContentsList from "./SavedContentsList";

export default observer(function ProfilesRoute() {
    const {lang} = useParams();
    const {userStore: {setSelectedLanguage, selectedProfile}} = useStore();
    useEffect(() => {
        setSelectedLanguage(lang!);
        console.log(selectedProfile);
    }, [lang, setSelectedLanguage, selectedProfile])
    return (
            <Container>
                {(selectedProfile?.languageProfileId) ? (
                    <MetricView profileId={selectedProfile?.languageProfileId!} />
                ) : (
                    <Loader active />
                )}
                <SavedContentsList />
            </Container>
    );

})