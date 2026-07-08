import React from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import BookAppointmentModel from '../../Components/Models/BookAppointmentModel';

function BookAppointment() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const preselectedDoctorId = searchParams.get('doctorId');
  const maroonColor = '#8b1031';

  return (
    <div className="container py-5 text-start">
      <div className="row justify-content-center">
        <div className="col-md-8 col-lg-6">
          <div className="card shadow-sm border-0 rounded-4 p-4 mt-3">
            <div className="d-flex align-items-center mb-4 pb-2 border-bottom">
              <i className="bi bi-calendar-plus fs-2 me-3" style={{ color: maroonColor }}></i>
              <div>
                <h3 className="fw-bold mb-0" style={{ color: maroonColor }}>Book Appointment</h3>
                <p className="text-muted mb-0 mt-1" style={{ fontSize: '0.9rem' }}>Schedule a visit with our specialists</p>
              </div>
            </div>

            <BookAppointmentModel
              isModal={false}
              showPatientSelect={false}
              initialDoctorId={preselectedDoctorId || ''}
              onSuccess={() => navigate('/patient/appointments')}
              onCancel={() => navigate('/patient')}
            />
          </div>
        </div>
      </div>
    </div>
  );
}

export default BookAppointment;