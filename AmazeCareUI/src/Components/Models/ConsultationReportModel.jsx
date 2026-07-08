import React, { useState, useEffect } from 'react';
import LoadingSpinner from '../Common/LoadingSpinner';
import { getReportForRecord } from '../../api/medicalRecords';

function ConsultationReportModel({ recordId, onClose }) {
  const [reportDetails, setReportDetails] = useState(null);
  const [fetching, setFetching] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchReport() {
      setFetching(true);
      setError(null);
      try {
        const response = await getReportForRecord(recordId);
        if (response && response.statusCode === 200 && response.Data) {
          setReportDetails(response.Data);
        } else {
          setError('Failed to load report data.');
        }
      } catch (err) {
        console.error(err);
        setError('Error occurred while fetching medical record.');
      } finally {
        setFetching(false);
      }
    }

    if (recordId) {
      fetchReport();
    }
  }, [recordId]);

  const maroonColor = '#8b1031';

  return (
    
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      
      
      <div className="card border-0 shadow-lg mx-3 rounded-4 w-100 d-flex flex-column" style={{ maxWidth: '800px', maxHeight: '90vh' }}>
        
        
        <div className="card-header bg-white border-bottom d-flex justify-content-between align-items-center p-4">
          <h4 className="fw-bold mb-0" style={{ color: maroonColor }}>Consultation Summary Report</h4>
          <button type="button" className="btn-close" onClick={onClose}></button>
        </div>
        
        <div className="card-body p-4 overflow-y-auto">
          {fetching ? (
            <div className="d-flex justify-content-center py-5">
              <LoadingSpinner message="Loading Report..." />
            </div>
          ) : error ? (
            <div className="alert alert-danger p-3 rounded-3 text-center" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              {error}
            </div>
          ) : reportDetails ? (
            <div>
              
              <div className="row mb-4 border-bottom pb-3">
                <div className="col-md-6 mb-3 mb-md-0">
                  <small className="text-muted d-block fw-semibold text-uppercase" style={{ letterSpacing: '0.5px' }}>Patient Name</small>
                  <span className="fw-bold text-dark fs-5">{reportDetails.patientName}</span>
                </div>
                <div className="col-md-6 text-md-end">
                  <small className="text-muted d-block fw-semibold text-uppercase" style={{ letterSpacing: '0.5px' }}>Consultation Date</small>
                  <span className="fw-bold text-dark fs-5">{new Date(reportDetails.appointmentDate).toLocaleDateString()}</span>
                </div>
              </div>

              
              <div className="mb-4">
                <h6 className="fw-bold text-secondary">Current Symptoms:</h6>
                <p className="bg-light p-3 rounded-3 border text-dark">{reportDetails.currentSymptoms}</p>
              </div>

              <div className="mb-4">
                <h6 className="fw-bold text-secondary">Physical Examination:</h6>
                <p className="bg-light p-3 rounded-3 border text-dark">{reportDetails.physicalExamination}</p>
              </div>

              <div className="mb-4">
                <h6 className="fw-bold text-secondary">Recommended Medical Tests:</h6>
                <p className="bg-light p-3 rounded-3 border text-dark">{reportDetails.medicalTest || 'None recommended'}</p>
              </div>

              <div className="mb-4">
                <h6 className="fw-bold text-secondary">Treatment & Management Plan:</h6>
                <p className="bg-light p-3 rounded-3 border text-dark">{reportDetails.treatmentPlan}</p>
              </div>

              
              <div className="mt-5 pt-4 border-top">
                <h5 className="fw-bold mb-3" style={{ color: maroonColor }}>Medications Prescribed:</h5>
                {reportDetails.prescriptions && reportDetails.prescriptions.length > 0 ? (
                  <div className="table-responsive border rounded-3">
                    <table className="table table-striped table-hover mb-0">
                      <thead className="table-light text-secondary">
                        <tr>
                          <th className="py-3 px-3">Medicine Name</th>
                          <th className="py-3 px-3">Dosage</th>
                          <th className="py-3 px-3">Instructions</th>
                        </tr>
                      </thead>
                      <tbody className="border-top-0">
                        {reportDetails.prescriptions.map((pres, idx) => (
                          <tr key={idx}>
                            <td className="py-3 px-3 fw-semibold text-dark">{pres.medicineName}</td>
                            <td className="py-3 px-3 text-muted">{pres.dosage}</td>
                            <td className="py-3 px-3 text-muted">{pres.instructions}</td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                ) : (
                  <p className="text-muted fst-italic bg-light p-3 rounded-3 border">No medications prescribed yet.</p>
                )}
              </div>

            </div>
          ) : (
            <div className="text-center text-muted py-5">
              <i className="bi bi-file-earmark-x fs-1 d-block mb-3"></i>
              No report details available.
            </div>
          )}
        </div>
        
        <div className="card-footer bg-white border-top p-4 d-flex justify-content-end">
          <button type="button" className="btn btn-outline-secondary px-4 fw-bold shadow-sm" onClick={onClose}>Close Report</button>
        </div>
        
      </div>
    </div>
  );
}

export default ConsultationReportModel;