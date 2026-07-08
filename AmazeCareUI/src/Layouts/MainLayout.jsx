import React from 'react';
import { Outlet } from 'react-router-dom';
import Footer from '../Components/Footer/Footer';
import HEADER from '../Components/Header/Header';



function MainLayout() {
  return (
   <div className="d-flex flex-column min-vh-100">
      <HEADER />
      
      <main className="flex-grow-1 pb-4">
        <Outlet />
      </main>

      <Footer />
    </div>
  );
};

export default MainLayout;