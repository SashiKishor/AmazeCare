import React from 'react';
import { Route, Routes } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';


import MainLayout from './Layouts/MainLayout';
import ProtectedLayout from './Layouts/ProtectedLayout';


import Home from './Pages/Home/Home';
import About from './Pages/About/About';
import FindADoctor from './Pages/FindADoctor';
import Login from './Pages/Login';
import Register from './Pages/Register';
import BookAppointment from './Pages/Patients/BookAppointment';
import PatientAppointments from './Pages/Patients/PatientAppointments';
import PatientMedicalRecords from './Pages/Patients/PatientMedicalRecord';
import DoctorAppointments from './Pages/Doctors/DoctorAppointments';
import DoctorDashboard from './Pages/Doctors/DoctorDashboard';
import AdminDashboard from './Pages/Admin/AdminDashboard';
import ManageDoctors from './Pages/Admin/ManageDoctors';
import ManagePatients from './Pages/Admin/ManagePatients';
import ManageAppointments from './Pages/Admin/ManageAppointments';
import PersonalInfo from './Pages/PersonalInfo';
import NotFound from './Pages/NotFound';

function App() {
  return (
    <>
      <Routes>
        
        <Route path="/" element={<MainLayout />}>
          <Route index element={<Home />} />
          <Route path="about" element={<About />} />
          <Route path="find-a-doctor" element={<FindADoctor />} />
        </Route>

        
        <Route element={<ProtectedLayout allowedRoles={['Patient']} />}>
          <Route element={<MainLayout />}>
            <Route path="/patient/book-appointment" element={<BookAppointment />} />
            <Route path="/patient/appointments" element={<PatientAppointments />} />
            <Route path="/patient/medical-records" element={<PatientMedicalRecords />} />
            <Route path="/patient/personal-info" element={<PersonalInfo />} />
            <Route path="/patient/personal_info" element={<PersonalInfo />} />
          </Route>
        </Route>

        <Route element={<ProtectedLayout allowedRoles={['Doctor']} />}>
          <Route element={<MainLayout />}>
            <Route path="/doctor" element={<DoctorDashboard />} />
            <Route path="/doctor/appointments" element={<DoctorAppointments />} />
          </Route>
        </Route>

        <Route element={<ProtectedLayout allowedRoles={['Admin']} />}>
          <Route element={<MainLayout />}>
            <Route path="/admin" element={<AdminDashboard />} />
            <Route path="/admin/doctors" element={<ManageDoctors />} />
            <Route path="/admin/patients" element={<ManagePatients />} />
            <Route path="/admin/appointments" element={<ManageAppointments />} />
          </Route>
        </Route>

        <Route path="login" element={<Login />} />
        <Route path="register" element={<Register />} />

        <Route path="*" element={<NotFound />} />
      </Routes>
    </>
  );
}

export default App;