import { observer } from "mobx-react-lite";
import React from "react";
import { useStore } from "../../../app/stores/store";
import RatingButton from "./RatingButton";
import '../../styles/flex.css';


export default observer(function RatingButtonGroup() {
    const {contentStore} = useStore();
    const {selectedTerm} = contentStore;
    return(
        <div className="hflex-basic">
           <RatingButton term={selectedTerm!} ratingValue={1} key={1} />
           <RatingButton term={selectedTerm!} ratingValue={2} key={2} />
           <RatingButton term={selectedTerm!} ratingValue={3} key={3} />
           <RatingButton term={selectedTerm!} ratingValue={4} key={4} />
           <RatingButton term={selectedTerm!} ratingValue={5} key={5} />
        </div>
    )
})