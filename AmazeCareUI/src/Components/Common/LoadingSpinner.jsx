import React from 'react';

function LoadingSpinner({ message = 'Loading...', fullPage = true, small = false }) {
  if (small) {
    return (
      <div className="spinner-ring spinner-ring-sm color-primary" role="status">
        <span className="screen-reader-only">{message}</span>
      </div>
    );
  }

  if (fullPage) {
    return (
      <div className="spinner-container-large">
        <div className="spinner-ring color-primary" role="status">
          <span className="screen-reader-only">{message}</span>
        </div>
      </div>
    );
  }

  return (
    <div className="spinner-container">
      <div className="spinner-ring color-primary" role="status">
        <span className="screen-reader-only">{message}</span>
      </div>
    </div>
  );
}

export default LoadingSpinner;
