import { observer } from "mobx-react-lite";
import React from "react";
import { Header, Loader } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import PhraseCreateButton from "./PhraseCreateButton";
import PhraseCreateForm from "./PhraseCreateForm";


export default observer(function PhraseCreator() {
    const {termStore} = useStore()
    const {selectedAbstractPhrase} = termStore;
    return ( (selectedAbstractPhrase === null) ? (
            <Loader active={true} />
        ) : (
           <div>
               <Header as='h2' content={selectedAbstractPhrase.value} />
               <PhraseCreateButton />
               <PhraseCreateForm />
           </div> 
        )
    )

})