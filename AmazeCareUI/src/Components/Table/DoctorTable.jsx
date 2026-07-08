import React, { useState, useEffect } from 'react';
import LoadingSpinner from '../Common/LoadingSpinner';
import DoctorModel from '../Models/DoctorModel';
import { getAllDoctors } from '../../api/doctor';

function DoctorTable() {
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [modelType, setModelType] = useState(null); 
  const [selectedDoctor, setSelectedDoctor] = useState(null);

  const maroonColor = '#8b1031';

  useEffect(() => {
    loadDoctors();
  }, []);

  const loadDoctors = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getAllDoctors();
      if (Array.isArray(data)) {
        setDoctors(data);
      } else {
        setDoctors([]);
      }
    } catch (err) {
      console.error(err);
      setError('Failed to retrieve doctors list.');
    } finally {
      setLoading(false);
    }
  };

  const handleEditClick = (doc) => {
    setSelectedDoctor(doc);
    setModelType('edit');
  };

  const handleDeleteClick = (doc) => {
    setSelectedDoctor(doc);
    setModelType('delete');
  };

  return (
    <div className="card border-0 shadow-sm rounded-4 overflow-hidden">
      <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center py-3 px-4">
        <h5 className="fw-bold mb-0 text-dark">System Doctors</h5>
        <div className="d-flex gap-2">
          <button 
            onClick={() => {
              setModelType('add');
            }} 
            className="btn btn-sm text-white fw-bold shadow-sm d-inline-flex align-items-center" 
            style={{ backgroundColor: maroonColor }}
          >
            <i className="bi bi-plus-lg me-1"></i> Register Doctor
          </button>
          <button 
            onClick={loadDoctors} 
            disabled={loading}
            className="btn btn-sm btn-outline-secondary fw-bold shadow-sm d-inline-flex align-items-center"
          >
            {loading ? (
              <span className="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
            ) : (
              <i className="bi bi-arrow-clockwise me-1"></i>
            )}
            {loading ? 'Refreshing...' : 'Refresh'}
          </button>
        </div>
      </div>

      <div className="card-body p-0">
        {loading && doctors.length === 0 ? (
          <div className="d-flex justify-content-center py-5">
            <LoadingSpinner message="Loading doctors..." />
          </div>
        ) : error ? (
          <div className="p-4">
            <div className="alert alert-danger d-flex align-items-center shadow-sm mb-0" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              <div>{error}</div>
            </div>
          </div>
        ) : doctors.length === 0 ? (
          <div className="text-center py-5">
            <i className="bi bi-person-badge text-muted mb-3 d-block" style={{ fontSize: '4rem' }}></i>
            <h4 className="fw-bold text-dark">No Doctors Registered</h4>
            <p className="text-muted mb-0">Click the Register Doctor button to add a new doctor profile.</p>
          </div>
        ) : (
          <div className="table-responsive">
            <table className="table table-hover align-middle mb-0">
              <thead className="table-light text-secondary">
                <tr>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Doctor Name</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Designation</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Speciality</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Experience</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-end">Actions</th>
                </tr>
              </thead>
              <tbody className="border-top-0">
                {doctors.map((doc) => (
                  <tr key={doc.doctorId}>
                    <td className="py-3 px-4 fw-bold text-dark text-start">{doc.doctorName}</td>
                    <td className="py-3 px-4 text-muted text-start">{doc.designation}</td>
                    <td className="py-3 px-4 text-muted text-start">{doc.speciality}</td>
                    <td className="py-3 px-4 text-muted text-start">{doc.experience} Years</td>
                    <td className="py-3 px-4 text-end">
                      <div className="d-inline-flex gap-2">
                        <button onClick={() => handleEditClick(doc)} className="btn btn-sm btn-outline-primary fw-bold shadow-sm">
                          Edit
                        </button>
                        <button onClick={() => handleDeleteClick(doc)} className="btn btn-sm btn-outline-danger fw-bold shadow-sm">
                          Remove
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {modelType && (
        <DoctorModel
          mode={modelType}
          doctor={selectedDoctor}
          onClose={() => {
            setModelType(null);
            setSelectedDoctor(null);
          }}
          onSuccess={() => {
            setModelType(null);
            setSelectedDoctor(null);
            loadDoctors();
          }}
        />
      )}
    </div>
  );
}

export default DoctorTable;
