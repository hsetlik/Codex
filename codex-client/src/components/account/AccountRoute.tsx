import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { Container, Segment } from "semantic-ui-react";

export default observer(function AccountRoute() {
    const { userStore} = useStore();
    return (
        <>
            <Container>
                <Segment>
                    {userStore.user?.displayName}
                </Segment>

            </Container>
        </>
    );

})