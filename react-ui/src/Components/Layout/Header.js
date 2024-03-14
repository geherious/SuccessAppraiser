import { Link } from "react-router-dom";
import useAuth from "../../hooks/useAuth"


const Header = () => {

    const {auth} = useAuth();

  return (
    <>
    <div>
        {auth.username ? auth.username : <Link to="/login">Login</Link>}
        <Link to="/policy">Policy</Link>

    </div>
    </>
  )
}

export default Header