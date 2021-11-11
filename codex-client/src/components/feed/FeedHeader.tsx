import React from "react";
import { Container, Dropdown, Header, Item, Label, Segment } from "semantic-ui-react";
import getLanguageName from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";

export default function FeedHeader(){
    const {userStore} = useStore();
    const langProfiles = userStore.languageProfiles;
    console.log(langProfiles.length + " profiles found");
    return(
            <Segment>
                <Header as="h3">Content Feed</Header>
                <Dropdown selection={"en"} >
                    <Dropdown.Menu >
                    {
                        langProfiles.map(lang => {
                            <Dropdown.Item  text={getLanguageName(lang)} />
                        })
                    }
                    </Dropdown.Menu>
                </Dropdown>
            </Segment>
    )
}