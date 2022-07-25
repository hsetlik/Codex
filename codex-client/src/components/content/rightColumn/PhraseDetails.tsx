import React from "react";
import { Header, List } from "semantic-ui-react";
import { Phrase } from "../../../app/models/phrase";
import "../../styles/details.css";

interface Props {phrase: Phrase}

export default function PhraseDetails({phrase}: Props) {

    return (
        <div className="details-div">
            <Header as='h2' content={phrase.value} />
            <List>
                {phrase.translations.map(t => (
                    <List.Item key={t} >
                        {t}
                    </List.Item>
                ))}
            </List>
        </div>
    )
}