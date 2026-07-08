import React, { useState } from 'react';

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

function RescheduleForm({ appointment, onSubmit, onCancel, loading }) {
  const initialDate = appointment?.appointmentDate ? appointment.appointmentDate.split('T')[0] : '';
  
  const [newDate, setNewDate] = useState(initialDate);
  const [newTime, setNewTime] = useState(appointment?.preferedTime ? appointment.preferedTime.slice(0, 5) : '');
  
  const maroonColor = '#8b1031';

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({
      newDate,
      newTime
    });
  };

  return (
    <form onSubmit={handleSubmit} className="p-3">
      <p className="mb-4 text-secondary">
        Rescheduling appointment for: <strong className="text-dark">{appointment?.doctorName || appointment?.patientName}</strong>.
      </p>

      <div className="mb-3">
        <label className="form-label fw-bold text-secondary">New Date</label>
        <input
          type="date"
          className="form-control p-2"
          value={newDate}
          onChange={(e) => setNewDate(e.target.value)}
          min={new Date().toISOString().split('T')[0]} 
          required
        />
      </div>

      <div className="mb-4">
        <label className="form-label fw-bold text-secondary">New Time</label>
        <select
          className="form-select p-2"
          value={newTime}
          onChange={(e) => setNewTime(e.target.value)}
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

      <div className="d-flex gap-2">
        <button 
          type="submit" 
          className="btn text-white fw-bold px-4 py-2" 
          disabled={loading}
          style={{ backgroundColor: maroonColor }}
        >
          {loading ? 'Processing...' : 'Confirm Reschedule'}
        </button>
        <button 
          type="button" 
          className="btn btn-outline-secondary px-4 py-2" 
          onClick={onCancel}
        >
          Close
        </button>
      </div>
    </form>
  );
}

export default RescheduleForm;