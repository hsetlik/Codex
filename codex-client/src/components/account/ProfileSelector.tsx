import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import LanguageDropdown from "../common/LanguageDropdown";
import '../styles/styles.css';




export default observer(function ProfileSelector() {
    const {userStore} = useStore();
    const navigate = useNavigate();
    const {languageProfiles, setSelectedLanguage} = userStore;
    const handleChange = (language: string) => {
        setSelectedLanguage(language);
        navigate(`/feed/${language}`);
    }
    let options: string[] = [];
    for(var p of languageProfiles) {
        options.push(p.language);
    }
    return (
    <LanguageDropdown options={options} onChange={handleChange} /> 
    )
})