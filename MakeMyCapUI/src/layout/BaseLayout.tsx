import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCopyright, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import "./BaseLayout.css"
import { Outlet } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import React from 'react';


export default function BaseLayout() {

    return (
        <div className="base-layout">
            <div className="outlet-pane">
                <div className="icon-content">
                    <img src="MakeMyCapLogo.png" width="50" height="30" alt="Make My Cap"/>
                </div>
                <ToastContainer/>
                <Outlet/>
            </div>
            <footer className="d-flex flex-wrap col-12">
                <span className="col-6">Copyright <FontAwesomeIcon icon={faCopyright}/> 2024 Make My Cap</span>
                <span className="col-6 justify-content-end footer-right"><FontAwesomeIcon icon={faEnvelope}/> support@makemycap.com</span>
            </footer>
        </div>
    )
}
