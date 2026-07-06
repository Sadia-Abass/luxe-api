import {
  createContext,
  useState,
  useContext,
  useEffect,
  Children,
} from "react";

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
export const AuthProvider = ({ Children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const userData = localStorage.getItem("user");

    if (token && userData) {
      setUser(JSON.parse(userData));
    }

    setLoading(false);
  }, []);

  // Login function
  const login = async (email, password) => {
    try {
      const response = await fetch(
        "https://localhost:7217/api/authentication/login",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ email, password }),
        },
      );

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || "Login failed");
      }

      const data = await response.json();

      // Save token and user data
      localStorage.setItem("token", data.token);
      localStorage.setItem(
        "user",
        JSON.stringify({
          email: data.email,
        }), //username: data.username
      );

      setUser({
        email: data.email,
        //username: data.username,
      });

      return { success: true };
    } catch (error) {
      return {
        success: false,
        error: error.message,
      };
    }
  };

  // Register function
  const register = async (firstname, lastname, email, password) => {
    try {
      const resposne = await fetch(
        "https://localhost:7217/api/authentication/register",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ firstname, lastname, email, password }),
        },
      );

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || "Registration failed");
      }

      // Save token and user data
      localStorage.setItem("token", data.token);
      localStorage.setItem(
        "user",
        JSON.stringify({
          firstname: data.firstname,
          lastname: data.lastname,
          email: data.email,
        }),
      );

      setUser({
        firstname: data.firstname,
        lastname: data.lastname,
        email: data.email,
      });

      return { success: true };
    } catch (error) {
      return { success: false, error: error.message };
    }
  };

  // Logout function
  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
  };

  // Function to make authentication API calls
  const fetchWithAuth = async (URL, options = {}) => {
    const token = localStorage.getItem("token");

    const config = {
      ...options,
      headers: {
        ...options.headers,
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    };

    return fetch(URL, config);
  };

  const value = {
    user,
    login,
    register,
    logout,
    fetchWithAuth,
    isAuthenticated: !!user,
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return <AuthContext.Provider value={values}>{children}</AuthContext.Provider>;
};
