import React, { useState } from 'react';
import PrescriptionForm from '../Forms/PrescriptionForm';
import { addPrescription } from '../../api/prescription';

export default function PrescriptionModel({ onClose, appointment, onSuccess }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async ({ medicineName, dosage, instructions }) => {
    setLoading(true);
    setError(null);

    try {
      const response = await addPrescription({
        recordId: appointment.recordId,
        medicineName,
        dosage,
        instructions
      });

      if (response && (response.statusCode === 200 || response.statusCode === 201)) {
        alert('Medication prescribed successfully.');
        if (onSuccess) onSuccess();
        onClose();
      } else {
        setError(response.message || 'Failed to add prescription.');
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Error occurred while saving prescription.');
    } finally {
      setLoading(false);
    }
  };

  if (!appointment) return null;

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      <div className="card border-0 shadow-lg w-100 mx-3 rounded-4" style={{ maxWidth: '500px' }}>
        <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center pt-4 px-4 pb-0">
          <h5 className="fw-bold mb-0" style={{ color: '#8b1031' }}>Prescribe Medication</h5>
          <button type="button" className="btn-close" onClick={onClose}></button>
        </div>
        <div className="card-body px-4 pb-4">
          {error && <div className="alert alert-danger p-2 mb-3" style={{ fontSize: '14px' }}>{error}</div>}
          <PrescriptionForm
            appointment={appointment}
            onSubmit={handleSubmit}
            onCancel={onClose}
            loading={loading}
          />
        </div>
      </div>
    </div>
  );
}
