import React, { useState, useEffect } from 'react';
import MedicalRecordForm from '../Forms/MedicalRecordForm';
import LoadingSpinner from '../Common/LoadingSpinner';
import { getReportForRecord, addMedicalRecord, updateMedicalRecord } from '../../api/medicalRecords';
import { updateAppointmentStatus } from '../../api/appointment';

export default function DiagnosisModel({ onClose, appointment, onSuccess }) {
  const [loading, setLoading] = useState(false);
  const [initialValues, setInitialValues] = useState(null);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (appointment && appointment.status === 'Completed' && appointment.recordId) {
      loadExistingRecord();
    }
  }, [appointment]);

  const loadExistingRecord = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await getReportForRecord(appointment.recordId);
      if (response && response.statusCode === 200 && response.Data) {
        setInitialValues({
          symptoms: response.Data.CurrentSymptoms,
          physicalExam: response.Data.PhysicalExamination,
          medicalTest: response.Data.RecommendedTests,
          treatmentPlan: response.Data.TreatmentPlan
        });
      } else {
        setError('Failed to load initial medical record values.');
      }
    } catch (err) {
      console.error(err);
      setError('Failed to load initial medical record values.');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async ({ symptoms, physicalExam, medicalTest, treatmentPlan }) => {
    setLoading(true);
    setError(null);

    try {
      if (appointment.status !== 'Completed') {
        
        const response = await addMedicalRecord({
          appointmentId: appointment.appointmentId,
          currentSymptoms: symptoms,
          physicalExamination: physicalExam,
          medicalTest,
          treatmentPlan
        });

        if (response && response.success) {
          
          await updateAppointmentStatus({
            appointmentId: appointment.appointmentId,
            status: 'Completed'
          });
          
          alert('Medical Record saved successfully.');
          if (onSuccess) onSuccess();
          onClose();
        } else {
          setError(response.message || 'Failed to add record.');
        }
      } else {
        
        const response = await updateMedicalRecord({
          recordId: appointment.recordId,
          currentSymptoms: symptoms,
          physicalExamination: physicalExam,
          medicalTest,
          treatmentPlan
        });

        if (response && response.success) {
          alert('Medical Record updated successfully.');
          if (onSuccess) onSuccess();
          onClose();
        } else {
          setError(response.message || 'Failed to update record.');
        }
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Error occurred while saving medical record.');
    } finally {
      setLoading(false);
    }
  };

  if (!appointment) return null;

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style={{ backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 1050 }}>
      <div className="card border-0 shadow-lg w-100 mx-3 rounded-4" style={{ maxWidth: '800px' }}>
        <div className="card-header bg-white border-bottom-0 d-flex justify-content-between align-items-center pt-4 px-4 pb-0">
          <h5 className="fw-bold mb-0" style={{ color: '#8b1031' }}>
            {appointment.status === 'Completed' ? 'Edit Diagnosis Record' : 'Create Diagnosis Record'}
          </h5>
          <button type="button" className="btn-close" onClick={onClose}></button>
        </div>
        <div className="card-body px-4 pb-4">
          {error && <div className="alert alert-danger p-2 mb-3" style={{ fontSize: '14px' }}>{error}</div>}
          
          {loading && !initialValues && appointment.status === 'Completed' ? (
            <div className="d-flex justify-content-center py-4">
              <LoadingSpinner message="Retrieving record detail..." />
            </div>
          ) : (
            <MedicalRecordForm
              appointment={appointment}
              initialValues={initialValues}
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
