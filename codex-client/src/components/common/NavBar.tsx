import { Link } from 'react-router-dom';
import { Container, Icon, Menu} from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import { observer } from 'mobx-react-lite';
import '../styles/styles.css';
import LanguageSelector from '../account/ProfileSelector';
import FlagLabel from './FlagLabel';
import { CssPallette } from '../../app/common/uiColors';

export default observer(function NavBar()
{
    const {userStore: {user, logout, isLoggedIn, selectedProfile}} = useStore();
    const lang = selectedProfile?.language!; 
    let accountComponent;
    if (isLoggedIn) {
        accountComponent = (
            <Menu.Item as={Link} to={`/profiles/${user?.username}/${lang}`} name={user?.displayName} style={{'position': ''}} />
        )
    } else {
        accountComponent = (
            <Menu.Item as={Link} to='/account/login' content='Login' />
        )
    }
    return (
        <Menu inverted fixed='top' className='codex-nav-bar' style={CssPallette.PrimaryDark}>
            <Container >
                <Menu.Item as={Link} to={`feed/${lang}`}>
                    <Icon name='home' />
                </Menu.Item>
                <FlagLabel />
                <Menu.Item>
                    <LanguageSelector />
                </Menu.Item>
                <Menu.Item as={Link} to={`/collections/${lang}`} >
                    Browse
                </Menu.Item>
                {accountComponent}
                <Menu.Item name="Logout" onClick={logout} />
            </Container>
        </Menu>
    )
})