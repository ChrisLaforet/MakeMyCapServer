import { Outlet } from "react-router-dom";
import NavBar from '../components/navbar/NavBar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCopyright, faEnvelope } from '@fortawesome/free-regular-svg-icons'
import { ToastContainer } from 'react-toastify';
import { useSharedContext } from '../context/SharedContext';


export default function RootLayout() {

    const sharedContextData = useSharedContext();

    return (
        <div className="root-layout">
            <header>
                <NavBar />
            </header>
            <main className="ca-main-container">
                <ToastContainer />
                <Outlet />

                <footer className="d-flex flex-wrap col-12">
                    <span className="col-6">Copyright <FontAwesomeIcon icon={faCopyright} /> 2024 CapAmerica</span>
                    <span className="col-6 justify-content-end footer-right"><FontAwesomeIcon icon={faEnvelope} /> art.host@capamerica.com</span>
                </footer>
            </main>
        </div>
    )
}
