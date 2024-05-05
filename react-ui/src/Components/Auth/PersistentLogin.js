import { Outlet } from "react-router-dom";
import { useState, useEffect } from "react";
import useAuth from '../../hooks/useAuth';
import useRefreshToken from "../../hooks/useRefreshToken";
import LoaderCircle from "../Loaders/LoaderCircle";

const PersistLogin = () => {
    const [isLoading, setIsLoading] = useState(true);
    const refresh = useRefreshToken();
    const { auth, persist } = useAuth();
    const controller = new AbortController();

    useEffect(() => {
        let isMounted = true;

        const verifyRefreshToken = async () => {
            try {
                await refresh(controller.signal);
            }
            finally {
                isMounted && setIsLoading(false);
            }
        }

        !auth?.accessToken && persist ? verifyRefreshToken() : setIsLoading(false);

        return () => {
            isMounted = false;
            controller.abort();
        };
    }, [])

    return (
        <>
            {!persist
                ? <Outlet />
                : isLoading
                    ? <LoaderCircle/>
                    : <Outlet />
            }
        </>
    )
}

export default PersistLogin