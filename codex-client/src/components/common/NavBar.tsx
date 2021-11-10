import { Link } from 'react-router-dom';
import { Button, Container, Menu} from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import { observer } from 'mobx-react-lite';
import AccountNav from '../account/AccountNav';
import UserStore from '../../app/stores/userStore';

export default observer(function NavBar()
{
    const {userStore: {user, logout, isLoggedIn}} = useStore();
    return (
        <Menu inverted fixed='top'>
            <Container >
                <Menu.Item as={Link} to='/feed' name="Content" header/>
                <Menu.Item as={Link} to='account/login' name="Login" position='right'/>
                <Menu.Item >
                    <Button onClick={logout}>Logout</Button>
                </Menu.Item>
            </Container>
        </Menu>
    )
})