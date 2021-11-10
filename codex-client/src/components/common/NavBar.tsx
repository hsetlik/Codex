import { Link } from 'react-router-dom';
import { Container, Menu} from 'semantic-ui-react';
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
                <Menu.Item as={Link} to='account/login' name="Login" position='right'/>
            </Container>
        </Menu>
    )
})