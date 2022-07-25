import {useEffect} from 'react';
import './App.css';
import { useStore } from './app/stores/store'
import { Route, Routes, useLocation } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import NavBar from './components/common/NavBar';
import FeedRoute from './components/feed/FeedRoute';
import LoginForm from './components/account/LoginForm';
import HomeRoute from './components/common/HomeRoute';
import RegisterForm from './components/account/RegisterForm';
import ProfilesRoute from './components/profile/ProfilesRoute';
import CollectionsRoute from './components/collection/CollectionsRoute';
import TagRoute from './components/feed/TagRoute';
import ArticleRoute from './components/contentFrame/ArticleRoute';
import VideoRoute from './components/content/leftColumn/youtubePlayer/VideoRoute';
import ModalContainer from './app/common/modals/ModalContainer';
import FeedRedirectRoute from './components/feed/FeedRedirectRoute';

function App() {
  const location = useLocation();
  const entered = location.pathname !== "/";
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
    <div>
      <ModalContainer />
      {entered && <NavBar />}
      <Container style={{margin: (entered) ? '7em' : '0em'}}>
        <Routes >
          <Route path='/feed/:lang' element={<FeedRoute />}/>
          <Route path='/feed' element={<FeedRedirectRoute />}/>
          <Route path='/' element={<HomeRoute />} />
          <Route path='/profiles/:username/:lang' element={<ProfilesRoute />}/>
          <Route path='account/login' element={<LoginForm />} /> 
          <Route path='account/register' element={<RegisterForm />} /> 
          <Route path='/collections/:lang' element={<CollectionsRoute />} />
          <Route path='tags/:tag' element={<TagRoute />} />
          <Route path='viewer/:contentId' element={<ArticleRoute />} />
          <Route path='video/:contentId' element={<VideoRoute />} />
        </Routes>
      </Container>
   </div>
  );
}

export default App;
