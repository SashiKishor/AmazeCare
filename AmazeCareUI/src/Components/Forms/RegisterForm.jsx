import React, { useState } from 'react';
import { Link } from 'react-router-dom';

function RegisterForm({ onSubmit, loading, error: serverError, success }) {
  const [fullName, setFullName] = useState('');
  const [userName, setUserName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const [dateOfBirth, setDateOfBirth] = useState('');
  const [gender, setGender] = useState('Male');
  const [contactNumber, setContactNumber] = useState('');


  const [localError, setLocalError] = useState(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setLocalError('Passwords do not match.');
      return;
    }

    if (!/^\d{10}$/.test(contactNumber)) {
      setLocalError('Contact number must be exactly 10 digits.');
      return;
    }

    setLocalError(null);
    onSubmit({
      fullName,
      userName,
      email,
      password,
      dateOfBirth,
      gender,
      contactNumber
    });
  };


  const maroonColor = '#8b1031';


  const displayError = localError || serverError;

  return (

    <div className="d-flex justify-content-center align-items-center w-100 py-5" style={{ minHeight: '80vh' }}>

      <div
        className="card shadow border-0"
        style={{
          padding: '40px',
          maxWidth: '650px',
          width: '100%',
          borderRadius: '12px'
        }}
      >

        <h2 className="text-center fw-bold mb-2" style={{ color: maroonColor }}>
          AmazeCare
        </h2>
        <h5 className="text-center fw-bold mb-4" style={{ color: '#000000' }}>
          Patient Registration
        </h5>


        {success && (
          <div className="alert alert-success d-flex align-items-center p-2 mb-4" role="alert" style={{ fontSize: '14px', borderRadius: '6px' }}>
            <i className="bi bi-check-circle-fill flex-shrink-0 me-2"></i>
            <div>Registration successful! Redirecting to login...</div>
          </div>
        )}

        {displayError && (
          <div className="alert alert-danger d-flex align-items-center p-2 mb-4" role="alert" style={{ fontSize: '14px', borderRadius: '6px' }}>
            <i className="bi bi-exclamation-triangle-fill flex-shrink-0 me-2"></i>
            <div>{displayError}</div>
          </div>
        )}

        <form onSubmit={handleSubmit}>


          <div className="mb-3 text-start">
            <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Full Name:</label>
            <input
              type="text"
              className="form-control p-2"
              placeholder="Enter full name"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              required
            />
          </div>

          <div className="row mb-3">
            <div className="col-md-6 text-start mb-3 mb-md-0">
              <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Username:</label>
              <input
                type="text"
                className="form-control p-2"
                placeholder="Choose a username"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                required
              />
            </div>
            <div className="col-md-6 text-start">
              <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Email Address:</label>
              <input
                type="email"
                className="form-control p-2"
                placeholder="Enter email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
          </div>


          <div className="row mb-3">
            <div className="col-md-6 text-start mb-3 mb-md-0">
              <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Date of Birth:</label>
              <input
                type="date"
                className="form-control p-2"
                value={dateOfBirth}
                onChange={(e) => setDateOfBirth(e.target.value)}
                max={new Date().toISOString().split('T')[0]}
                required
              />
            </div>
            <div className="col-md-6 text-start">
              <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Gender:</label>
              <select
                className="form-select p-2"
                value={gender}
                onChange={(e) => setGender(e.target.value)}
                required
              >
                <option value="Male">Male</option>
                <option value="Female">Female</option>
                <option value="Other">Other</option>
              </select>
            </div>
          </div>


          <div className="mb-3 text-start">
            <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Contact Number:</label>
            <input
              type="tel"
              className="form-control p-2"
              placeholder="Enter 10-digit number (e.g. 9876543210)"
              value={contactNumber}
              onChange={(e) => setContactNumber(e.target.value)}
              required
            />
          </div>

          <div className="row mb-4">
            <div className="col-md-6 text-start mb-3 mb-md-0">
              <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Password:</label>
              <input
                type="password"
                className="form-control p-2"
                placeholder="Enter password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            <div className="col-md-6 text-start">
              <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Confirm Password:</label>
              <input
                type="password"
                className="form-control p-2"
                placeholder="Confirm password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
              />
            </div>
          </div>


          <button
            type="submit"
            className="btn w-100 fw-bold mb-4 p-2 d-flex justify-content-center align-items-center gap-2"
            disabled={loading}
            style={{ backgroundColor: maroonColor, color: '#ffffff', borderRadius: '6px' }}
          >
            {loading ? (
              <>
                <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                <span>Registering...</span>
              </>
            ) : (
              'Register Account'
            )}
          </button>


          <div className="text-center" style={{ fontSize: '15px', color: '#000000' }}>
            Already have an account?{' '}
            <Link to="/login" style={{ color: maroonColor, textDecoration: 'none' }}>
              Sign In
            </Link>
          </div>

        </form>
      </div>

    </div>
  );
}

export default RegisterForm;