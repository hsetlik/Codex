import React from "react";
import { Container, Label } from "semantic-ui-react";
import { UserTerm } from "../../app/models/userTerm";

function returnColorFor(term: UserTerm){

}



export default function ReaderUserTerm(term: UserTerm){
    return(
        <Label >
            {term.termValue}
        </Label>
        
    )
}