import React, { useState, useEffect } from 'react';

export default function PatientForm({ initialValues, onSubmit, onCancel, loading, mode = 'edit' }) {
  const [fullName, setFullName] = useState('');
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [email, setEmail] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState('');
  const [gender, setGender] = useState('');
  const [contactNumber, setContactNumber] = useState('');

  useEffect(() => {
    if (initialValues) {
      setFullName(initialValues.fullName || '');
      setDateOfBirth(initialValues.dateOfBirth?.split('T')[0] || initialValues.dateOfBirth || '');
      setGender(initialValues.gender || '');
      setContactNumber(initialValues.contactNumber || '');
    }
  }, [initialValues]);

  const handleSubmit = (e) => {
    e.preventDefault();
    const payload = {
      fullName,
      dateOfBirth,
      gender,
      contactNumber
    };
    if (mode === 'add') {
      payload.userName = userName;
      payload.password = password;
      payload.email = email;
    }
    onSubmit(payload);
  };

  const maroonColor = '#8b1031';

  return (
    <form onSubmit={handleSubmit} className="text-start">
      <div className="row g-3">
        {mode === 'add' ? (
          <>
            <div className="col-12 col-md-6">
              <label className="form-label fw-semibold text-secondary">Full Name</label>
              <input type="text" className="form-control" value={fullName} onChange={e => setFullName(e.target.value)} placeholder="John Doe" required />
            </div>
            <div className="col-12 col-md-6">
              <label className="form-label fw-semibold text-secondary">Username</label>
              <input type="text" className="form-control" value={userName} onChange={e => setUserName(e.target.value)} placeholder="johndoe" required />
            </div>
            <div className="col-12 col-md-6">
              <label className="form-label fw-semibold text-secondary">Password</label>
              <input type="password" className="form-control" value={password} onChange={e => setPassword(e.target.value)} placeholder="••••••••" required />
            </div>
            <div className="col-12 col-md-6">
              <label className="form-label fw-semibold text-secondary">Email Address</label>
              <input type="email" className="form-control" value={email} onChange={e => setEmail(e.target.value)} placeholder="john@example.com" required />
            </div>
          </>
        ) : (
          <div className="col-12">
            <label className="form-label fw-semibold text-secondary">Full Name</label>
            <input type="text" className="form-control" value={fullName} onChange={e => setFullName(e.target.value)} required />
          </div>
        )}
        <div className="col-12 col-md-6">
          <label className="form-label fw-semibold text-secondary">Date of Birth</label>
          <input type="date" className="form-control" value={dateOfBirth} onChange={e => setDateOfBirth(e.target.value)} required />
        </div>
        <div className="col-12 col-md-6">
          <label className="form-label fw-semibold text-secondary">Gender</label>
          <select className="form-select" value={gender} onChange={e => setGender(e.target.value)} required>
            <option value="" disabled>-- Select Gender --</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
        </div>
        <div className="col-12">
          <label className="form-label fw-semibold text-secondary">Contact Number</label>
          <input type="text" className="form-control" value={contactNumber} onChange={e => setContactNumber(e.target.value)} required />
        </div>
      </div>

      <div className="d-flex justify-content-end gap-2 mt-4">
        <button type="button" className="btn btn-outline-secondary fw-bold" onClick={onCancel}>Cancel</button>
        <button type="submit" className="btn text-white fw-bold shadow-sm" disabled={loading} style={{ backgroundColor: maroonColor, color: '#ffffff' }}>
          {loading ? 'Saving...' : mode === 'add' ? 'Register' : 'Save Changes'}
        </button>
      </div>
    </form>
  );
}
