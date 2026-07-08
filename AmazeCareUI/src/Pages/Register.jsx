import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { registerPatientUser } from '../api/auth';
import RegisterForm from '../Components/Forms/RegisterForm';

function Register() {
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  const handleRegisterSubmit = async ({ fullName, userName, email, password, dateOfBirth, gender, contactNumber }) => {
    setLoading(true);
    setError(null);

    try {
      const response = await registerPatientUser({
        fullName,
        userName,
        email,
        password,
        dateOfBirth,
        gender,
        contactNumber
      });

      if (response.success) {
        setSuccess(true);
        setTimeout(() => {
          navigate('/login');
        }, 2000);
      } else {
        setError(response.message || 'Registration failed.');
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Registration failed. Username may already be in use.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      
      <RegisterForm 
        onSubmit={handleRegisterSubmit} 
        loading={loading} 
        error={error}
        success={success} 
      />
    </div>
  );
}

export default Register;