import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from "../../Context/AuthContext";
import './Home.css';

function Home() {
    const { isAuthenticated, user } = useAuth();


    if (isAuthenticated && (user?.role === 'Admin' || user?.role === 'Doctor')) {

        return (
            <div
                className="d-flex flex-column justify-content-center align-items-center"
                style={{ minHeight: '80vh', color: '#8b1031' }}
            >
                <h1 className="fw-bold display-4">Welcome, {user?.fullName || user?.userName}</h1>
                <p className="text-muted fs-5">AmazeCare {user?.role} Portal</p>
            </div>
        );
    }

    return (
        <div className="home-page">

            <section className="hero-section">
                <div className="hero-content">
                    <h1>Your Health, Our Priority</h1>
                    <p>Providing world-class healthcare with compassion and excellence. Book an appointment today and experience the AmazeCare difference.</p>
                    <div className="hero-buttons">
                        <Link to="/find-a-doctor" className="btn btn-primary">Find a Doctor</Link>
                        <Link to="/about" className="btn btn-secondary">Learn More</Link>
                    </div>
                </div>
            </section>
            <section className="py-5 bg-white">
                <div className="container">
                    <div className="row align-items-center g-5">
                        <div className="col-lg-6 text-start">
                            <h2 className="fw-bold mb-3" style={{ color: '#8b1031', fontSize: '2.5rem' }}>About AmazeCare</h2>
                            <div className="mb-4" style={{ height: '3px', width: '60px', backgroundColor: '#8b1031' }}></div>
                            <p className="text-muted fs-5 mb-4">
                                AmazeCare is a state-of-the-art digital healthcare system designed to deliver world-class medical services with maximum ease and compassion. We connect patients with expert specialists and ensure seamless clinical management.
                            </p>
                            <p className="text-muted mb-4">
                                Our platform enables hassle-free doctor consultations, streamlined appointment bookings, and digitized medical records. By bringing cutting-edge healthcare management directly to your screen, we ensure that your well-being always remains our priority.
                            </p>
                        </div>
                        <div className="col-lg-6">
                            <div className="row g-4 text-start">
                                <div className="col-sm-6">
                                    <div className="p-4 bg-light rounded-4 shadow-sm" style={{ borderLeft: '4px solid #8b1031' }}>
                                        <h3 className="fw-bold" style={{ color: '#8b1031' }}>50+</h3>
                                        <p className="text-secondary fw-semibold mb-0">Specialist Doctors</p>
                                    </div>
                                </div>
                                <div className="col-sm-6">
                                    <div className="p-4 bg-light rounded-4 shadow-sm" style={{ borderLeft: '4px solid #8b1031' }}>
                                        <h3 className="fw-bold" style={{ color: '#8b1031' }}>24/7</h3>
                                        <p className="text-secondary fw-semibold mb-0">Emergency Support</p>
                                    </div>
                                </div>
                                <div className="col-sm-6">
                                    <div className="p-4 bg-light rounded-4 shadow-sm" style={{ borderLeft: '4px solid #8b1031' }}>
                                        <h3 className="fw-bold" style={{ color: '#8b1031' }}>10k+</h3>
                                        <p className="text-secondary fw-semibold mb-0">Happy Patients</p>
                                    </div>
                                </div>
                                <div className="col-sm-6">
                                    <div className="p-4 bg-light rounded-4 shadow-sm" style={{ borderLeft: '4px solid #8b1031' }}>
                                        <h3 className="fw-bold" style={{ color: '#8b1031' }}>100%</h3>
                                        <p className="text-secondary fw-semibold mb-0">Digitized Records</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>


            <section className="cta-section">
                <h2>Need Immediate Assistance?</h2>
                <p>Our emergency services are available 24/7. Don't hesitate to reach out.</p>
                <button className="btn btn-urgent">Call Emergency: 911</button>
            </section>

        </div>
    );
}

export default Home;