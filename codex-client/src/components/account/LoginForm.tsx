import { useFormik, } from "formik";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import "../styles/forms.css";
import { useNavigate } from "react-router-dom";


interface LoginProps {
    email: string,
    password: string
}


export default observer(function LoginForm() {
    const store = useStore();
    const navigate = useNavigate();
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
                navigate(`/feed/${store.userStore.user?.lastStudiedLanguage}`);
            })
        }
      
    });

    return (
        <div className="container form-div bg-dark text-light">
            <form onSubmit={formik.handleSubmit} className="col input-group">
                <label
                className="h4" 
                htmlFor="email">Email:</label>
                <input
                name="email"
                id="email"
                type="text"
                onChange={formik.handleChange}
                value={formik.values.email}/>
                <label
                className="h4" 
                htmlFor="password">Password</label>
                <input
                name="password"
                id="password"
                type="password"
                onChange={formik.handleChange}
                value={formik.values.password}/>
                <button
                className="btn text-light bg-dark btn-outline-white"
                type="submit"
                >Submit</button>
            </form>
        </div>

    )
})