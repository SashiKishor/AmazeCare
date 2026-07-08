import React, { useState } from 'react';
import { Link } from 'react-router-dom';


const LoginForm = ({ onSubmit, loading, error }) => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ userName, password });
  };

  const maroonColor = '#8b1031';

  return (
    <div className="d-flex justify-content-center align-items-center w-100" style={{ minHeight: '80vh' }}>
      <div
        className="card shadow border-0"
        style={{ padding: '40px', maxWidth: '450px', width: '100%', borderRadius: '12px' }}
      >
        <h2 className="text-center fw-bold mb-3" style={{ color: maroonColor }}>
          AmazeCare
        </h2>
        <h5 className="text-center fw-bold mb-4" style={{ color: '#000000' }}>
          Enter your login credentials
        </h5>

        {error && (
          <div className="alert alert-danger d-flex align-items-center p-2 mb-4" role="alert" style={{ fontSize: '14px', borderRadius: '6px' }}>
            <i className="bi bi-exclamation-triangle-fill flex-shrink-0 me-2"></i>
            <div>{error}</div>
          </div>
        )}

        <form onSubmit={handleSubmit}>

          <div className="mb-3 text-start">
            <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Username:</label>
            <input
              type="text"
              className="form-control p-2"
              placeholder="Enter your Username"
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              required
            />
          </div>

          <div className="mb-4 text-start">
            <label className="form-label fw-bold text-secondary" style={{ fontSize: '14px' }}>Password:</label>
            <input
              type="password"
              className="form-control p-2"
              placeholder="Enter your Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
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
                <span>Submitting...</span>
              </>
            ) : (
              'Submit'
            )}
          </button>

          <div className="text-center" style={{ fontSize: '15px', color: '#000000' }}>
            Not registered?{' '}
            <Link to="/register" style={{ color: maroonColor, textDecoration: 'none' }}>
              Create an account
            </Link>
          </div>

        </form>
      </div>
    </div>
  );
};

export default LoginForm;