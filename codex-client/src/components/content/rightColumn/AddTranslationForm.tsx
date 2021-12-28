import { Formik, Form, ErrorMessage } from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Label } from "semantic-ui-react";
import { AddTranslationDto } from "../../../app/models/dtos";
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import MyTextInput from "../../formComponents/MyTextInput";
interface Props {
    term: AbstractTerm
}


export default observer(function AddTranslationForm({term}: Props) {
    const {contentStore} = useStore();
    const {addTranslation} = contentStore;
    //TODO: replace addTranslationToSelected- probably just move old functionality into contentStore.ts
    const handleFormSubmit = async (dto: AddTranslationDto) => {
       if (dto.userTermId === undefined) {
           console.log("Term is undefned!");
       } else {
           addTranslation(dto);
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
                        <Button loading={isSubmitting} positive content='Add Translation' type='submit' fluid />
                    </Form>
                )}
            </Formik>  
    )
});