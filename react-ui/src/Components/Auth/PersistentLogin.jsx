import { Outlet } from "react-router-dom";
import useRefreshToken from "../../hooks/useRefreshToken";
import LoaderCircle from "../Loaders/LoaderCircle";
import useAuthStore from "../../Store/useAuthStore";

const PersistLogin = () => {
  const { tokenIsLoading, abortToken } = useRefreshToken();
  const persist = useAuthStore(state => state.persist);

  return (
    <>
      {!persist
        ? <Outlet />
        : tokenIsLoading
          ? <LoaderCircle />
          : <Outlet />
      }
    </>
  )
}

export default PersistLogin