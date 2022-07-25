import { observer } from "mobx-react-lite";
import React from "react";
import { Flag, FlagNameValues, Menu } from "semantic-ui-react";
import { getFlagName } from "../../app/common/langStrings";
import { useStore } from "../../app/stores/store";

export default observer(function FlagLabel(){
    const {userStore: {selectedProfile}} = useStore();
    const flagName = getFlagName(selectedProfile?.language || 'en');
    return (
        <Menu.Item>
            <Flag name={flagName as FlagNameValues} />
        </Menu.Item>
    )

})