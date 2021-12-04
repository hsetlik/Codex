import {useEffect} from 'react';
import './App.css';
import { useStore } from './app/stores/store'
import { Route, Routes } from 'react-router-dom';
import { Container } from 'semantic-ui-react';
import NavBar from './components/common/NavBar';
import ContentRoute from './components/content/termDetails/ContentRoute';
import FeedRoute from './components/feed/FeedRoute';
import LoginForm from './components/account/LoginForm';
import ProfilesRoute from './components/account/ProfilesRoute';
import HomeRoute from './components/common/HomeRoute';
import RegisterForm from './components/account/RegisterForm';

function App() {
//const location = useLocation();
  const {commonStore, userStore} = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally((() => {
        userStore.selectDefaultLanguage();
        commonStore.setAppLoaded();
      }));
      commonStore.setAppLoaded();
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  return (
    <Container>
      <NavBar />
      <Container style={{margin: '7em'}}>
        <Routes >
          <Route path='/feed/:lang' element={<FeedRoute />}/>
          <Route path='/home' element={<HomeRoute />} />
          <Route path='/content/:contentId/:index'element={<ContentRoute />}/>
          <Route path='/profiles/:username/:lang' element={<ProfilesRoute />}/>
          <Route path='account/login' element={<LoginForm />} /> 
          <Route path='account/register' element={<RegisterForm />} /> 
        </Routes>
      </Container>
   </Container>
  );
}

export default App;
