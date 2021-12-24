import { Link } from 'react-router-dom';
import { Container, Menu} from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import { observer } from 'mobx-react-lite';

export default observer(function NavBar()
{
    const {userStore: {user, logout, isLoggedIn}, profileStore: {selectedProfile}} = useStore();
    let accountComponent;
    if (isLoggedIn) {
        accountComponent = (
            <Menu.Item as={Link} to={`/profiles/${user?.username}/${selectedProfile?.language}`} name={user?.displayName} />
        )
    } else {
        accountComponent = (
            <Menu.Item as={Link} to='/account/login' content='Login' />
        )
    }
    return (
        <Menu inverted fixed='top'>
            <Container >
                <Menu.Item as={Link} to={`feed/${selectedProfile?.language}`} name="Content" header/>
                {accountComponent}
                <Menu.Item name="Logout" onClick={logout} />
            </Container>
        </Menu>
    )
})