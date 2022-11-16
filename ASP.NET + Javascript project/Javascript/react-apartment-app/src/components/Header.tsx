import { NavLink } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { update } from "../store/identity";

function Header() {
    const jwt = useAppSelector((state) => state.identity);
    const dispatch = useAppDispatch();

    function logOut() {
        dispatch(update({
            token: "",
            refreshToken: "",
            firstName: "",
            lastName: "",
            appUserId: "",
            loggedIn: false}))
    
    }

    return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/">
                        Apartment Web App
                    </a>
                    <button
                        className="navbar-toggler"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target=".navbar-collapse"
                        aria-controls="navbarSupportedContent"
                        aria-expanded="false"
                        aria-label="Toggle navigation"
                    >
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                        <ul className="navbar-nav flex-grow-1">
                            <li className="nav-item">
                                <NavLink
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link active"
                                            : "nav-link"
                                    }
                                    to="/"
                                >
                                    Home
                                </NavLink>
                            </li>
                            {jwt.loggedIn && (<>
                                
                            <li className="nav-item">
                                <NavLink
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link active"
                                            : "nav-link"
                                    }
                                    to="/Associations"
                                >
                                    Associations
                                </NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link active"
                                            : "nav-link"
                                    }
                                    to="/Owners"
                                >
                                    Owner
                                </NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link active"
                                            : "nav-link"
                                    }
                                    to="/Funds"
                                >
                                    Funds
                                </NavLink>
                            </li>
                            </>)}
                            
                            <li className="nav-item">
                                <NavLink
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link active"
                                            : "nav-link"
                                    }
                                    to="/Home/Privacy"
                                >
                                    Privacy
                                </NavLink>
                            </li>
                        </ul>

                        <ul className="navbar-nav">
                            {!jwt.loggedIn && (
                                <>
                                    <li className="nav-item">
                                        {/* <a className="nav-link text-dark" href="/Identity/Account/Register">Register</a> */}
                                        <NavLink
                                            className={({ isActive }) =>
                                                isActive
                                                    ? "nav-link active"
                                                    : "nav-link"
                                            }
                                            to="/Register"
                                        >
                                            Register
                                        </NavLink>
                                    </li>
                                    <li className="nav-item">
                                        <NavLink
                                            className={({ isActive }) =>
                                                isActive
                                                    ? "nav-link active"
                                                    : "nav-link"
                                            }
                                            to="/Login"
                                        >
                                            Login
                                        </NavLink>
                                        {/* <a className="nav-link text-dark" href="/Identity/Account/Login">Login</a> */}
                                    </li>
                                </>
                            )}

                            {jwt.loggedIn && (
                                <>
                                    <li className="nav-item">
                                        <div className="nav-link active">
                                        {jwt.firstName + " " + jwt.lastName}
                                        </div>
                                        
                                    </li>
                                    <li className="nav-item">
                                        <NavLink
                                            className={({ isActive }) =>
                                                isActive
                                                    ? "nav-link active"
                                                    : "nav-link"
                                            }
                                            to="/"
                                            onClick={logOut}
                                        >
                                            Log Out
                                        </NavLink>
                                        {/* <a className="nav-link text-dark" href="/Identity/Account/Login">Login</a> */}
                                    </li>
                                </>
                            )}
                        </ul>
                    </div>
                </div>
            </nav>
        </header>
    );
}

export default Header;
