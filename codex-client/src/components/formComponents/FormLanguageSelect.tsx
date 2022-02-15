import { Field } from "formik";
import React from "react";
import { IsoLangNames } from "../../app/common/langStrings";
import { DropdownField } from "./DropdownField";


interface Props {
    name: string,
    placeholder?: string
}

export default function FormLanguageSelect({name, placeholder}: Props) {
    const options = IsoLangNames.map(ln => {return {value: ln.iso, text: ln.fullName}})
    return (
        <Field
            placeholder={placeholder || "Select Language"}
            options={options}
            name={name}
            component={DropdownField}
        />
    )
}

/*
import * as React from "react";
import { Dropdown } from "semantic-ui-react";

export const DropdownField = ({
  field: { name, value },
  form: { touched, errors, setFieldValue },
  options,
  children: _,
  ...props
}: any) => {
  const errorText = touched[name] && errors[name];
  return (
    <Dropdown
      selection
      options={options}
      value={value}
      onChange={(_, { value }) => setFieldValue(name, value)}
      error={errorText}
      {...props}
    />
  );
};
*/