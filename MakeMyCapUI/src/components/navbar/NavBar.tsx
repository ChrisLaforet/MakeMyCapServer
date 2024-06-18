import React, { CSSProperties, useContext, useEffect, useRef, useState } from "react";
import { Navbar, Nav, Container, Dropdown } from 'react-bootstrap';
import { NavLink } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css'
import './NavBar.css';
import { useSharedContext } from '../../context/SharedContext';
import { UserChangeEndpoint } from '../../context/SharedContextData';

const NavBar = () => {

    const sharedContextData = useSharedContext();
    const onUserChangeCallback = useRef<UserChangeEndpoint | null>(null);
    const [userChanged, setUserChanged] = useState<boolean>(false);

    function isLoggedIn() {
        if (!sharedContextData) {
            console.log('Cannot find sharedContext at NavBar');
            return false;
        }

        const user = sharedContextData.getAuthenticatedUser();
        // at some point we want to show the user's info in the corner of the navbar
        return user != null;
    }


    function onDropdown() {
        console.log("Dropdown")
    }

    async function onUserChange(isLoggedIn: boolean): Promise<void> {
        console.log("USER CHANGE TO " + isLoggedIn)
        setUserChanged(isLoggedIn);
    }

    function getProfileButtonMoniker() {
        if (!sharedContextData) {
            console.log('Cannot find sharedContext at NavBar');
            return false;
        }

        const user = sharedContextData.getAuthenticatedUser();
        return user ? `Welcome ${user.userName}` : 'Profile';
    }

    useEffect(() => {
        const handler = new UserChangeEndpoint(onUserChange);
        onUserChangeCallback.current = handler;
        sharedContextData.registerUserChangeNotificationEndpoint(handler);

        return () => {
            if (onUserChangeCallback.current) {
                sharedContextData.deregisterUserChangeNotificationEndpoint(onUserChangeCallback.current)
                onUserChangeCallback.current = null;
            }
        }
    }, [])

    return (
        <Navbar bg="light" expand="lg" className="navbarBar">
            <Container className="navbarContainer">
                <Navbar.Brand className="navbar-brand"><span className="mmc-red">Make My Cap Tools</span></Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="navbar-menu">
                    <Nav className="">
                        <Nav.Link as={NavLink} to="/" className="navbarLink">Home</Nav.Link>
                        <Nav.Link as={NavLink} to="/Status" className="navbarLink">Status</Nav.Link>
                        <Nav.Link as={NavLink} to="/Orders" className="navbarLink">Orders</Nav.Link>
                        <Nav.Link as={NavLink} to="/SkuMapping" className="navbarLink">SKU mapping</Nav.Link>
                        <Nav.Link as={NavLink} to="/Inventory" className="navbarLink">Inventory</Nav.Link>

                        <Nav.Link as={NavLink} to="/Notifications" className="navbarLink">Notifications</Nav.Link>
                        <Nav.Link as={NavLink} to="/Settings" className="navbarLink">Settings</Nav.Link>
                        <Nav.Link as={NavLink} to="/Users" className="navbarLink">Users</Nav.Link>
                    </Nav>
                    <Nav className="navbar-force-right">
                        <Dropdown className="navbar-nav">
                            <Dropdown.Toggle id="dropdown-toggle" className="groupDropdown btn-light">
                                {
                                    getProfileButtonMoniker()
                                }
                            </Dropdown.Toggle>
                            <Dropdown.Menu onDrop={onDropdown} align="end" id="dropdown-menu">
                                { !isLoggedIn() &&
                                    <NavLink className="dropdown-item" to="/Login">
                                        <span>Log in</span>
                                    </NavLink>
                                }
                                { isLoggedIn() &&
                                    <>
                                        <NavLink className="dropdown-item" to="/EditSettings">
                                            <span>My Settings</span>
                                        </NavLink>
                                        <NavLink className="dropdown-item" to="/Logoff">
                                            <span className="mmc-blue">Log off</span>
                                        </NavLink>
                                    </>
                                }
                            </Dropdown.Menu>
                        </Dropdown>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

export default NavBar;
