import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './Components/App/App';
import { AuthProvider } from './Context/AuthProvide';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
  <BrowserRouter>
      <Routes>
        <Route path='/*' element={<App />} />
      </Routes>
      <ToastContainer/>
  </BrowserRouter>
  </React.StrictMode>
);
