import { Link } from 'react-router-dom';
import { Container, Icon, Menu} from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import { observer } from 'mobx-react-lite';
import { getLanguageName } from '../../app/common/langStrings';
import '../styles/styles.css';

export default observer(function NavBar()
{
    const {userStore: {user, logout, isLoggedIn, selectedProfile}} = useStore();
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
        <Menu inverted fixed='top' className='codex-nav-bar'>
            <Container >
                <Menu.Item as={Link} to={`feed/${selectedProfile?.language}`} name={`${getLanguageName(selectedProfile?.language!)}`} header/>
                {accountComponent}
                <Menu.Item name="Logout" onClick={logout} />
                <Menu.Item as={Link} to='/'>
                    <Icon name='home' />
                </Menu.Item>
            </Container>
        </Menu>
    )
})