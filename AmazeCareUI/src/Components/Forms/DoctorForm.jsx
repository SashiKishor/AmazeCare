import React, { useState, useEffect } from 'react';

export default function DoctorForm({ initialValues, onSubmit, onCancel, loading, mode = 'add' }) {
  const [fullName, setFullName] = useState('');
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [email, setEmail] = useState('');
  const [speciality, setSpeciality] = useState('');
  const [experience, setExperience] = useState('');
  const [qualification, setQualification] = useState('');
  const [designation, setDesignation] = useState('');

  useEffect(() => {
    if (initialValues) {
      setFullName(initialValues.doctorName || initialValues.fullName || '');
      setSpeciality(initialValues.speciality || '');
      setExperience(initialValues.experience || '');
      setQualification(initialValues.qualification || '');
      setDesignation(initialValues.designation || '');
    }
  }, [initialValues]);

  const handleSubmit = (e) => {
    e.preventDefault();
    const payload = {
      fullName,
      speciality,
      experience: experience !== '' ? parseFloat(experience) : 0,
      qualification,
      designation
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
              <input type="text" className="form-control" value={fullName} onChange={e => setFullName(e.target.value)} placeholder="Dr. John Doe" required />
            </div>
            <div className="col-12 col-md-6">
              <label className="form-label fw-semibold text-secondary">Username</label>
              <input type="text" className="form-control" value={userName} onChange={e => setUserName(e.target.value)} placeholder="johndoe123" required />
            </div>
            <div className="col-12 col-md-6">
              <label className="form-label fw-semibold text-secondary">Password</label>
              <input type="password" className="form-control" value={password} onChange={e => setPassword(e.target.value)} placeholder="••••••••" required />
            </div>
            <div className="col-12 col-md-6">
              <label className="form-label fw-semibold text-secondary">Email Address</label>
              <input type="email" className="form-control" value={email} onChange={e => setEmail(e.target.value)} placeholder="doctor@example.com" />
            </div>
          </>
        ) : (
          <div className="col-12">
            <label className="form-label fw-semibold text-secondary">Doctor Name</label>
            <input type="text" className="form-control" value={fullName} onChange={e => setFullName(e.target.value)} required />
          </div>
        )}
        <div className="col-12 col-md-6">
          <label className="form-label fw-semibold text-secondary">Speciality</label>
          <input type="text" className="form-control" value={speciality} onChange={e => setSpeciality(e.target.value)} placeholder="e.g. Cardiology" required />
        </div>
        <div className="col-12 col-md-6">
          <label className="form-label fw-semibold text-secondary">Experience (Years)</label>
          <input type="number" className="form-control" value={experience} onChange={e => setExperience(e.target.value)} placeholder="5" required />
        </div>
        <div className="col-12 col-md-6">
          <label className="form-label fw-semibold text-secondary">Qualification</label>
          <input type="text" className="form-control" value={qualification} onChange={e => setQualification(e.target.value)} placeholder="e.g. MBBS, MD" required />
        </div>
        <div className="col-12 col-md-6">
          <label className="form-label fw-semibold text-secondary">Designation</label>
          <input type="text" className="form-control" value={designation} onChange={e => setDesignation(e.target.value)} placeholder="e.g. Consultant" required />
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
