import ReactDOM from 'react-dom';
import App from './App';
import reportWebVitals from './reportWebVitals';
import '../src/components/styles/styles.css';
import 'semantic-ui-css/semantic.min.css'
import { BrowserRouter } from 'react-router-dom';
import { store, storeContext as StoreContext } from './app/stores/store';


ReactDOM.render(
  <StoreContext.Provider value={store} >
  <BrowserRouter >
    <App />
  </BrowserRouter>
</StoreContext.Provider>,
document.getElementById('root') 
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
