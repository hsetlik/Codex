import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { List } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import VocabWord from "../content/rightColumn/VocabWord";

interface Props {
    lang: string
}

export default observer(function ProfileVocabList({lang}: Props) {
    const {profileStore} = useStore();
    const {userTermsLoaded, currentUserTerms, loadProfile, currentLanguage} = profileStore;
    useEffect(() => {
        if (!userTermsLoaded || lang !== currentLanguage) {
            loadProfile(lang);
        }
    }, [userTermsLoaded, lang, currentLanguage, loadProfile]);
    return (
            <List>
                {currentUserTerms.map(term => {
                    return( 
                    <List.Item key={term.userTermId}>
                        <VocabWord term={term} />
                    </List.Item>)
                })
                }
            </List>
    );
})