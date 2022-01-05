import { withFormik, FormikProps, Form, Field, FormikErrors } from 'formik';
import { observer } from "mobx-react-lite";
import { Button } from "semantic-ui-react";
import { CreateCollectionQuery } from "../../app/models/collection";
import { useStore } from "../../app/stores/store";


interface FormValues {
    creatorUsername: string,
    isPrivate: boolean,
    language: string,
    description: string,
    collectionName: string,
    firstContentUrl: string,
    submitMethod: (query: CreateCollectionQuery) => void
}


  
const InnerForm = (props: FormikProps<FormValues>) => {
    const {touched, errors, isSubmitting} = props;
    return (
      <Form>
          <Field type='text' name='collectionName' />
          {touched.collectionName && errors.collectionName && <div>{errors.collectionName}</div>}
          <Field type='text' name='description' />
          {touched.description && errors.description && <div>{errors.description}</div>}
          <Field type='checkbox' name='isPrivate' />
          <Button type='submit' disabled={isSubmitting} />
      </Form>
    );
  };

interface FormProps {
    firstContentUrl: string,
    language: string,
    creatorUsername: string,
    submitMethod: (query: CreateCollectionQuery) => void 
}

const MiddleForm = withFormik<FormProps, FormValues>({
    mapPropsToValues: props => {
        return {
            collectionName: 'Name',
            description: 'Description',
            isPrivate: false,
            language: props.language,
            creatorUsername: props.creatorUsername,
            firstContentUrl: props.firstContentUrl,
            submitMethod: props.submitMethod
        }
    },
    validate: (values: FormValues) => {
        let errors: FormikErrors<FormValues> = {};
        if (values.collectionName.length < 2)
            errors.collectionName = 'Name must be at least 2 letters long!'
        return errors;
      },
    handleSubmit: values => {
        const createQuery: CreateCollectionQuery = {
            creatorUsername: values.creatorUsername,
            isPrivate: values.isPrivate,
            language: values.language,
            collectionName: values.collectionName,
            description: values.description,
            firstContentUrl: values.firstContentUrl
        }
        values.submitMethod(createQuery);
    },
})(InnerForm);



interface Props {contentUrl: string}
export default observer(function CollectionCreateForm({contentUrl}: Props) {
    const {userStore: {user, selectedProfile}, collectionStore: {createCollection}} = useStore();
return (
    <MiddleForm
     firstContentUrl={contentUrl}
     language={selectedProfile?.language!}
     creatorUsername={user?.username!}
     submitMethod={createCollection}
    /> )
});
  
