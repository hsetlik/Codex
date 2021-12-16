import { observer } from "mobx-react-lite";
import { SectionAbstractTerms } from "../../../../app/models/content";
import TextElement from "../textElement/TextElement";

interface Props {
    section: SectionAbstractTerms
}

export default observer (function SectionReader({section}: Props){
    return (
        <div>
            {section.elementGroups.map(group => (<TextElement terms={group} key={group.index}/>)) }
        </div>
    )
})