import { useState } from 'react';
import './Login.css';
import { LoginApi } from '../../api/LoginApi';
import { Link, useNavigate } from 'react-router-dom';
import { useSharedContext } from '../../context/SharedContext';
import { Alerter } from '../../layout/Alerter';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';

// @ts-ignore
export default function Login() {

    const [username, setUserName] = useState<string>();
    const [password, setPassword] = useState<string>();

    const sharedContextData = useSharedContext();

    const navigate = useNavigate();

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        if (!username || !password) {
            sharedContextData.setAuthenticatedUser(null);
        } else {
            const value = await LoginApi.loginUser(username, password);
            if (value instanceof AuthenticatedUser) {
                sharedContextData.setAuthenticatedUser(value);
                navigate('/');
            } else {
                Alerter.showError("Login attempt failed.  Please check your credentials and attempt to log in again.", Alerter.DEFAULT_TIMEOUT)
                sharedContextData.setAuthenticatedUser(null);
            }
        }
    }

    return (
        <div className='container'>
            <form className="container login-form-container" onSubmit={handleSubmit}>
                <div className="row mmc-form-row top-heading">
                    <div>
                        <h1 id="ScreenTitle" className="display-page-title">
                            <span className="mmc-red">Log In To Your Account</span>
                        </h1>
                    </div>
                </div>

                <div className='row mmc-form-row'>
                    <div>
                        <label htmlFor="loginName" className="col-form-label">Username</label>
                        <input id="loginName" className="form-control" type="text" maxLength={100} required
                               onChange={e => setUserName(e.target.value)}/>
                    </div>
                </div>
                <div className='row mmc-form-row'>
                    <div>
                        <label htmlFor="password" className="col-form-label">Password</label>
                        <input id="password" className="form-control" type="password" maxLength={50} required
                               onChange={e => setPassword(e.target.value)}/>
                    </div>
                </div>
                <div className='row mmc-form-row mmc-form-button-row'>
                    <div>
                        <button type="submit" className="btn btn-primary mmc-form-wide-button">Log me in!</button>
                    </div>
                </div>

                <div className='row mmc-form-row mmc-form-options-row'>
                    <div>
                        <div className="mmc-form-options-label">Did you forget your password? <Link
                            to="/RequestPasswordChange">Request a password change</Link></div>
                        <div className="mmc-form-options-label">Need to complete a password reset? <Link
                            to="/ChangePassword">Complete password change</Link></div>

                    </div>
                </div>
            </form>
        </div>
    )
}

