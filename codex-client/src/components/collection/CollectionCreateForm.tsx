import { withFormik, FormikProps, Form, FormikErrors } from 'formik';
import { observer } from "mobx-react-lite";
import { Button, Label } from "semantic-ui-react";
import { CreateCollectionQuery } from "../../app/models/collection";
import { useStore } from "../../app/stores/store";
import CodexTextInput from '../formComponents/CodexTextInput';
import { Form as FormComp} from "semantic-ui-react";
import "../styles/styles.css";


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
          <CodexTextInput type='text' name='collectionName' className='codex-text-input' />
          {touched.collectionName && errors.collectionName && <Label>{errors.collectionName}</Label>}
          <CodexTextInput type='text' name='description' className='codex-text-input' />
          {touched.description && errors.description && <div>{errors.description}</div>}
          <FormComp.Checkbox type='checkbox' name='isPrivate' label="Private"  />
          <Button type='submit' disabled={isSubmitting} content='Create' />
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
  
