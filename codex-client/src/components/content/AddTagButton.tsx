import React from "react";
import { Label } from "semantic-ui-react";
import '../styles/content.css';
import styled from 'styled-components';

const AddLabel = styled.label`
    font-size: small;
    font-family: 'Lato';
    font-weight: bold;
    padding: 0.45em;
    text-align: center;
    cursor: pointer;
    background-color: rgb(66, 64, 64);
    color: rgb(241, 240, 240);
    padding-top: 0.25em;
    padding-bottom: 0.25em;
    border-radius: 0.4em;
    &:hover{
        padding: 0.50em;
        background-color: rgb(122, 121, 121);
    }
`;

export default function AddTagButton() {

    return (
        <AddLabel >Add Tag +</AddLabel>
    )
}