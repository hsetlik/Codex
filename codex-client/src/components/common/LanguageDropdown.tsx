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
    
    const defaultFunc = (input: string): void => {
        console.log(input);
    }
    const safeOnChange = props.onChange || defaultFunc; 
    const handleChange = (lang: string) => {
        safeOnChange(lang);
    }
    let dOptions: LangDropdownOption[] = [];
    props.options.forEach(o => dOptions.push(getDropdownProps(o)));
    return(
        <Dropdown
        defaultValue={props.defaultLanguage}
        selection
        options={dOptions}
        onChange={(e, d) => {
            handleChange(d.value as string);
        }}
        />
    )

}