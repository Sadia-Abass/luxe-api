import { Route, Routes } from "react-router-dom";
import Home from "../pages/Home";
import { ROUTES } from "../util/constants";

function AppRouter() {
  return (
    <Routes>
      <Route path={ROUTES.HOME} element={<Home />} />
      {/* <Route path="" element={} /> */}
    </Routes>
  );
}

export default AppRouter;
