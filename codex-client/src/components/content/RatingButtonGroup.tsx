import { observer } from "mobx-react-lite";
import React from "react";
import { Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import RatingButton from "./RatingButton";


export default observer(function RatingButtonGroup() {
    const {transcriptStore} = useStore();
    const {selectedTerm} = transcriptStore;
    return(
        <Button.Group>
           <RatingButton term={selectedTerm!} ratingValue={1} key={1} />
           <RatingButton term={selectedTerm!} ratingValue={2} key={2} />
           <RatingButton term={selectedTerm!} ratingValue={3} key={3} />
           <RatingButton term={selectedTerm!} ratingValue={4} key={4} />
           <RatingButton term={selectedTerm!} ratingValue={5} key={5} />
        </Button.Group>
    )
})