import React, { useState } from 'react';
import PatientForm from '../Forms/PatientForm';
import { registerPatientUser } from '../../api/auth';
import { updatePatient, removePatient } from '../../api/patient';

function PatientModel({ mode, patient, onClose, onSuccess, isModal = true }) {
  const [submitLoading, setSubmitLoading] = useState(false);
  const [formError, setFormError] = useState(null);

  const maroonColor = '#8b1031';

  const handleSubmit = async (formData) => {
    setSubmitLoading(true);
    setFormError(null);
    try {
      if (mode === 'add') {
        const response = await registerPatientUser(formData);
        if (response && response.success) {
          alert('Patient registered successfully.');
          if (onSuccess) onSuccess();
        } else {
          setFormError(response?.message || 'Failed to register patient.');
        }
      } else {
        await updatePatient({
          ...formData,
          patientId: patient.patientId,
          userId: patient.userId
        });
        alert('Patient details updated successfully.');
        if (onSuccess) onSuccess();
      }
    } catch (err) {
      console.error(err);
      setFormError(err.message || `Failed to ${mode} patient.`);
    } finally {
      setSubmitLoading(false);
    }
  };

  const handleDelete = async () => {
    setSubmitLoading(true);
    setFormError(null);
    try {
      await removePatient(patient.patientId);
      alert('Patient removed successfully.');
      if (onSuccess) onSuccess();
    } catch (err) {
      console.error(err);
      setFormError(err.message || 'Failed to remove patient.');
    } finally {
      setSubmitLoading(false);
    }
  };

  const renderContent = () => {
    if (mode === 'delete') {
      return (
        <div>
          <p className="text-dark mb-4">
            Are you sure you want to remove patient <strong>{patient.fullName}</strong> from the system? This action is permanent and cannot be undone.
          </p>
          <div className="d-flex gap-2 justify-content-end">
            <button 
              type="button" 
              className="btn btn-outline-secondary fw-bold px-4" 
              onClick={onClose}
              disabled={submitLoading}
            >
              No, Cancel
            </button>
            <button 
              type="button" 
              className="btn btn-danger fw-bold px-4 shadow-sm" 
              onClick={handleDelete}
              disabled={submitLoading}
            >
              {submitLoading ? 'Removing...' : 'Yes, Remove'}
            </button>
          </div>
        </div>
      );
    }

    return (
      <PatientForm
        initialValues={mode === 'edit' ? patient : null}
        onSubmit={handleSubmit}
        onCancel={onClose}
        loading={submitLoading}
        mode={mode}
      />
    );
  };

  const getTitle = () => {
    if (mode === 'delete') return 'Remove Patient';
    return mode === 'edit' ? 'Edit Patient Details' : 'Register New Patient';
  };

  const formElement = (
    <>
      {formError && <div className="alert alert-danger p-2 mb-3 small">{formError}</div>}
      {renderContent()}
    </>
  );

  if (!isModal) {
    return formElement;
  }

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      <div className="card border-0 shadow-lg w-100 mx-3 rounded-4" style={{ maxWidth: mode === 'delete' ? '450px' : mode === 'edit' ? '500px' : '600px', maxHeight: '90vh', overflowY: 'auto' }}>
        <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center pt-4 px-4 pb-0">
          <h5 className="fw-bold mb-0" style={{ color: maroonColor }}>
            {getTitle()}
          </h5>
          <button type="button" className="btn-close" onClick={onClose} disabled={submitLoading}></button>
        </div>
        <div className="card-body px-4 pb-4">
          {formElement}
        </div>
      </div>
    </div>
  );
}

export default PatientModel;
