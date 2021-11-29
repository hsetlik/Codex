import { Formik, Form, ErrorMessage } from "formik";
import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { Header, Segment, Button, Label } from "semantic-ui-react";
import { AddTranslationDto, UserTermCreateDto } from "../../../app/api/agent";
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import MyTextInput from "../../formComponents/MyTextInput";
interface Props {
    term: AbstractTerm
}


export default observer(function AddTranslationForm({term}: Props) {
    const {userStore, transcriptStore} = useStore();
    const {selectedTerm, addTranslationToSelected} = transcriptStore;
    const handleFormSubmit = async (dto: AddTranslationDto) => {
       if (dto.userTermId === undefined) {
           console.log("Term is undefned!");
       } else {
           await addTranslationToSelected(dto.newTranslation);
       }
    }
    return(
            <Formik
            initialValues={{userTermId: term?.userTermId!, newTranslation: '', error: null}}
            onSubmit={((values, {setErrors}) => handleFormSubmit(values).catch(error => setErrors(error)))}
            >
                {({handleSubmit, isSubmitting, errors}) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='newTranslation' placeholder='New Translation' />
                        <ErrorMessage name='error' render={() => (
                            <Label style={{marginBottom: 10}}  basic color='red' content={errors.error}/> )}
                        />
                        <Button loading={isSubmitting} positive content='Add Term' type='submit' fluid />
                    </Form>
                )}
            </Formik>  
    )
});