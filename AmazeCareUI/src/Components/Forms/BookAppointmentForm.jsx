import React, { useState } from 'react';
import LoadingSpinner from '../Common/LoadingSpinner';

const timeSlots = [
  { label: '09:00 AM', value: '09:00' },
  { label: '09:30 AM', value: '09:30' },
  { label: '10:00 AM', value: '10:00' },
  { label: '10:30 AM', value: '10:30' },
  { label: '11:00 AM', value: '11:00' },
  { label: '11:30 AM', value: '11:30' },
  { label: '12:00 PM', value: '12:00' },
  { label: '12:30 PM', value: '12:30' },
  { label: '01:00 PM', value: '13:00' },
  { label: '01:30 PM', value: '13:30' },
  { label: '02:00 PM', value: '14:00' },
  { label: '02:30 PM', value: '14:30' },
  { label: '03:00 PM', value: '15:00' },
  { label: '03:30 PM', value: '15:30' },
  { label: '04:00 PM', value: '16:00' },
  { label: '04:30 PM', value: '16:30' },
  { label: '05:00 PM', value: '17:00' },
  { label: '05:30 PM', value: '17:30' },
  { label: '06:00 PM', value: '18:00' }
];

function BookAppointmentForm({
  doctors,
  initialDoctorId = '',
  fetchingDocs,
  onSubmit,
  onCancel,
  loading,
  showPatientSelect = false,
  patients = []
}) {
  const [selectedDoctorId, setSelectedDoctorId] = useState(initialDoctorId);
  const [selectedPatientId, setSelectedPatientId] = useState('');
  const [appointmentDate, setAppointmentDate] = useState('');
  const [preferedTime, setPreferedTime] = useState('');
  const [natureOfVisit, setNatureOfVisit] = useState('Consultation');
  const [symptomsDescription, setSymptomsDescription] = useState('');

  const maroonColor = '#8b1031';

  const handleSubmit = (e) => {
    e.preventDefault();
    const data = {
      selectedDoctorId,
      appointmentDate,
      preferedTime,
      natureOfVisit,
      symptomsDescription
    };
    if (showPatientSelect) {
      data.selectedPatientId = selectedPatientId;
    }
    onSubmit(data);
  };

  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 text-start">
      <form onSubmit={handleSubmit}>


        {showPatientSelect && (
          <div className="mb-4">
            <label className="form-label fw-bold text-secondary">Select Patient</label>
            <select
              className="form-select bg-light border p-2 shadow-none"
              value={selectedPatientId}
              onChange={(e) => setSelectedPatientId(e.target.value)}
              required
            >
              <option value="" disabled>-- Select Patient --</option>
              {patients.map((pat) => (
                <option key={pat.patientId} value={pat.patientId}>
                  {pat.fullName} (ID: {pat.patientId})
                </option>
              ))}
            </select>
          </div>
        )}

        <div className="mb-4">
          <label className="form-label fw-bold text-secondary">Select Specialist</label>
          {fetchingDocs ? (
            <div className="d-flex align-items-center"><LoadingSpinner small={true} /></div>
          ) : (
            <select
              className="form-select bg-light border p-2 shadow-none"
              value={selectedDoctorId}
              onChange={(e) => setSelectedDoctorId(e.target.value)}
              required
            >
              <option value="" disabled>-- Select Doctor --</option>
              {doctors.map((doc) => (
                <option key={doc.doctorId} value={doc.doctorId}>
                  {doc.doctorName} ({doc.speciality})
                </option>
              ))}
            </select>
          )}
        </div>


        <div className="row mb-4">
          <div className="col-md-6 mb-3 mb-md-0">
            <label className="form-label fw-bold text-secondary">Preferred Date</label>
            <input
              type="date"
              className="form-control bg-light border p-2 shadow-none"
              value={appointmentDate}
              onChange={(e) => setAppointmentDate(e.target.value)}
              min={new Date().toISOString().split('T')[0]}
              required
            />
          </div>
          <div className="col-md-6">
            <label className="form-label fw-bold text-secondary">Preferred Time</label>
            <select
              className="form-select bg-light border p-2 shadow-none"
              value={preferedTime}
              onChange={(e) => setPreferedTime(e.target.value)}
              required
            >
              <option value="" disabled>-- Select Time --</option>
              {timeSlots.map((slot) => (
                <option key={slot.value} value={slot.value}>
                  {slot.label}
                </option>
              ))}
            </select>
          </div>
        </div>


        <div className="mb-4">
          <label className="form-label fw-bold text-secondary">Nature of Visit</label>
          <select
            className="form-select bg-light border p-2 shadow-none"
            value={natureOfVisit}
            onChange={(e) => setNatureOfVisit(e.target.value)}
            required
          >
            <option value="Consultation">Consultation</option>
            <option value="Follow-up">Follow-up</option>
            <option value="Emergency">Emergency</option>
            <option value="Routine Checkup">Routine Checkup</option>
          </select>
        </div>


        <div className="mb-4">
          <label className="form-label fw-bold text-secondary">Describe Symptoms / Reasons</label>
          <textarea
            className="form-control bg-light border p-2 shadow-none"
            rows="4"
            placeholder="Briefly explain your symptoms or reason for visit..."
            value={symptomsDescription}
            onChange={(e) => setSymptomsDescription(e.target.value)}
            required
          ></textarea>
        </div>


        <div className="d-flex gap-3 mt-2">
          <button
            type="submit"
            className="btn text-white fw-bold px-4 py-2 shadow-sm"
            disabled={loading}
            style={{ backgroundColor: maroonColor }}
          >
            {loading ? 'Booking...' : 'Request Appointment'}
          </button>
          <button
            type="button"
            onClick={onCancel}
            className="btn btn-outline-secondary fw-bold px-4 py-2 shadow-sm"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
};

export default BookAppointmentForm;