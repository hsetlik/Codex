import { Formik, Form, ErrorMessage } from "formik";
import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { Header, Segment, Button, Label } from "semantic-ui-react";
import { UserTermCreateDto } from "../../../app/api/agent";
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import MyTextInput from "../../formComponents/MyTextInput";
interface Props {
    term: AbstractTerm
}


export default observer(function UserTermCreator({term}: Props) {
    const {userStore, contentStore} = useStore();
    const {selectedTerm} = contentStore;
    const {createTerm} = userStore;
    const handleFormSubmit = async (dto: UserTermCreateDto) => {
        if (dto.termValue !== selectedTerm?.termValue) {
            console.log(`Warning! submitted term ${dto.termValue} does not match selected term ${selectedTerm?.termValue}`);
            dto.termValue = selectedTerm?.termValue!;
            dto.language = selectedTerm?.language!;
            console.log(`Submitting term is now: ${dto.termValue}`);
        }
        await createTerm(dto);
    }
    return(
            <Formik
            initialValues={{language: selectedTerm?.language!, termValue: selectedTerm?.termValue!, firstTranslation: '', error: null}}
            onSubmit={((values, {setErrors}) => handleFormSubmit(values).catch(error => setErrors(error)))}
            >
                {({handleSubmit, isSubmitting, errors}) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <Header as='h2' content='Create term: ' />
                        <MyTextInput name='firstTranslation' placeholder='First Translation' />
                        <ErrorMessage name='error' render={() => (
                            <Label style={{marginBottom: 10}}  basic color='red' content={errors.error}/> )}
                        />
                        <Button loading={isSubmitting} positive content='Add Term' type='submit' fluid />
                    </Form>
                )}
            </Formik>  
    )
});