import React from "react";
import { useParams } from "react-router";
import { Container } from "semantic-ui-react";
import TranscriptReader from "./TranscriptReader";

export default function ContentRoute(){
    const {id} = useParams();
    return(
        <TranscriptReader />
    )
}