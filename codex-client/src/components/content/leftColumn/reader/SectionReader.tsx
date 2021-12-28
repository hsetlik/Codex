import { observer } from "mobx-react-lite";
import { Container, Loader } from "semantic-ui-react";
import { SectionAbstractTerms } from "../../../../app/models/content";
import { useStore } from "../../../../app/stores/store";
import TextElement from "../commonReader/textElement/TextElement";


interface Props {
    section: SectionAbstractTerms
}

export default observer (function SectionReader({section}: Props){
    const {contentStore: {sectionLoaded}} = useStore();
    if (!sectionLoaded) {
        return (
            <Container>
                <Loader active={!sectionLoaded} />
            </Container>
        )
    }
    return (
        <Container>
            {section.elementGroups.map(group => (<TextElement terms={group} key={group.index}/>)) }
        </Container>
    )
})