import { FC } from "react";
import { IState } from "./models/IState";
import { useSelector } from "react-redux";
import AuthContainer from "./components/auth/AuthContainer";
import { Navigate, Route, Routes } from "react-router-dom";
import CreateImage from "./components/create/Create";
import Main from "./components/main/Main";
import Picture from "./components/picture/Picture";

type RouteProps = { isAuth: boolean };

const AppRoutes: FC<RouteProps> = (props: RouteProps) => {
    return (
        <Routes>
            <Route path="/create" element={
                <AuthGuard>
                    <CreateImage />
                </AuthGuard>
            } />
             <Route path="/picture/:id" element={
                <AuthGuard>
                    <Picture />
                </AuthGuard>
            } />
            <Route path="/main" element={
                <AuthGuard>
                    <Main />
                </AuthGuard>
            } />
            <Route path="/auth" element={<AuthContainer />} />
            <Route path="*" element={
                <Navigate to={"/" + props.isAuth ? "main" : "auth"} />
            } />
        </Routes>
    );
}

const AuthGuard = ({ children }: { children: JSX.Element }) => {
    const isAuth = useSelector((state: IState) => state.auth.isAuth);

    // if (!localStorage.getItem('token') || !isAuth) {
    //     return <Navigate to="/auth" />;
    // }

    return children;
}

export default AppRoutes;