import { observer } from "mobx-react-lite";
import React from "react";
import { Header, Loader } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import PhraseCreateButton from "./PhraseCreateButton";
import PhraseCreateForm from "./PhraseCreateForm";


export default observer(function PhraseCreator() {
    const {contentStore: {currentAbstractPhrase}} = useStore();
    return ( (currentAbstractPhrase === null) ? (
            <Loader active={true} />
        ) : (
           <div>
               <Header as='h2' content={currentAbstractPhrase.value} />
               <PhraseCreateButton />
               <PhraseCreateForm />
           </div> 
        )
    )

})