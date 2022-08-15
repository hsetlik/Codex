import { ErrorMessage, Form, Formik, } from "formik";
import { Button, Header } from "semantic-ui-react";
import CodexTextInput from "../formComponents/CodexTextInput";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import * as Yup from 'yup';
import ValidationErrors from "../errors/ValidationErrors";
import FormLanguageSelect from "../formComponents/FormLanguageSelect";
import { useNavigate } from "react-router-dom";

export default observer(function RegisterForm() {
    const {userStore} = useStore();
    const navigate = useNavigate();
    return (
        <Formik
            initialValues={{
                email: '', 
                password: '', 
                error: null, 
                displayName: '', 
                username: '',
                nativeLanguage: 'en',
                studyLanguage: ''
            }}

            onSubmit={(values, {setErrors}) => {
                userStore.register(values).then(() => {
                    navigate(`/feed/${values.studyLanguage}`);
                }).catch(error => {
                    setErrors({error});
                });
            }} 
            
            validationSchema={Yup.object({
                displayName: Yup.string().required(),
                username: Yup.string().required(),
                email: Yup.string().required().email(),
                password: Yup.string().required(),
                nativeLanguage: Yup.string().required(),
                studyLanguage: Yup.string().required()
            })}
        >
            {({handleSubmit, isSubmitting, errors, isValid, dirty}) => (
                <Form className='ui form error' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Sign up to Reactivities' color='teal' textAlign='center' />
                    <CodexTextInput name='displayName' placeholder='Display Name' />
                    <div>
                        <FormLanguageSelect name="nativeLanguage" placeholder="Native Language" />
                    </div>
                    <div>
                        <FormLanguageSelect name="studyLanguage" placeholder="Study Language" />
                    </div>
                    <CodexTextInput name='username' placeholder='Username' />
                    <CodexTextInput name='email' placeholder='Email' />
                    <CodexTextInput name='password' placeholder='Password' type='password' />
                    <ErrorMessage name='error' render={() => (
                        <ValidationErrors errors={errors.error} />
                        )}
                    />
                    <Button disabled={!isValid || !dirty || isSubmitting} loading={isSubmitting} 
                    positive content='Register' 
                    type='submit' fluid
                     />
                </Form>
            )}
        </Formik>
    )
})