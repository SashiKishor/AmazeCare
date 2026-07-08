import React from 'react';
import './Footer.css';
import { useAuth } from '../../Context/AuthContext';
import { Link, useNavigate } from 'react-router-dom';

function Footer(){
  const { isAuthenticated, user } = useAuth();
  
  const navTextColor = '#ffffff'; 

  if (isAuthenticated && (user?.role === 'Admin' || user?.role === 'Doctor')) {
        return (
            <footer 
                className="py-3 text-center mt-auto w-100" 
                style={{ backgroundColor: '#94062ce8', color: '#ffffff' }}
            >
                <p className="mb-0 fw-bold">
                    © {new Date().getFullYear()} AmazeCare Hospital. All Rights Reserved.
                </p>
            </footer>
        );
    }
  
  return (
    <footer className="hospital-footer">
      <div className="footer-container">
        
        <div className="footer-column">
          <div className="footer-logo">
            AmazeCare Hospital
          </div>
          <p>Providing world-class healthcare with compassion and excellence.</p>
        </div>

        <div className="footer-column">
          <h3 className="footer-heading">Contact Information</h3>
          <p><strong> Address:</strong><br />
            123 Health Avenue<br />
            Medical District<br />
            Metropolis, NY 10001
          </p>
          <p><strong> Phone:</strong><br />
            +1 (555) 123-4567<br />
            Emergency: 911
          </p>
          <p><strong> Email:</strong><br />
            info@citycarehospital.com
          </p>
        </div>

        <div className="footer-column">
          <h3 className="footer-heading">Useful Links</h3>
          <ul className="footer-links">
            <Link className="nav-link text-decoration-none p-0 fs-6" to="/find-a-doctor" style={{ color: navTextColor }}>
              Find a Doctor
            </Link>
          </ul>
        </div>

      </div>

      <div className="footer-bottom">
        <div className="copyright">
          © {new Date().getFullYear()} CityCare Hospital. All Rights Reserved.
        </div>
        <div className="footer-bottom-links">
          <span>Privacy Policy</span>
          <span>|</span>
          <span>Terms of Service</span>
          <span>|</span>
          <span>Sitemap</span>
        </div>
      </div>
    </footer>
  );
};

export default Footer;