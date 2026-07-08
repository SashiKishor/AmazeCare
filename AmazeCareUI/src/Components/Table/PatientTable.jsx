import React, { useState, useEffect } from 'react';
import LoadingSpinner from '../Common/LoadingSpinner';
import PatientModel from '../Models/PatientModel';
import { getAllPatients } from '../../api/patient';

function PatientTable() {
  const [patients, setPatients] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [modelType, setModelType] = useState(null); 
  const [selectedPatient, setSelectedPatient] = useState(null);

  const maroonColor = '#8b1031';

  useEffect(() => {
    loadPatients();
  }, []);

  const loadPatients = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getAllPatients();
      if (Array.isArray(data)) {
        setPatients(data);
      } else {
        setPatients([]);
      }
    } catch (err) {
      console.error(err);
      setError('Failed to retrieve patients list.');
    } finally {
      setLoading(false);
    }
  };

  const handleEditClick = (pat) => {
    setSelectedPatient(pat);
    setModelType('edit');
  };

  const handleDeleteClick = (pat) => {
    setSelectedPatient(pat);
    setModelType('delete');
  };

  return (
    <div className="card border-0 shadow-sm rounded-4 overflow-hidden">
      <div className="card-header bg-white border-bottom d-flex justify-content-between align-items-center py-3 px-4">
        <h5 className="fw-bold mb-0 text-dark">Registered Patients</h5>
        <div className="d-flex gap-2">
          <button 
            onClick={() => {
              setModelType('add');
            }} 
            className="btn btn-sm text-white fw-bold shadow-sm d-inline-flex align-items-center" 
            style={{ backgroundColor: maroonColor }}
          >
            <i className="bi bi-plus-lg me-1"></i> Register Patient
          </button>
          <button 
            onClick={loadPatients} 
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
        {loading && patients.length === 0 ? (
          <div className="d-flex justify-content-center py-5">
            <LoadingSpinner message="Loading patients..." />
          </div>
        ) : error ? (
          <div className="p-4">
            <div className="alert alert-danger d-flex align-items-center shadow-sm mb-0" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              <div>{error}</div>
            </div>
          </div>
        ) : patients.length === 0 ? (
          <div className="text-center py-5">
            <i className="bi bi-people text-muted mb-3 d-block" style={{ fontSize: '4rem' }}></i>
            <h4 className="fw-bold text-dark">No Patients Registered</h4>
            <p className="text-muted mb-0">Click the Register Patient button to add a new patient profile.</p>
          </div>
        ) : (
          <div className="table-responsive">
            <table className="table table-hover align-middle mb-0">
              <thead className="table-light text-secondary">
                <tr>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Patient Name</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Date of Birth</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Gender</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-start">Contact Number</th>
                  <th scope="col" className="py-3 px-4 fw-bold border-bottom-0 text-end">Actions</th>
                </tr>
              </thead>
              <tbody className="border-top-0">
                {patients.map((pat) => (
                  <tr key={pat.patientId}>
                    <td className="py-3 px-4 fw-bold text-dark text-start">{pat.fullName}</td>
                    <td className="py-3 px-4 text-muted text-start">{pat.dateOfBirth?.split('T')[0] || pat.dateOfBirth}</td>
                    <td className="py-3 px-4 text-muted text-start">{pat.gender}</td>
                    <td className="py-3 px-4 text-muted text-start">{pat.contactNumber}</td>
                    <td className="py-3 px-4 text-end">
                      <div className="d-inline-flex gap-2">
                        <button onClick={() => handleEditClick(pat)} className="btn btn-sm btn-outline-primary fw-bold shadow-sm">
                          Edit
                        </button>
                        <button onClick={() => handleDeleteClick(pat)} className="btn btn-sm btn-outline-danger fw-bold shadow-sm">
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
        <PatientModel
          mode={modelType}
          patient={selectedPatient}
          onClose={() => {
            setModelType(null);
            setSelectedPatient(null);
          }}
          onSuccess={() => {
            setModelType(null);
            setSelectedPatient(null);
            loadPatients();
          }}
        />
      )}
    </div>
  );
}

export default PatientTable;
