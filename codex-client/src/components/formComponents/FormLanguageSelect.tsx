import { useField } from "formik";
import React from "react";
import { Form } from "semantic-ui-react";
import { IsoLangNames } from "../../app/common/langStrings";


interface Props {
    name: string
}

export default function FormLanguageSelect({name}: Props) {
    const [field, meta] = useField(name);
    return (
        <Form.Field
            control="dropdown"
            name={name}
        >
            {IsoLangNames.map(lang => (
                <option value={lang.iso}>{lang.fullName}</option>
            ))}
        </Form.Field>
    )
}