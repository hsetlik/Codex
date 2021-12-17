import { observer } from "mobx-react-lite";
import { Loader } from "semantic-ui-react";
import { SectionAbstractTerms } from "../../../../app/models/content";
import { useStore } from "../../../../app/stores/store";
import TextElement from "../textElement/TextElement";

interface Props {
    section: SectionAbstractTerms
}

export default observer (function SectionReader({section}: Props){
    const {contentStore: {sectionLoaded}} = useStore();
    if (!sectionLoaded) {
        return (
            <div>
                <Loader />
            </div>
        )
    }
    return (
        <div>
            {section.elementGroups.map(group => (<TextElement terms={group} key={group.index}/>)) }
        </div>
    )
})