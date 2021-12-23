import { observer } from "mobx-react-lite";
import { Menu } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { NavLink } from "react-router-dom";

export default observer(function AccountNavComponent() {
    const {userStore} = useStore();
    return (
        <Menu fluid>
            <Menu.Item as={NavLink} to='/account'>{userStore.user?.displayName}</Menu.Item>            
        </Menu>  
    )
});