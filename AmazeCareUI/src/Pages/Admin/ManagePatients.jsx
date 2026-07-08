import React from 'react';
import PatientTable from '../../Components/Table/PatientTable';

export default function ManagePatients() {
  const maroonColor = '#8b1031';

  return (
    <div className="container py-5 text-start">
      <div className="mb-4">
        <h2 className="fw-bold mb-0" style={{ color: maroonColor }}>Manage Patients</h2>
      </div>

      <PatientTable />
    </div>
  );
}
