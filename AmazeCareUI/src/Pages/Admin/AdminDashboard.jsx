import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import LoadingSpinner from '../../Components/Common/LoadingSpinner';
import { getAllDoctors } from '../../api/doctor';
import { getAllPatients } from '../../api/patient';
import { getAllAppointments } from '../../api/appointment';

export default function AdminDashboard() {
  const [stats, setStats] = useState({
    doctorsCount: 0,
    patientsCount: 0,
    appointmentsCount: 0
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const maroonColor = '#8b1031';

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    setLoading(true);
    setError(null);
    try {
      const [doctors, patients, appointments] = await Promise.all([
        getAllDoctors(),
        getAllPatients(),
        getAllAppointments()
      ]);

      setStats({
        doctorsCount: doctors.length,
        patientsCount: patients.length,
        appointmentsCount: appointments.length
      });
    } catch (err) {
      console.error(err);
      setError('Failed to load dashboard statistics.');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="d-flex justify-content-center py-5">
        <LoadingSpinner message="Loading statistics..." />
      </div>
    );
  }

  return (
    <div className="container py-5 text-start">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2 className="fw-bold mb-0" style={{ color: maroonColor }}>Admin Dashboard</h2>
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

      
      <div className="row g-4 mb-5">
        
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-3 h-100">
            <div className="card-body d-flex align-items-center justify-content-between">
              <div>
                <h6 className="text-uppercase fw-bold text-muted mb-2">Total Doctors</h6>
                <h2 className="fw-bold mb-0 text-dark">{stats.doctorsCount}</h2>
              </div>
              <div className="rounded-circle p-3 bg-light text-primary" style={{ fontSize: '2rem' }}>
                <i className="bi bi-person-badge"></i>
              </div>
            </div>
          </div>
        </div>

        
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-3 h-100">
            <div className="card-body d-flex align-items-center justify-content-between">
              <div>
                <h6 className="text-uppercase fw-bold text-muted mb-2">Total Patients</h6>
                <h2 className="fw-bold mb-0 text-dark">{stats.patientsCount}</h2>
              </div>
              <div className="rounded-circle p-3 bg-light text-success" style={{ fontSize: '2rem' }}>
                <i className="bi bi-people"></i>
              </div>
            </div>
          </div>
        </div>

        
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-3 h-100">
            <div className="card-body d-flex align-items-center justify-content-between">
              <div>
                <h6 className="text-uppercase fw-bold text-muted mb-2">Total Appointments</h6>
                <h2 className="fw-bold mb-0 text-dark">{stats.appointmentsCount}</h2>
              </div>
              <div className="rounded-circle p-3 bg-light text-warning" style={{ fontSize: '2rem' }}>
                <i className="bi bi-calendar-check"></i>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Actions / Management Section */}
      <h4 className="fw-bold mb-4" style={{ color: maroonColor }}>Manage Operations</h4>
      <div className="row g-4">
        {/* Doctors Management */}
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 h-100 text-center py-4">
            <div className="card-body">
              <i className="bi bi-shield-person text-primary mb-3" style={{ fontSize: '3rem' }}></i>
              <h5 className="fw-bold mb-2">Manage Doctors</h5>
              <p className="text-muted mb-4 small">Add, edit details, and manage system doctor profiles.</p>
              <Link to="/admin/doctors" className="btn fw-bold px-4 shadow-sm" style={{ backgroundColor: maroonColor, color: '#ffffff' }}>
                Go to Doctors
              </Link>
            </div>
          </div>
        </div>

        
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 h-100 text-center py-4">
            <div className="card-body">
              <i className="bi bi-people-fill text-success mb-3" style={{ fontSize: '3rem' }}></i>
              <h5 className="fw-bold mb-2">Manage Patients</h5>
              <p className="text-muted mb-4 small">View and manage registered patient records.</p>
              <Link to="/admin/patients" className="btn fw-bold px-4 shadow-sm" style={{ backgroundColor: maroonColor, color: '#ffffff' }}>
                Go to Patients
              </Link>
            </div>
          </div>
        </div>

        {/* Appointments Management */}
        <div className="col-12 col-md-4">
          <div className="card border-0 shadow-sm rounded-4 h-100 text-center py-4">
            <div className="card-body">
              <i className="bi bi-calendar-event-fill text-warning mb-3" style={{ fontSize: '3rem' }}></i>
              <h5 className="fw-bold mb-2">Manage Appointments</h5>
              <p className="text-muted mb-4 small">Accept, cancel, and reschedule appointments.</p>
              <Link to="/admin/appointments" className="btn fw-bold px-4 shadow-sm" style={{ backgroundColor: maroonColor, color: '#ffffff' }}>
                Go to Appointments
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
