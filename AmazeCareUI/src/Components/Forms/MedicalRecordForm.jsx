import React, { useState, useEffect } from 'react';


function MedicalRecordForm({ appointment, initialValues, onSubmit, onCancel, loading }) {
  
  const [symptoms, setSymptoms] = useState(initialValues?.symptoms || '');
  const [physicalExam, setPhysicalExam] = useState(initialValues?.physicalExam || '');
  const [medicalTest, setMedicalTest] = useState(initialValues?.medicalTest || '');
  const [treatmentPlan, setTreatmentPlan] = useState(initialValues?.treatmentPlan || '');

  
  useEffect(() => {
    if (initialValues) {
      setSymptoms(initialValues.symptoms || '');
      setPhysicalExam(initialValues.physicalExam || '');
      setMedicalTest(initialValues.medicalTest || '');
      setTreatmentPlan(initialValues.treatmentPlan || '');
    }
  }, [initialValues]);

  
  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({
      symptoms,
      physicalExam,
      medicalTest,
      treatmentPlan
    });
  };

  return (
    <form onSubmit={handleSubmit} className="text-start">
      <p className="mb-3 text-muted">Patient Name: <strong className="text-dark">{appointment?.patientName}</strong></p>
      
      
      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">Current Symptoms</label>
        <textarea
          className="form-control"
          rows="3"
          value={symptoms}
          onChange={(e) => setSymptoms(e.target.value)}
          placeholder="e.g. High fever, headache, body aches"
          required
        ></textarea>
      </div>
 
      {/* 2. Physical examination records */}
      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">Physical Examination Findings</label>
        <textarea
          className="form-control"
          rows="3"
          value={physicalExam}
          onChange={(e) => setPhysicalExam(e.target.value)}
          placeholder="e.g. Temperature 101.5°F, BP 120/80, normal chest sounds"
          required
        ></textarea>
      </div>
 
      
      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">Recommended Medical Tests</label>
        <input
          type="text"
          className="form-control"
          value={medicalTest}
          onChange={(e) => setMedicalTest(e.target.value)}
          placeholder="e.g. CBC, Widal blood test (leave blank if none)"
        />
      </div>
 
      {/* 4. Treatment and therapy instructions */}
      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">Treatment & Management Plan</label>
        <textarea
          className="form-control"
          rows="3"
          value={treatmentPlan}
          onChange={(e) => setTreatmentPlan(e.target.value)}
          placeholder="e.g. Complete bed rest for 3 days, drink lots of fluids"
          required
        ></textarea>
      </div>
 
      
      <div className="d-flex justify-content-end gap-2 mt-4">
        <button type="button" className="btn btn-outline-secondary fw-bold shadow-sm" onClick={onCancel}>Cancel</button>
        <button type="submit" className="btn btn-success fw-bold shadow-sm" disabled={loading} style={{ backgroundColor: '#8b1031', borderColor: '#8b1031', color: '#ffffff' }}>
          {loading ? 'Saving...' : 'Save & Submit'}
        </button>
      </div>
    </form>
  );
};

export default MedicalRecordForm;
