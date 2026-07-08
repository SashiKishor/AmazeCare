import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from "../../Context/AuthContext";

function HEADER() {
    const { isAuthenticated, user, logoutUser } = useAuth();
    const navigate = useNavigate();

    const [dropdownOpen, setDropdownOpen] = useState(false);

    const handleLogout = () => {
        setDropdownOpen(false);
        logoutUser();
        navigate('/');
    };

    
    const maroonColor = '#8b1031';
    const navTextColor = '#000000'; 

    
    const showPublicLinks = !isAuthenticated || user?.role === 'Patient';

    return (
        <header>
            <nav className="navbar navbar-expand bg-white border-bottom shadow-sm py-3">
                <div className="container-fluid">

                    
                    <Link
                        className="navbar-brand text-decoration-none fw-bold me-4 fs-4"
                        to="/"
                        style={{ color: maroonColor }}
                    >
                        AmazeCare
                    </Link>

                    <div className="d-flex w-100 justify-content-between align-items-center">

                        <ul className="navbar-nav flex-row gap-4 mb-0">

                            {showPublicLinks && (
                                <>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/" style={{ color: navTextColor }}>
                                            Home
                                        </Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/about" style={{ color: navTextColor }}>
                                            About Us
                                        </Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/find-a-doctor" style={{ color: navTextColor }}>
                                            Find a Doctor
                                        </Link>
                                    </li>
                                </>
                            )}

                            {isAuthenticated && user?.role === 'Patient' && (
                                <>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/patient/book-appointment" style={{ color: navTextColor }}>Book Appointment</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/patient/appointments" style={{ color: navTextColor }}>My Appointments</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/patient/medical-records" style={{ color: navTextColor }}>Medical Records</Link>
                                    </li>
                                </>
                            )}

                            {isAuthenticated && user?.role === 'Doctor' && (
                                <>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/doctor" style={{ color: navTextColor }}>Dashboard</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/doctor/appointments" style={{ color: navTextColor }}>Manage Appointments</Link>
                                    </li>
                                </>
                            )}

                            
                            {isAuthenticated && user?.role === 'Admin' && (
                                <>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/admin" style={{ color: navTextColor }}>Dashboard</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/admin/doctors" style={{ color: navTextColor }}>Manage Doctors</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/admin/patients" style={{ color: navTextColor }}>Manage Patients</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link text-decoration-none p-0 fs-6" to="/admin/appointments" style={{ color: navTextColor }}>Manage Appointments</Link>
                                    </li>
                                </>
                            )}
                        </ul>

                        
                        <div className="d-flex align-items-center ms-auto">
                            {isAuthenticated ? (
                                <div className="dropdown">
                                    <button
                                        className="btn dropdown-toggle d-flex align-items-center gap-2 border-0 p-0 fw-bold fs-6"
                                        type="button"
                                        style={{ color: maroonColor, backgroundColor: 'transparent' }}
                                        onClick={() => setDropdownOpen(!dropdownOpen)}
                                        onBlur={() => setTimeout(() => setDropdownOpen(false), 200)}
                                    >
                                        <span>Hi, {user?.fullName || user?.userName}</span>
                                    </button>

                                    <ul className={`dropdown-menu dropdown-menu-end shadow-sm ${dropdownOpen ? 'show' : ''} mt-2`}>
                                        {user?.role === 'Patient' && (
                                            <li>
                                                <Link 
                                                    className="dropdown-item d-flex align-items-center gap-2 fw-bold" 
                                                    to="/patient/personal-info"
                                                    style={{ color: maroonColor }}
                                                    onClick={() => setDropdownOpen(false)}
                                                >
                                                    <i className="bi bi-person-fill"></i> Personal Info
                                                </Link>
                                            </li>
                                        )}
                                        <li>
                                            <button
                                                className="dropdown-item d-flex align-items-center gap-2 fw-bold"
                                                type="button"
                                                onClick={handleLogout}
                                                style={{ color: maroonColor }}
                                            >
                                                <i className="bi bi-box-arrow-right"></i> Logout
                                            </button>
                                        </li>
                                    </ul>
                                </div>
                            ) : (
                                <div className="d-flex gap-4">
                                    <Link
                                        className="text-decoration-none fw-bold fs-6"
                                        to="/login"
                                        style={{ color: maroonColor }}
                                    >
                                        Login
                                    </Link>
                                    <Link
                                        className="text-decoration-none fw-bold fs-6"
                                        to="/register"
                                        style={{ color: maroonColor }}
                                    >
                                        Register
                                    </Link>
                                </div>
                            )}
                        </div>

                    </div>
                </div>
            </nav>
        </header>
    );
}

export default HEADER;