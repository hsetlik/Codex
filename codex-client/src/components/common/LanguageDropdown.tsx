import { useState } from "react";
import { Dropdown } from "semantic-ui-react";
import { flagCodes, getLanguageName } from "../../app/common/langStrings";
import '../styles/flex.css';

interface LangDropdownOption {
    key: string,
    text: string,
    flag: string,
    value: string
}

const getDropdownProps = (lang: string): LangDropdownOption => {
    return {
        key: lang,
        text: getLanguageName(lang),
        flag: flagCodes.find(f => f.lang === lang)?.flag || 'us',
        value: lang
    }
}


interface LanguageDropdownProps {
    options: string[],
    onChange?: (language: string) => void,
    defaultLanguage?: string
}

export default function LanguageDropdown(props: LanguageDropdownProps) {
    const [language, setLanguage] = useState(props.defaultLanguage || 'ru');
    const defaultFunc = (input: string): void => {
        console.log(input);
    }
    const safeOnChange = props.onChange || defaultFunc; 
    const handleChange = (lang: string) => {
        setLanguage(lang);
        safeOnChange(lang);
    }
    let dOptions: LangDropdownOption[] = [];
    props.options.forEach(o => dOptions.push(getDropdownProps(o)));
    return(
        <Dropdown text={getLanguageName(language)}>
            <Dropdown.Menu>
                {dOptions.map(opt => (
                    <Dropdown.Item {...opt} onClick={() => handleChange(opt.value)} />
                ))}
            </Dropdown.Menu>
        </Dropdown>
    )

}