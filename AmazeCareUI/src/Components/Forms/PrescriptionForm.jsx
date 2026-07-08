import React, { useState } from 'react';

function PrescriptionForm({ appointment, onSubmit, onCancel, loading }) {

  const [medicineName, setMedicineName] = useState('');
  const [dosage, setDosage] = useState('');
  const [instructions, setInstructions] = useState('');


  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({
      medicineName,
      dosage,
      instructions
    });
  };

  return (
    <form onSubmit={handleSubmit} className="text-start">
      <p className="mb-3 text-muted">Prescribing for Patient: <strong className="text-dark">{appointment?.patientName}</strong></p>


      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">Medicine Name</label>
        <input
          type="text"
          className="form-control"
          value={medicineName}
          onChange={(e) => setMedicineName(e.target.value)}
          placeholder="e.g. Paracetamol 500mg"
          required
        />
      </div>

      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">Dosage</label>
        <input
          type="text"
          className="form-control"
          value={dosage}
          onChange={(e) => setDosage(e.target.value)}
          placeholder="e.g. Twice a day (1-0-1)"
          required
        />
      </div>


      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">Special Instructions</label>
        <input
          type="text"
          className="form-control"
          value={instructions}
          onChange={(e) => setInstructions(e.target.value)}
          placeholder="e.g. Take after meals, continue for 5 days"
          required
        />
      </div>

      <div className="d-flex justify-content-end gap-2 mt-4">
        <button type="button" className="btn btn-outline-secondary fw-bold shadow-sm" onClick={onCancel}>Cancel</button>
        <button type="submit" className="btn btn-success fw-bold shadow-sm" disabled={loading} style={{ backgroundColor: '#8b1031', borderColor: '#8b1031', color: '#ffffff' }}>
          {loading ? 'Saving...' : 'Add Medication'}
        </button>
      </div>
    </form>
  );
};

export default PrescriptionForm;
