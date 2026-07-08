import React from 'react';
import DoctorTable from '../../Components/Table/DoctorTable';

export default function ManageDoctors() {
  const maroonColor = '#8b1031';

  return (
    <div className="container py-5 text-start">
      <div className="mb-4">
        <h2 className="fw-bold mb-0" style={{ color: maroonColor }}>Manage Doctors</h2>
      </div>

      <DoctorTable />
    </div>
  );
}
