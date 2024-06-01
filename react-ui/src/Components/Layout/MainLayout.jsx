import { Outlet } from "react-router-dom"
import Header from "./Header";

const MainLayout = () => {
  return (
    <main className="App">
      <Outlet />
    </main>
  )
}

export default MainLayout