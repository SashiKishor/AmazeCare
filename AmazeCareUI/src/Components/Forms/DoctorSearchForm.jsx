import React, { useState, useEffect } from 'react';

function DoctorSearchForm({ onSearch, onReset }) {
  const [searchName, setSearchName] = useState('');
  const [searchSpeciality, setSearchSpeciality] = useState('');
  const [searchExperience, setSearchExperience] = useState('');

  useEffect(() => {
    const delaySearch = setTimeout(() => {
      onSearch({ searchName, searchSpeciality, searchExperience });
    }, 300);

    return () => clearTimeout(delaySearch);
  }, [searchName, searchSpeciality, searchExperience]);

  const handleReset = () => {
    setSearchName('');
    setSearchSpeciality('');
    setSearchExperience('');
    onReset();
  };

  return (
    <div className="row g-3 align-items-end text-start">
      
      <div className="col-md-4">
        <label className="form-label fw-bold text-secondary">Doctor Name</label>
        <input
          type="text"
          className="form-control p-2"
          placeholder="e.g. Dr. John"
          value={searchName}
          onChange={(e) => setSearchName(e.target.value)}
        />
      </div>
      
      <div className="col-md-4">
        <label className="form-label fw-bold text-secondary">Specialty</label>
        <select
          className="form-select p-2"
          value={searchSpeciality}
          onChange={(e) => setSearchSpeciality(e.target.value)}
        >
          <option value="">All Specialities</option>
          <option value="Cardiology">Cardiology</option>
          <option value="Dermatology">Dermatology</option>
          <option value="Neurology">Neurology</option>
          <option value="Pediatrics">Pediatrics</option>
          <option value="General Medicine">General Medicine</option>
        </select>
      </div>
      
      <div className="col-md-2">
        <label className="form-label fw-bold text-secondary">Min Experience</label>
        <input
          type="number"
          min="0"
          className="form-control p-2"
          placeholder="Years (e.g. 5)"
          value={searchExperience}
          onChange={(e) => {
            const val = e.target.value;
            if (val === '' || parseFloat(val) >= 0) {
              setSearchExperience(val);
            }
          }}
        />
      </div>
      
      {/* Reset Button */}
      <div className="col-md-2">
        <button 
          type="button" 
          onClick={handleReset} 
          className="btn btn-outline-secondary w-100 p-2 fw-bold"
        >
          Reset Filters
        </button>
      </div>
      
    </div>
  );
}

export default DoctorSearchForm;