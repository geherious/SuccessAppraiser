import './App.css';
import Register from '../Auth/Register/Register';
import "bootstrap/dist/css/bootstrap.min.css";
import Login from '../Auth/Login/Login';
import MainLayout from '../Layout/MainLayout'
import { Routes, Route } from 'react-router-dom';
import Welcome from '../Welcome/Welcome';
import RequireAuth from '../Auth/RequireAuth';
import Home from '../Home/Home';
import Policy from '../Policy/Policy';
import PersistLogin from '../Auth/PersistentLogin';
import Missing from '../Missing/Missing';

function App() {
  return (
    <Routes>
      <Route path='/' element={<MainLayout />}>
        <Route element={<PersistLogin />}>
          {/* public routes */}
          <Route path='login' element={<Login />} />
          <Route path='register' element={<Register />} />
          <Route path='/' element={<Welcome />} />
          <Route path='/policy' element={<Policy />} />

          {/* private routes */}
          <Route element={<RequireAuth />}>
            <Route path="home" element={<Home />} />
          </Route>
        </Route>
      </Route>

      <Route path='*' element={<Missing />} />
    </Routes>
  );
}

export default App;
