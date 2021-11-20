import { Formik, Form, ErrorMessage } from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import { Header, Segment, Button, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import MyTextInput from "../formComponents/MyTextInput";

export default observer(function UserTermCreator() {
    const {userStore, transcriptStore} = useStore();
    const {selectedTerm} = transcriptStore;
    return(
            <Formik
            initialValues={{language: selectedTerm?.language!, termValue: selectedTerm?.termValue!, firstTranslation: '', error: null}}
            onSubmit={(
                (values, {setErrors}) =>  userStore.createTerm(values).finally(async () => {
                       await transcriptStore.refreshTerm(selectedTerm?.indexInChunk!);
                }).catch(error => setErrors({error: 'not a valid term'})))}
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