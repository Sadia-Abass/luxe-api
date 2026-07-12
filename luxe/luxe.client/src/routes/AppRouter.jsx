import { Route, Routes } from "react-router-dom";
import Home from "../pages/Home";
import { ROUTES } from "../util/constants";
import Login from "../pages/auth/Login";
import Dashboard from "../pages/Dashboard";

function AppRouter() {
  return (
    <Routes>
      <Route path={ROUTES.HOME} element={<Home />} />
      <Route path={ROUTES.LOGIN} element={<Login />} />
      <Route path={ROUTES.DASHBOARD} element={<Dashboard />} />
      {/* <Route path="" element={} /> */}
    </Routes>
  );
}

export default AppRouter;
