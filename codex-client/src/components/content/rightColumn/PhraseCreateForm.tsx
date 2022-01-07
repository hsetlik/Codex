import { Form, Formik, FormikProps } from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import { PhraseCreateQuery } from "../../../app/models/phrase";
import {Button, Form as SForm, Header} from 'semantic-ui-react';
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import CodexTextInput from "../../formComponents/CodexTextInput";



interface FormValues {
    firstTranslation: string,
}

const getPhraseValue = (terms: AbstractTerm[]): string => {
    let output = '';
    for(let i = 0; i < terms.length; ++i) {
        let term = terms[i];
        if (term.leadingCharacters.length > 0) {
            output += term.termValue;
        }
        output += term.termValue;
        if (term.trailingCharacters.length > 0) {
            output += term.trailingCharacters;
        }
        output += ' ';
    }
    return output;
}


export default observer(function PhraseCreateForm() {
const {contentStore, userStore: {selectedProfile}} = useStore();
const {phraseTerms, createPhrase} = contentStore;
const phraseValue = getPhraseValue(phraseTerms);
const handleFormSubmit = (firstTranslation: string) => {
    createPhrase({value: phraseValue, language: selectedProfile?.language || 'en', firstTranslation: firstTranslation});

}
return (
    <Formik
    initialValues={{firstTranslation: ' '}}
    onSubmit={(values) => handleFormSubmit(values.firstTranslation)}
    >
        {({handleSubmit, isSubmitting}) => (
            <Form className="ui form" onSubmit={handleSubmit}>
                <Header as='h2' content='Create phrase: ' />
                <CodexTextInput name='firstTranslation' placeholder="" />
                <Button content='Create phrase' loading={isSubmitting} />
            </Form>
        )}
    </Formik>
)

})