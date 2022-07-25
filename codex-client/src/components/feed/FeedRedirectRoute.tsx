import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useStore } from "../../app/stores/store";


export default observer(function FeedRedirectRoute() {

    const {userStore: {selectedProfile, isLoggedIn}} = useStore();
    const navigate = useNavigate();

    useEffect(() => {
        if (isLoggedIn) {
            navigate(`/feed/${selectedProfile?.language}`);
        }

    }, [isLoggedIn, selectedProfile, navigate])
    return (
        <div>

        </div>
    )
})