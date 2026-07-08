import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../Context/AuthContext';
import { getAllDoctors, getAvailableDoctors } from '../api/doctor';
import DoctorSearchForm from '../Components/Forms/DoctorSearchForm';
import DoctorCard from '../Components/Cards/DoctorCard';
import LoadingSpinner from '../Components/Common/LoadingSpinner';

function FindADoctor() {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();

  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const maroonColor = '#8b1031';

  useEffect(() => {
    fetchDoctors();
  }, []);

  const fetchDoctors = async () => {
    setLoading(true);
    try {
      const data = await getAllDoctors();
      setDoctors(data);
      setError(null);
    } catch (err) {
      console.error(err);
      setError('Could not load doctors data.');
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async ({ searchName, searchSpeciality, searchExperience }) => {
    setLoading(true);
    try {
      const filters = {};
      if (searchName.trim()) filters.DoctorName = searchName;
      if (searchSpeciality.trim()) filters.Speciality = searchSpeciality;
      if (searchExperience.trim()) filters.Experience = parseFloat(searchExperience);

      const result = await getAvailableDoctors(filters);
      if (result.success && result.data) {
        setDoctors(result.data);
      } else {
        setDoctors([]);
      }
      setError(null);
    } catch (err) {
      console.error(err);
      setDoctors([]);
      setError('No doctors found matching filters or an error occurred.');
    } finally {
      setLoading(false);
    }
  };

  const handleBookClick = (doctorId) => {
    if (!isAuthenticated) {
      navigate('/login', { state: { from: `/patient/book-appointment?doctorId=${doctorId}` } });
    } else {
      navigate(`/patient/book-appointment?doctorId=${doctorId}`);
    }
  };

  return (
    <div className="container py-5">

      
      <div className="card shadow-sm border-0 mb-5 rounded-4">
        <div className="card-body p-4 text-start">
          <h3 className="fw-bold mb-4" style={{ color: maroonColor }}>
            <i className="bi bi-search me-2"></i>Find a Healthcare Specialist
          </h3>
          <DoctorSearchForm onSearch={handleSearch} onReset={fetchDoctors} />
        </div>
      </div>

      <h4 className="fw-bold mb-4 text-start" style={{ color: maroonColor }}>Available Doctors</h4>

      {loading ? (
        <div className="d-flex justify-content-center py-5">
          <LoadingSpinner message="Loading doctors directory..." />
        </div>
      ) : error ? (

        <div className="alert alert-warning text-center" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-2"></i>{error}
        </div>
      ) : doctors.length === 0 ? (

        <div className="alert alert-info text-center" role="alert">
          <i className="bi bi-info-circle-fill me-2"></i>No doctors found matching your criteria.
        </div>
      ) : (
        <div className="row g-4">
          {doctors.map((doc) => (
            <div className="col-12 col-md-6 col-lg-4" key={doc.doctorId}>
              <DoctorCard doctor={doc} onBook={handleBookClick} />
            </div>
          ))}
        </div>
      )}

    </div>
  );
}

export default FindADoctor;