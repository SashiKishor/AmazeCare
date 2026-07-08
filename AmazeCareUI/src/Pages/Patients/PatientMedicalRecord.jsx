import React from 'react';
import AppointmentTable from '../../Components/Table/AppointmentTable';

function PatientMedicalRecords() {
  const maroonColor = '#8b1031';

  return (
    <div className="container py-5 text-start">
      <div className="mb-4">
        <h2 className="fw-bold mb-0" style={{ color: maroonColor }}>My Medical History & Records</h2>
        <p className="text-muted mt-1">View your past consultation summaries and prescriptions.</p>
      </div>

      <AppointmentTable
        role="patient"
        historyOnly={true}
        showDoctorColumn={true}
      />
    </div>
  );
}

export default PatientMedicalRecords;