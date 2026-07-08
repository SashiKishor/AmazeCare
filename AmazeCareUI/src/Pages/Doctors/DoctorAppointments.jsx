import React from 'react';
import AppointmentTable from '../../Components/Table/AppointmentTable';

function DoctorAppointments() {
  const maroonColor = '#8b1031';

  return (
    <div className="container py-5 text-start">
      <div className="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center mb-4 gap-3">
        <h2 className="fw-bold mb-0" style={{ color: maroonColor }}>Patient Appointment Manager</h2>
      </div>

      <AppointmentTable
        role="doctor"
        showPatientColumn={true}
        showDoctorColumn={false}
      />
    </div>
  );
}

export default DoctorAppointments;
