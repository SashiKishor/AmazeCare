import React, { createContext, useContext, useState, useEffect } from 'react';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem('token') || null);
  const [user, setUser] = useState(() => {
    const savedUser = localStorage.getItem('user');
    return savedUser ? JSON.parse(savedUser) : null;
  });
  const [profileId, setProfileId] = useState(() => {
    const savedProfileId = localStorage.getItem('profileId');
    return savedProfileId ? parseInt(savedProfileId, 10) : null;
  });

  const loginUser = (authToken, userDetails, profileVal) => {
    localStorage.setItem('token', authToken);
    localStorage.setItem('user', JSON.stringify(userDetails));
    if (profileVal !== undefined && profileVal !== null) {
      localStorage.setItem('profileId', profileVal.toString());
      setProfileId(profileVal);
    } else {
      localStorage.removeItem('profileId');
      setProfileId(null);
    }
    setToken(authToken);
    setUser(userDetails);
  };

  const logoutUser = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('profileId');
    setToken(null);
    setUser(null);
    setProfileId(null);
  };

  const updateProfileId = (id) => {
    localStorage.setItem('profileId', id.toString());
    setProfileId(id);
  };

  const isAuthenticated = !!token;
  const isAdmin = user?.role === 'Admin';
  const isDoctor = user?.role === 'Doctor';
  const isPatient = user?.role === 'Patient';

  return (
    <AuthContext.Provider
      value={{
        token,
        user,
        profileId,
        isAuthenticated,
        isAdmin,
        isDoctor,
        isPatient,
        loginUser,
        logoutUser,
        updateProfileId
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};



