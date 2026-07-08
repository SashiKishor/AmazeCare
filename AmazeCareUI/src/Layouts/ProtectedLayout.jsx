import React from 'react';
import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';


function ProtectedLayout({ allowedRoles }) {
  
  const { isAuthenticated, user, profileId } = useAuth();
  const location = useLocation();

  
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  if (allowedRoles && !allowedRoles.includes(user.role)) {
    return <Navigate to="/" replace />;
  }
 
  return <Outlet />;
};

export default ProtectedLayout;
