import {useEffect} from 'react';
import './App.css';
import { useStore } from './app/stores/store'
import { Route, Routes } from 'react-router-dom';
import { Container } from 'semantic-ui-react';
import NavBar from './components/common/NavBar';
import FeedRoute from './components/feed/FeedRoute';
import LoginForm from './components/account/LoginForm';
import HomeRoute from './components/common/HomeRoute';
import RegisterForm from './components/account/RegisterForm';
import ContentRoute from './components/content/ContentRoute';
import ProfilesRoute from './components/profile/ProfilesRoute';
import CollectionsRoute from './components/collection/CollectionsRoute';

function App() {
//const location = useLocation();
  const {commonStore, userStore} = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally((() => {
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
          <Route path='/' element={<HomeRoute />} />
          <Route path='/content/:contentId/:index'element={<ContentRoute />}/>
          <Route path='/profiles/:username/:lang' element={<ProfilesRoute />}/>
          <Route path='account/login' element={<LoginForm />} /> 
          <Route path='account/register' element={<RegisterForm />} /> 
          <Route path='/collections/:lang' element={<CollectionsRoute />} />
        </Routes>
      </Container>
   </Container>
  );
}

export default App;
