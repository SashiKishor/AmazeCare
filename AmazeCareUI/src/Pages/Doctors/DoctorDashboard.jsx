import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import LoadingSpinner from '../../Components/Common/LoadingSpinner';
import { useAuth } from '../../Context/AuthContext';
import { getAllAppointmentsOfTheDoctor } from '../../api/appointment';

export default function DoctorDashboard() {
  const { profileId, user } = useAuth();
  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const maroonColor = '#8b1031';

  useEffect(() => {
    if (profileId) {
      loadStats();
    } else {
      setLoading(false);
    }
  }, [profileId]);

  const loadStats = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getAllAppointmentsOfTheDoctor(profileId);
      setAppointments(data);
    } catch (err) {
      console.error(err);
      setError('Failed to load appointments summary.');
    } finally {
      setLoading(false);
    }
  };

  const getStats = () => {
    const total = appointments.length;
    const upcoming = appointments.filter(a => a.status === 'Upcoming' || a.status === 'Scheduled' || a.status === 'Rescheduled').length;
    const completed = appointments.filter(a => a.status === 'Completed').length;
    const requested = appointments.filter(a => a.status === 'Requested').length;

    return { total, upcoming, completed, requested };
  };

  const stats = getStats();

  if (loading) {
    return (
      <div className="d-flex justify-content-center py-5">
        <LoadingSpinner message="Loading summary stats..." />
      </div>
    );
  }

  return (
    <div className="container py-5 text-start">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 className="fw-bold mb-1" style={{ color: maroonColor }}>Doctor Dashboard</h2>
          <p className="text-muted">Welcome back, <strong>Dr. {user?.fullName || user?.userName}</strong>!</p>
        </div>
        <button onClick={loadStats} className="btn btn-outline-secondary fw-bold shadow-sm">
          <i className="bi bi-arrow-clockwise me-2"></i>Refresh
        </button>
      </div>

      {error && (
        <div className="alert alert-danger shadow-sm mb-4" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-2"></i>
          {error}
        </div>
      )}

      {/* Stats row */}
      <div className="row g-4 mb-5">
        {/* Total Appointments Card */}
        <div className="col-12 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 h-100">
            <div className="card-body d-flex align-items-center justify-content-between">
              <div>
                <h6 className="text-uppercase fw-bold text-muted mb-2">Total Assigned</h6>
                <h2 className="fw-bold mb-0 text-dark">{stats.total}</h2>
              </div>
              <div className="rounded-circle p-3 bg-light text-primary" style={{ fontSize: '1.8rem' }}>
                <i className="bi bi-journals"></i>
              </div>
            </div>
          </div>
        </div>

        {/* Upcoming appointments */}
        <div className="col-12 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 h-100">
            <div className="card-body d-flex align-items-center justify-content-between">
              <div>
                <h6 className="text-uppercase fw-bold text-muted mb-2">Upcoming Slots</h6>
                <h2 className="fw-bold mb-0 text-dark">{stats.upcoming}</h2>
              </div>
              <div className="rounded-circle p-3 bg-light text-success" style={{ fontSize: '1.8rem' }}>
                <i className="bi bi-calendar-range"></i>
              </div>
            </div>
          </div>
        </div>

        {/* Completed Consultations */}
        <div className="col-12 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 h-100">
            <div className="card-body d-flex align-items-center justify-content-between">
              <div>
                <h6 className="text-uppercase fw-bold text-muted mb-2">Completed</h6>
                <h2 className="fw-bold mb-0 text-dark">{stats.completed}</h2>
              </div>
              <div className="rounded-circle p-3 bg-light text-info" style={{ fontSize: '1.8rem' }}>
                <i className="bi bi-check-circle"></i>
              </div>
            </div>
          </div>
        </div>

        {/* Pending Requests */}
        <div className="col-12 col-md-3">
          <div className="card border-0 shadow-sm rounded-4 p-3 h-100">
            <div className="card-body d-flex align-items-center justify-content-between">
              <div>
                <h6 className="text-uppercase fw-bold text-muted mb-2">Pending Requests</h6>
                <h2 className="fw-bold mb-0 text-dark">{stats.requested}</h2>
              </div>
              <div className="rounded-circle p-3 bg-light text-warning" style={{ fontSize: '1.8rem' }}>
                <i className="bi bi-clock-history"></i>
              </div>
            </div>
          </div>
        </div>
      </div>

      
      <div className="card border-0 shadow-sm rounded-4 p-4 text-center py-5">
        <div className="card-body">
          <i className="bi bi-calendar3-event text-primary mb-3" style={{ fontSize: '4rem' }}></i>
          <h4 className="fw-bold text-dark mb-2">Manage Patient Consultations</h4>
          <p className="text-muted mb-4">View scheduled timings, accept requests, reschedule or add medical prescription records.</p>
          <Link to="/doctor/appointments" className="btn fw-bold px-5 py-2 shadow-sm" style={{ backgroundColor: maroonColor, color: '#ffffff' }}>
            Go to Manage Appointments
          </Link>
        </div>
      </div>
    </div>
  );
}
