import { jwtDecode } from "jwt-decode";
import { createContext, useState, useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { ROUTES } from "../util/constants";
import { loginApi } from "../api/authApi";

const BASE_URL = import.meta.env.VITE_API_URL;

const AuthContext = createContext(null);

// Create the context
export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }

  return context;
  //return useContext(AuthContext);
}

// AuthProvider component
export function AuthProvider({ children }) {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const userData = localStorage.getItem("data");
    const accessToken = localStorage.getItem("accessToken");

    console.log(userData);

    if (userData && accessToken) {
      setUser(JSON.parse(userData));
      setToken(jwtDecode(accessToken.accessToken));
    }

    setLoading(false);
  }, []);

  function buildUserFromToken(decoded) {
    // Roles can come back as a single string or an array depending on how many roles a user has -
    // the ClaimTypes.Role claim from our .NET backend collapses to one value if there's only one role
    const roles = decoded.role
      ? Array.isArray(decoded)
        ? decoded.role
        : [decoded.role]
      : [];

    return { id: decoded.sub, email: decoded.email, roles };
  }

  // login function
  const login = async (email, password) => {
    try {
      const response = await fetch(`${BASE_URL}/authentication/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (!response.ok) toast.warning("Login failed");

      const data = await response.json();
      setUser(data.user);
      setToken(data.accessToken);
      localStorage.setItem("accessToken", data.accessToken);
      return data;
    } catch (error) {
      throw error;
    }
  };

  // Logout function
  function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
  }

  const values = {
    user,
    login,
    logout,
    loading,
    isAuthenticated: !!user,
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return <AuthContext.Provider value={values}>{children}</AuthContext.Provider>;
}
