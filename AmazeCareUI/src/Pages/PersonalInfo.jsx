import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import PatientModel from '../Components/Models/PatientModel';
import LoadingSpinner from '../Components/Common/LoadingSpinner';
import { getPatientById } from '../api/patient';

function PersonalInfo() {
  const { profileId, user } = useAuth();
  const role = user?.role;
  const navigate = useNavigate();

  const [profileData, setProfileData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const maroonColor = '#8b1031';

  useEffect(() => {
    if (!profileId || role !== 'Patient') {
      setError('Access denied. Profile settings are only available for Patient users.');
      setLoading(false);
      return;
    }
    loadProfileDetails();
  }, [profileId, role]);

  const loadProfileDetails = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await getPatientById(profileId);
      const data = response?.Data || response?.data || response;
      if (data) {
        setProfileData(data);
      } else {
        setError('Failed to retrieve patient profile details.');
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Error occurred while loading profile details.');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="container py-5 text-center">
        <LoadingSpinner message="Retrieving your account details..." />
      </div>
    );
  }

  if (error) {
    return (
      <div className="container py-5 text-start">
        <div className="alert alert-danger d-flex align-items-center shadow-sm" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-3 fs-3"></i>
          <div>
            <h5 className="alert-heading fw-bold mb-1">Access Error</h5>
            <p className="mb-0">{error}</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container py-5 text-start">
      <div className="row justify-content-center">
        <div className="col-lg-8">
          
          <div className="d-flex align-items-center mb-4 pb-2 border-bottom">
            <i className="bi bi-person-gear fs-1 me-3" style={{ color: maroonColor }}></i>
            <div>
              <h2 className="fw-bold mb-0 text-dark">Personal Information</h2>
              <p className="text-muted mb-0 mt-1">Review and update your profile details</p>
            </div>
          </div>

          <div className="card shadow-sm border-0 rounded-4 p-4 mb-4">
            {profileData && (
              <PatientModel
                isModal={false}
                mode="edit"
                patient={profileData}
                onClose={() => navigate(-1)}
                onSuccess={loadProfileDetails}
              />
            )}
          </div>

        </div>
      </div>
    </div>
  );
}

export default PersonalInfo;
