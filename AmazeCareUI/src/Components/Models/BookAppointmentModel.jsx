import React, { useState, useEffect } from 'react';
import LoadingSpinner from '../Common/LoadingSpinner';
import BookAppointmentForm from '../Forms/BookAppointmentForm';
import { useAuth } from '../../Context/AuthContext';
import { createNewAppointment } from '../../api/appointment';
import { getAllPatients } from '../../api/patient';
import { getAllDoctors } from '../../api/doctor';

function BookAppointmentModel({ 
  isModal = true, 
  showPatientSelect = false, 
  initialDoctorId = '', 
  onClose, 
  onSuccess 
}) {
  const { profileId } = useAuth();
  const [patients, setPatients] = useState([]);
  const [doctors, setDoctors] = useState([]);
  const [registriesLoading, setRegistriesLoading] = useState(false);
  const [submitLoading, setSubmitLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [success, setSuccess] = useState(false);

  const maroonColor = '#8b1031';

  useEffect(() => {
    loadRegistriesData();
  }, []);

  const loadRegistriesData = async () => {
    setRegistriesLoading(true);
    try {
      const doctorsData = await getAllDoctors();
      if (Array.isArray(doctorsData)) {
        setDoctors(doctorsData);
      }
      if (showPatientSelect) {
        const patientsData = await getAllPatients();
        if (Array.isArray(patientsData)) {
          setPatients(patientsData);
        }
      }
    } catch (err) {
      console.error('Failed to load registries:', err);
    } finally {
      setRegistriesLoading(false);
    }
  };

  const handleAddSubmit = async (formData) => {
    setSubmitLoading(true);
    setFormError(null);
    try {
      const finalPatientId = showPatientSelect 
        ? parseInt(formData.selectedPatientId, 10) 
        : profileId;
      
      const payload = {
        patientId: finalPatientId,
        doctorId: parseInt(formData.selectedDoctorId, 10),
        appointmentDate: formData.appointmentDate,
        preferedTime: formData.preferedTime + ":00",
        natureOfVisit: formData.natureOfVisit,
        symptomsDescription: formData.symptomsDescription,
        status: showPatientSelect ? 'Upcoming' : 'Requested'
      };

      const response = await createNewAppointment(payload);
      if (response && (response.statusCode === 200 || response.statusCode === 201)) {
        setSuccess(true);
        setTimeout(() => {
          setSuccess(false);
          if (onSuccess) onSuccess();
        }, 1500);
      } else {
        setFormError(response?.message || 'Failed to book appointment.');
      }
    } catch (err) {
      console.error(err);
      setFormError(err.message || 'Failed to book appointment.');
    } finally {
      setSubmitLoading(false);
    }
  };

  const handleCancel = () => {
    if (onClose) onClose();
  };

  const formElement = (
    <>
      {success && (
        <div className="alert alert-success d-flex align-items-center p-3 mb-4 rounded-3 shadow-sm" role="alert">
          <i className="bi bi-check-circle-fill me-3 fs-4"></i>
          <div className="fw-bold">Appointment Added Successfully!</div>
        </div>
      )}

      {formError && <div className="alert alert-danger p-2 mb-3 small">{formError}</div>}

      {registriesLoading ? (
        <div className="d-flex justify-content-center py-5">
          <LoadingSpinner message="Loading patients & doctors..." />
        </div>
      ) : (
        <BookAppointmentForm
          showPatientSelect={showPatientSelect}
          patients={patients}
          doctors={doctors}
          initialDoctorId={initialDoctorId}
          onSubmit={handleAddSubmit}
          onCancel={handleCancel}
          loading={submitLoading}
        />
      )}
    </>
  );

  if (!isModal) {
    return formElement;
  }

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      <div className="card border-0 shadow-lg w-100 mx-3 rounded-4" style={{ maxWidth: '600px', maxHeight: '90vh', overflowY: 'auto' }}>
        <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center pt-4 px-4 pb-0">
          <h5 className="fw-bold mb-0" style={{ color: maroonColor }}>Book New Appointment</h5>
          <button type="button" className="btn-close" onClick={handleCancel}></button>
        </div>
        <div className="card-body px-4 pb-4">
          {formElement}
        </div>
      </div>
    </div>
  );
}

export default BookAppointmentModel;
