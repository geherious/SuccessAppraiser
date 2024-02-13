import './App.css';
import Register from '../Auth/Register/Register';
import "bootstrap/dist/css/bootstrap.min.css";
import Login from '../Auth/Login/Login';
import Layout from '../Layout/Layout'
import { Routes, Route } from 'react-router-dom';
import Welcome from '../Welcome/Welcome';
import RequireAuth from '../Auth/RequireAuth';
import Home from '../Home/Home';

function App() {
  return (
    <Routes>
      <Route path='/' element={<Layout/>}>
        {/* public routes */}
        <Route path='login' element={<Login/>}/>
        <Route path='register' element={<Register/>}/>
        <Route path='/' element={<Welcome/>}/>

        {/* private routes */}
        <Route element={<RequireAuth/>}>
          <Route path="home" element={<Home/>} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
