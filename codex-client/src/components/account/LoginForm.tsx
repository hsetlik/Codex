import { ErrorMessage, Form, Formik, } from "formik";
import { Button, Header, Label } from "semantic-ui-react";
import MyTextInput from "../formComponents/MyTextInput"; 
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";


export default observer(function LoginForm() {
    const {userStore} = useStore();
    const navigate = useNavigate();
    return (
        <Formik
            initialValues={{email: '', password: '', error: null}}
            onSubmit={(
                (values, {setErrors}) =>  userStore.login(values).then(() => navigate('/feed'))
            .catch(error => setErrors({error: 'Invalid email or password'})))}
        >
            {({handleSubmit, isSubmitting, errors}) => (
                <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Login' color='teal' textAlign='center' />
                    <MyTextInput name='email' placeholder='Email' />
                    <MyTextInput name='password' placeholder='Password' type='password' />
                    <ErrorMessage name='error' render={() => (
                        <Label style={{marginBottom: 10}}  basic color='red' content={errors.error}/> )}
                    />
                    <Button loading={isSubmitting} positive content='Login' type='submit' fluid />
                </Form>
            )}
        </Formik>
    )
})