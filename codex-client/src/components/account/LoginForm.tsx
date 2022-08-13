import { FormikErrors, useFormik, } from "formik";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import CodexTextInput from "../formComponents/CodexTextInput";

interface LoginProps {
    email: string,
    password: string
}


export default observer(function LoginForm() {
    const store = useStore();
    const _validate = (values: LoginProps) => {
        let errors: FormikErrors<LoginProps> = {};
        if (!values.email)
            errors.email = "Not a valid email address"
        if (!values.password)
            errors.password = "Incorrect password"
    }
    const formik = useFormik<LoginProps>({
        initialValues: {
            email: '',
            password: ''
        },
        onSubmit(values, formikHelpers) {
            store.userStore.login({ email: values.email, password: values.password }).catch(pro => {
                console.log("Login failed");
            }).then(() => {
                //redirect to feed here
                console.log("login finished");
            })
        },
        validate: _validate
    });

    return (
        <div className="container">
            <form onSubmit={formik.handleSubmit} className="col input-group">
                <CodexTextInput name="email" />
                <CodexTextInput name="password" type="password" />
                <button type="submit">Submit</button>
            </form>

        </div>

    )
})