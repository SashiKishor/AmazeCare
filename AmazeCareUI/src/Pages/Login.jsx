import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import { login } from '../api/auth';
import LoginForm from '../Components/Forms/LoginForm';

function Login() {
  const { loginUser } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fromPath = location.state?.from;

  const handleLoginSubmit = async ({ userName, password }) => {
    setLoading(true);
    setError(null);

    try {
      const response = await login({ userName, password });

      if (response.statusCode === 200 && response.data) {
        const { token, userId, fullName, role, profileId } = response.data;
        const userDetails = { userId, fullName, userName, role };

        loginUser(token, userDetails, profileId);

        if (fromPath) {
          navigate(fromPath, { replace: true });
        } else {
          switch (role) {
            case 'Admin':
              navigate('/');
              break;
            case 'Doctor':
              navigate('/');
              break;
            case 'Patient':
              navigate('/patient/appointments');
              break;
            default:
              navigate('/');
          }
        }
      } else {
        setError(response.message || 'Login failed. Please check your credentials.');
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Login failed. Please verify your connection.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <LoginForm
        onSubmit={handleLoginSubmit}
        loading={loading}
        error={error}
      />
    </div>
  );
}

export default Login;