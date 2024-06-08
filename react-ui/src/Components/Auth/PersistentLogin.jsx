import { Outlet } from "react-router-dom";
import useRefreshToken from "../../hooks/useRefreshToken";
import LoaderCircle from "../Loaders/LoaderCircle";
import authStore from "../../Store/authStore";
import { useEffect, useRef, useState } from "react";

const PersistLogin = () => {
  const [isLoading, setIsLoading] = useState(true);
  const persist = authStore(state => state.persist);
  const auth = authStore(state => state.auth);
  const refresh = useRefreshToken();

  useEffect(() => {
    let isMounted = true;

    const verifyRefreshToken = async () => {
      try {
        await refresh();
      }
      catch (err) {
        console.error(err);
      }
      finally {
        isMounted && setIsLoading(false);
      }
    }
    !auth?.accessToken && persist ? verifyRefreshToken() : setIsLoading(false);

    return () => isMounted = false;
  }, [])


  return (
    <>
      {!persist
        ? <Outlet />
        : isLoading
          ? <LoaderCircle />
          : <Outlet />
      }
    </>
  )
}

export default PersistLogin