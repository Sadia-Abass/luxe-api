import { NavLink } from "react-router-dom";
import { ROUTES } from "../../util/constants";

function Header() {
  return (
    <nav className="navbar navbar-expand-lg bg-body-tertiary">
      <div className="container-fluid">
        <NavLink className="navbar-brand" href={ROUTES.HOME}>
          Luxe
        </NavLink>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarSupportedContent">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item dropdown">
              <NavLink
                to="/"
                className="nav-link dropdown-toggle"
                aria-current="page"
              >
                Shop
              </NavLink>
              <ul className="dropdown-menu">
                <li>
                  <NavLink>Womens</NavLink>
                </li>
                <li>
                  <NavLink>Mens</NavLink>
                </li>
                <li>
                  <NavLink>Baby & Children</NavLink>
                </li>
              </ul>
            </li>
            <li className="nav-item">
              <NavLink to={ROUTES.LOGIN} className="nav-link">
                Link
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink to={ROUTES.REGISTER} className="nav-link">
                Register
              </NavLink>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
}

export default Header;
