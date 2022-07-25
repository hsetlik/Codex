import React, { Component, useState } from "react";
import { createRef } from "react";
import { Popup, PopupContent, PopupProps } from "semantic-ui-react";
import { ContentMetadata } from "../../app/models/content";
import AddTagButton from "./AddTagButton";
import AddTagForm from "./AddTagForm";

interface Props {content: ContentMetadata}
export default function AddTagPopup({content}: Props) {
    var ref = createRef<Component<PopupProps, any,any>>();
    const [open, setOpen] = useState(false);
    const handleClose = () => {
        setOpen(false);
    }
    return (
        <Popup
        openOnTriggerClick={true}
        openOnTriggerMouseEnter={false}
        closeOnTriggerMouseLeave={false}
        closeOnDocumentClick={true}
        ref={ref}
        trigger={
            <AddTagButton />
        }
        open={open}
        onOpen={(e, d) => {
            setOpen(true);
        }}
        >
            <PopupContent
            >
                <AddTagForm content={content} closePopup={handleClose} />
            </PopupContent>
        </Popup>
    )
}