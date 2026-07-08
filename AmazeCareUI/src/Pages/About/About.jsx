import React from 'react';
import './About.css';
function About() {
    return (
        <div className="about-page">
            <section className="about-hero">
                <h1>About AmazeCare</h1>
                <p>Dedicated to healing, committed to care.</p>
            </section>
            <section className="about-mission">
                <div className="mission-text">
                    <h2>Our Mission & Vision</h2>
                    <div className="heading-underline"></div>
                    <p>Founded with a simple but powerful goal, AmazeCare has grown from a small community clinic into a premier healthcare institution. Our mission is to provide accessible, world-class medical care with an unwavering commitment to patient dignity and compassion.</p>
                    <p>We envision a future where advanced medical technology and human empathy seamlessly combine to create a healthier society for everyone. We believe that every patient deserves not just treatment, but genuine care and understanding.</p>
                </div>
                <div className="mission-image">
                    <img src="https://images.unsplash.com/photo-1586773860418-d37222d8fce3?q=80&w=800&auto=format&fit=crop" alt="AmazeCare Facility" />
                </div>
            </section>
            <section className="about-values">
                <div className="values-header">
                    <h2>Our Core Values</h2>
                    <div className="heading-underline"></div>
                </div>
                <div className="values-grid">
                    <div className="value-card">
                        <div className="value-icon">❤️</div>
                        <h3>Compassion</h3>
                        <p>We treat every patient like family, ensuring they feel heard, respected, and cared for during their most vulnerable moments.</p>
                    </div>
                    <div className="value-card">
                        <div className="value-icon">⭐</div>
                        <h3>Excellence</h3>
                        <p>We strive for the highest standards in clinical outcomes, patient safety, and continuous medical innovation.</p>
                    </div>
                    <div className="value-card">
                        <div className="value-icon">🤝</div>
                        <h3>Integrity</h3>
                        <p>We conduct our medical practice and business operations with absolute transparency, honesty, and ethical responsibility.</p>
                    </div>
                </div>
            </section>
            <section className="about-stats">
                <div className="stat-box">
                    <h3>15+</h3>
                    <p>Years of Excellence</p>
                </div>
                <div className="stat-box">
                    <h3>50+</h3>
                    <p>Specialist Doctors</p>
                </div>
                <div className="stat-box">
                    <h3>10k+</h3>
                    <p>Happy Patients</p>
                </div>
                <div className="stat-box">
                    <h3>24/7</h3>
                    <p>Emergency Care</p>
                </div>
            </section>
        </div>
    );
}
export default About;