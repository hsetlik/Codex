import { observer } from "mobx-react-lite";
import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import TermDetails from "./TermDetails";


export default observer(function AbstractTermDetails() {
    const {contentStore} = useStore();
    const {selectedTerm} = contentStore;
    if (selectedTerm == null) {
        return <Header content="No Term Selected" />
    } else {
        return <TermDetails term={selectedTerm} />
    }
});