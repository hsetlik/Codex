import React from "react";
import { Header, List } from "semantic-ui-react";
import { Phrase } from "../../../app/models/phrase";

interface Props {phrase: Phrase}

export default function PhraseDetails({phrase}: Props) {

    return (
        <div>
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