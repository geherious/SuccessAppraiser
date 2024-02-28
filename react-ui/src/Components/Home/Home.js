import { Link } from "react-router-dom"
import useAxiosPrivate from "../../hooks/useAxiosPrivate"

const Home = () => {

  const axiosPrivate = useAxiosPrivate();
  const handleOnClick = async () => {
    let response = await axiosPrivate.get('/work-space/goals');
    console.log(response.data);
  }
  return (
    <>
    <h1>Home page</h1>
    <p>It's a page for registered users</p>
    <Link to="/policy">Home</Link>
    <button onClick={handleOnClick}>Click</button>
    </>
  )
}

export default Home