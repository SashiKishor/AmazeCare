import React, { useState } from 'react';
import DoctorForm from '../Forms/DoctorForm';
import { createDoctorOrAdminUser } from '../../api/auth';
import { updateDoctor, removeDoctor } from '../../api/doctor';

function DoctorModel({ mode, doctor, onClose, onSuccess }) {
  const [submitLoading, setSubmitLoading] = useState(false);
  const [formError, setFormError] = useState(null);

  const maroonColor = '#8b1031';

  const handleSubmit = async (formData) => {
    setSubmitLoading(true);
    setFormError(null);
    try {
      if (mode === 'add') {
        await createDoctorOrAdminUser({
          ...formData,
          role: 'Doctor'
        });
        alert('Doctor registered successfully.');
        if (onSuccess) onSuccess();
      } else {
        await updateDoctor({
          doctorId: doctor.doctorId,
          doctorName: formData.fullName,
          speciality: formData.speciality,
          experience: formData.experience,
          qualification: formData.qualification,
          designation: formData.designation,
          userId: doctor.userId
        });
        alert('Doctor details updated successfully.');
        if (onSuccess) onSuccess();
      }
    } catch (err) {
      console.error(err);
      setFormError(err.message || `Failed to ${mode} doctor.`);
    } finally {
      setSubmitLoading(false);
    }
  };

  const handleDelete = async () => {
    setSubmitLoading(true);
    setFormError(null);
    try {
      await removeDoctor(doctor.doctorId);
      alert('Doctor removed successfully.');
      if (onSuccess) onSuccess();
    } catch (err) {
      console.error(err);
      setFormError(err.message || 'Failed to remove doctor.');
    } finally {
      setSubmitLoading(false);
    }
  };

  const getInitialValues = () => {
    if (mode === 'edit' && doctor) {
      return {
        fullName: doctor.doctorName,
        speciality: doctor.speciality,
        experience: doctor.experience,
        qualification: doctor.qualification,
        designation: doctor.designation
      };
    }
    return null;
  };

  const renderContent = () => {
    if (mode === 'delete') {
      return (
        <div>
          <p className="text-dark mb-4">
            Are you sure you want to remove doctor <strong>{doctor.doctorName}</strong> from the system? This action is permanent and cannot be undone.
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
      <DoctorForm
        initialValues={getInitialValues()}
        onSubmit={handleSubmit}
        onCancel={onClose}
        loading={submitLoading}
        mode={mode}
      />
    );
  };

  const getTitle = () => {
    if (mode === 'delete') return 'Remove Doctor';
    return mode === 'edit' ? 'Edit Doctor Details' : 'Register New Doctor';
  };

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      <div className="card border-0 shadow-lg w-100 mx-3 rounded-4" style={{ maxWidth: mode === 'delete' ? '450px' : '600px', maxHeight: '90vh', overflowY: 'auto' }}>
        <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center pt-4 px-4 pb-0">
          <h5 className="fw-bold mb-0" style={{ color: maroonColor }}>
            {getTitle()}
          </h5>
          <button type="button" className="btn-close" onClick={onClose} disabled={submitLoading}></button>
        </div>
        <div className="card-body px-4 pb-4">
          {formError && <div className="alert alert-danger p-2 mb-3 small">{formError}</div>}

          {renderContent()}
        </div>
      </div>
    </div>
  );
}

export default DoctorModel;
