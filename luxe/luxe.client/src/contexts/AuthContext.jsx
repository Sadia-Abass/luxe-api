import { jwtDecode } from "jwt-decode";
import { createContext, useState, useContext, useEffect } from "react";

const AuthContext = createContext(null);

// Create the context
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }

  return context;
};

// AuthProvider component
export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      try {
        const decoded = jwtDecode(accessToken);
        setUser(buildUserFromToken(decoded));
      } catch {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
      }
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
  function login(accessToken, refreshToken) {
    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);
  }

  // Logout function
  function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
  }

  const value = {
    user,
    login,
    logout,
    isLoading,
    isAuthenticated: !!user,
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return <AuthContext.Provider value={values}>{children}</AuthContext.Provider>;
};
