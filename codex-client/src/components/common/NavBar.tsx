import React from "react";
import { Link, NavLink } from 'react-router-dom';
import { Container, Menu, Button, MenuItem, Image, Dropdown} from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import { observer } from 'mobx-react-lite';

export default observer(function NavBar()
{
    const {userStore: {user, logout}} = useStore();
    return (
        <Menu inverted fixed='top'>
            <Container >
                <Menu.Item as={Link} to='/feed' name="Content" header/>
                <Menu.Item as={Link} to='/user' name="Profile" header/>
            </Container>
        </Menu>
    )
})