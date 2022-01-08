import { observer } from "mobx-react-lite";
import React from "react";
import { ContentMetadata } from "../../app/models/content";
import '../styles/flex.css';
import '../styles/content.css';
import { Label } from "semantic-ui-react";
import AddTagPopup from "./AddTagPopup";
import { Link } from "react-router-dom";

interface Props {content: ContentMetadata}
export default observer(function TagsList({content}: Props) {
    

    if (!content.contentTags) {
        return (
            <div></div>
        )
    }
    for(let t of content.contentTags) {
        console.log(`tag is: ${t}`);
    }
    return(
        <div className="hfex-basic" style={{marginLeft: 10}}  >
            {content.contentTags.map(tag => (
                <Label content={tag} className="tag-button" key={tag} as={Link} to={`../tags/${tag}`} />
            ))}
            <AddTagPopup content={content} />
        </div>
    )
})