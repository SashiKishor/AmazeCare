import React from 'react';
import AppointmentTable from '../../Components/Table/AppointmentTable';

export default function ManageAppointments() {
  const maroonColor = '#8b1031';

  return (
    <div className="container py-5 text-start">
      <div className="mb-4">
        <h2 className="fw-bold mb-0" style={{ color: maroonColor }}>Manage Appointments</h2>
      </div>

      <AppointmentTable
        showPatientColumn={true}
        showDoctorColumn={true}
        role="admin"
      />
    </div>
  );
}
