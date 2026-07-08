import React, { useState } from 'react';
import RescheduleForm from '../Forms/RescheduleForm';
import { rescheduleAppointment } from '../../api/appointment';

export default function RescheduleModel({ onClose, appointment, onSuccess }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  if (!appointment) return null;

  const handleSubmit = async ({ newDate, newTime }) => {
    setLoading(true);
    setError(null);

    const today = new Date().toISOString().split('T')[0];
    if (newDate < today) {
      setError('Date cannot be in the past.');
      setLoading(false);
      return;
    }

    try {
      const payload = {
        appointmentId: appointment.appointmentId,
        rescheduledSlot: `${newDate}T${newTime}:00`
      };

      const result = await rescheduleAppointment(payload);
      if (result && (result.statusCode === 200 || result.statusCode === 201)) {
        setSuccess(true);
        if (onSuccess) {
          onSuccess(appointment.appointmentId, newDate, newTime);
        }
        setTimeout(() => {
          setSuccess(false);
          onClose();
        }, 2000);
      } else {
        setError(result.message || 'Failed to reschedule.');
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Error scheduling slot. Ensure the slot is vacant.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      <div className="card border-0 shadow-lg w-100 mx-3 rounded-4" style={{ maxWidth: '500px' }}>
        <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center pt-4 px-4 pb-0">
          <h5 className="fw-bold mb-0" style={{ color: '#8b1031' }}>Reschedule Appointment</h5>
          <button type="button" className="btn-close" onClick={onClose}></button>
        </div>
        <div className="card-body px-4 pb-4">
          {success && (
            <div className="alert alert-success p-2 mb-3 d-flex align-items-center" role="alert" style={{ fontSize: '14px' }}>
              <i className="bi bi-check-circle-fill me-2 text-success"></i>
              <div>Appointment rescheduled successfully.</div>
            </div>
          )}

          {error && (
            <div className="alert alert-danger p-2 mb-3 d-flex align-items-center" role="alert" style={{ fontSize: '14px' }}>
              <i className="bi bi-exclamation-circle-fill me-2"></i>
              <div>{error}</div>
            </div>
          )}

          {!success && (
            <RescheduleForm
              appointment={appointment}
              onSubmit={handleSubmit}
              onCancel={onClose}
              loading={loading}
            />
          )}
        </div>
      </div>
    </div>
  );
}
