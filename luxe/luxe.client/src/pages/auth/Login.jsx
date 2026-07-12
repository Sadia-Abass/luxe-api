import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { login } from "../../api/authApi";
import { useAuth } from "../../contexts/AuthContext";
import { ROUTES } from "../../util/constants";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [loginUser] = useAuth();
  const navigate = useNavigate();

  async function handlesubmit(e) {
    e.preventDefault();
    setError("");
    setIsSubmitting(true);

    try {
      const response = await login(email, password);
      loginUser(response.data.acceesToken, response.data.refreshToken);
      navigate(ROUTES.DASHBOARD);
    } catch (err) {
      setError(
        err.response?.data?.message ||
          "Something went wrong. Please try again.",
      );
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className='container' style={{ maxWidth: "420px", marginTop: "80px" }}>
      <h2 className='mb-4'>Login</h2>

      {error && <div className='alert alert-danger'>{error}</div>}

      <form onSubmit={handlesubmit}>
        <div className='mb-3'>
          <label htmlFor='email' className='form-label'>
            Email
          </label>
          <input
            type='email'
            className='form-control'
            name='email'
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          ></input>
        </div>

        <div className='mb-3'>
          <label htmlFor='password' className='form-label'>
            Password
          </label>
          <input
            type='password'
            className='form-control'
            name='password'
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          ></input>
        </div>

        <button
          type='submit'
          className='btn btn-primary w-100'
          disabled={isSubmitting}
        >
          {isSubmitting ? "Logging in..." : "Login"}
        </button>
      </form>

      <div className='mt-3 text-center'>
        <Link to={ROUTES.FORGOT_PASSWORD}></Link>
      </div>
      <div className='mt-2 text-center'>
        <p>
          Don't have an account? <Link to={ROUTES.REGISTER}>Register</Link>
        </p>
      </div>
    </div>
  );
}
