import React, { useState } from 'react';
import LoadingSpinner from '../Common/LoadingSpinner';
import { updateAppointmentStatus } from '../../api/appointment';

function StatusModel({ appointment, newStatus, onClose, onSuccess }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const maroonColor = '#8b1031';

  if (!appointment) return null;

  const handleConfirm = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await updateAppointmentStatus({
        appointmentId: appointment.appointmentId,
        status: newStatus
      });

      if (response && (response.statusCode === 200 || response.statusCode === 201)) {
        alert(`Appointment successfully updated to "${newStatus}".`);
        if (onSuccess) onSuccess();
      } else {
        setError(response?.message || 'Failed to update appointment status.');
      }
    } catch (err) {
      console.error(err);
      setError('An error occurred while communicating with the server.');
    } finally {
      setLoading(false);
    }
  };

  const getStatusActionText = () => {
    switch (newStatus) {
      case 'Cancelled':
        return 'cancel/decline';
      case 'Upcoming':
        return 'accept';
      default:
        return `change the status to "${newStatus}" for`;
    }
  };

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      <div className="card border-0 shadow-lg w-100 mx-3 rounded-4" style={{ maxWidth: '450px' }}>
        <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center pt-4 px-4 pb-0">
          <h5 className="fw-bold mb-0" style={{ color: maroonColor }}>Confirm Action</h5>
          <button type="button" className="btn-close" onClick={onClose} disabled={loading}></button>
        </div>
        <div className="card-body px-4 pb-4">
          {error && <div className="alert alert-danger p-2 mb-3 small">{error}</div>}

          {loading ? (
            <div className="d-flex justify-content-center py-4">
              <LoadingSpinner message="Updating status..." />
            </div>
          ) : (
            <div>
              <p className="text-dark mb-4">
                Are you sure you want to <strong>{getStatusActionText()}</strong> this appointment for patient <strong>{appointment.patientName}</strong>?
              </p>
              <div className="d-flex gap-2 justify-content-end">
                <button 
                  type="button" 
                  className="btn btn-outline-secondary fw-bold px-4" 
                  onClick={onClose}
                >
                  No, Cancel
                </button>
                <button 
                  type="button" 
                  className="btn text-white fw-bold px-4 shadow-sm" 
                  style={{ backgroundColor: maroonColor }}
                  onClick={handleConfirm}
                >
                  Yes, Confirm
                </button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default StatusModel;
