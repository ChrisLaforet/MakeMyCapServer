import { Outlet } from "react-router-dom";
import NavBar from '../components/navbar/NavBar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCopyright, faEnvelope } from '@fortawesome/free-regular-svg-icons'
import { ToastContainer } from 'react-toastify';
import { useSharedContext } from '../context/SharedContext';
import React from 'react';


export default function RootLayout() {

    const sharedContextData = useSharedContext();

    return (
        <div className="root-layout">
            <header>
                <NavBar />
            </header>
            <main className="mmc-main-container">
                <ToastContainer />
                <Outlet />

                <footer className="d-flex flex-wrap col-12">
                    <span className="col-6">Copyright <FontAwesomeIcon icon={faCopyright}/> 2024 Make My Cap</span>
                    <span className="col-6 justify-content-end footer-right"><FontAwesomeIcon icon={faEnvelope}/> support@makemycap.com</span>
                </footer>
            </main>
        </div>
    )
}
