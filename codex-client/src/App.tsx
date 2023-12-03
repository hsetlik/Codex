import {useEffect} from 'react';
import './App.css';
import { useStore } from './app/stores/store'
import { Outlet, useLocation } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import NavBar from './components/common/NavBar';
import ModalContainer from './app/common/modals/ModalContainer';

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
        <Outlet />
      </Container>
   </div>
  );
}

export default App;
