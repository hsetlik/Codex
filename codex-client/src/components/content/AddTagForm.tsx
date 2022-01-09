import { ContentMetadata } from "../../app/models/content";
import { Button, Header, Input} from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { useState } from "react";



interface Props {
    content: ContentMetadata,
    closePopup?: () => void
}
export default observer(function AddTagForm({content, closePopup}: Props) {
    const {userStore, contentStore: {addContentTag}} = useStore();
    const tagLanguage = userStore.user?.nativeLanguage || 'en';
    const handleAdd = (value: string) => {
        addContentTag({tagValue: value, tagLanguage: tagLanguage, contentId: content.contentId});
        if (closePopup)
            closePopup();
    }
    const [currentText, setCurrentText] = useState('');
    return (
        <div>
            <Header content='New Tag:' as='h3' />
            <Input onChange={(e, d) => setCurrentText(d.value)} style={{'margin-bottom': 10}} />
            <Button size='mini' content='Add' onClick={() => handleAdd(currentText)} />
        </div>
    )
})