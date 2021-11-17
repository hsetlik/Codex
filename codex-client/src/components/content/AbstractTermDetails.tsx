import { observer } from "mobx-react-lite";
import { Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import TermDetails from "./TermDetails";
import UserTermDetails from "./UserTermDetails";


export default observer(function AbstractTermDetails() {
    const {transcriptStore} = useStore();
    const {selectedTerm} = transcriptStore;
    if (selectedTerm == null) {
        return <Header content="No Term Selected" />
    }
    else if (selectedTerm.hasUserTerm) {
        return <UserTermDetails term={selectedTerm} />
    } else {
        return <TermDetails term={selectedTerm} />
    }
});