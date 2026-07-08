import React from 'react';
import { useAuth } from '../../Context/AuthContext';

function DoctorCard({ doctor, onBook }) {
  const { isAuthenticated, user } = useAuth();
  
  const maroonColor = '#8b1031';
  
  return (
    <div className="card h-100 border-0 shadow-sm rounded-4 text-start transition-all">
      <div className="card-body d-flex flex-column p-4">
        
        <div className="mb-4">
          <h5 className="fw-bold mb-1" style={{ color: maroonColor }}>{doctor.doctorName}</h5>
          <small className="text-muted fw-semibold">{doctor.designation}</small>
        </div>
        
        <p className="card-text mb-2">
          <strong className="text-secondary">Specialty:</strong> {doctor.speciality}
        </p>
        
        <p className="card-text mb-2">
          <strong className="text-secondary">Qualification:</strong> {doctor.qualification}
        </p>
        
        <p className="card-text mb-4">
          <strong className="text-secondary">Experience:</strong> {doctor.experience} Years
        </p>

        <button
          onClick={() => onBook(doctor.doctorId)}
          className="btn w-100 fw-bold p-2 mt-auto shadow-sm"
          style={{ backgroundColor: maroonColor, color: '#ffffff', borderRadius: '8px' }}
        >
          Book Appointment
        </button>
        
      </div>
    </div>
  );
}

export default DoctorCard;