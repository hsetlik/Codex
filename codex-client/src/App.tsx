import {useEffect} from 'react';
import './App.css';
import { useStore } from './app/stores/store'
import { Route, Routes, useLocation } from 'react-router-dom';
import { Container } from 'semantic-ui-react';
import NavBar from './components/common/NavBar';
import ContentRoute from './components/content/ContentRoute';
import FeedRoute from './components/feed/FeedRoute';
import LoginForm from './components/account/LoginForm';
import AccountRoute from './components/account/AccountRoute';

function App() {
//const location = useLocation();
  const {commonStore, userStore} = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  

  return (
    <Container>
      <NavBar />
      <Routes >
        <Route path='/feed' element={<FeedRoute />}/>
        <Route path='/content'element={<ContentRoute />}/>
        <Route path='/account' element={<AccountRoute />}/>
        <Route path='account/login' element={<LoginForm />} /> 
      </Routes>
   </Container>
  );
}

export default App;
