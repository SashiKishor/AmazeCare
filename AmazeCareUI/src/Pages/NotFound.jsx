import React from 'react';
import { useNavigate } from 'react-router-dom';

export default function NotFound() {
  const navigate = useNavigate();
  const maroonColor = '#8b1031';

  return (
    <div className="container d-flex flex-column justify-content-center align-items-center text-center" style={{ minHeight: '80vh' }}>
      <div className="card border-0 shadow-sm p-5 rounded-4" style={{ maxWidth: '500px' }}>
        
        <div className="mb-4">
          <i className="bi bi-exclamation-octagon-fill text-danger" style={{ fontSize: '5rem' }}></i>
        </div>

        <h1 className="display-1 fw-bold text-dark mb-2">404</h1>

        <h3 className="fw-bold mb-3" style={{ color: maroonColor }}>Page Not Found</h3>
        
        <p className="text-muted mb-4">
          The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.
        </p>

        <div className="d-flex justify-content-center gap-2">
          <button 
            type="button" 
            className="btn btn-outline-secondary fw-bold px-4 shadow-sm" 
            onClick={() => navigate(-1)}
          >
            Go Back
          </button>
          <button 
            type="button" 
            className="btn text-white fw-bold px-4 shadow-sm" 
            style={{ backgroundColor: maroonColor }}
            onClick={() => navigate('/')}
          >
            Back to Homepage
          </button>
        </div>

      </div>
    </div>
  );
}
