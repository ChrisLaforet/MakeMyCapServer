import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Alerter } from '../../layout/Alerter';
import { PasswordApi } from '../../api/PasswordApi';

export default function RequestPasswordChange() {

    const [username, setUserName] = useState<string | undefined>();

    const navigate = useNavigate();

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        if (username && username.length > 0) {
            if (await PasswordApi.requestPasswordReset(username)) {
                navigate('/PasswordChangeInstructions');
            } else {
                Alerter.showError("Password change request failed.  Try it again.", Alerter.DEFAULT_TIMEOUT)
            }
        }
    }

    return (
        <div className='container'>
            <form className="container settings-form-container" onSubmit={handleSubmit}>
                <div className="row da-form-row top-heading">
                    <div>
                        <h1 id="ScreenTitle" className="display-page-title">
                            <span className="mmc-blue">Request a Password Change</span>
                        </h1>
                    </div>
                </div>

                <div className='row mmc-form-row'>
                    <div>
                        <p className="mmc-form-options-label">Please enter your account username or email address in the
                            field below to request a password reset. An email will be sent to you with instructions to
                            reset your password.</p>
                    </div>
                </div>

                <div className='row mmc-form-row'>
                    <div>
                        <label htmlFor="name" className="col-form-label">Username or Email</label>
                        <input id="name" className="form-control" type="text" maxLength={100} required
                               onChange={e => setUserName(e.target.value)}/>
                    </div>
                </div>
                <div className='row mmc-form-row mmc-form-button-row'>
                    <div>
                        <button type="submit" className="btn btn-primary mmc-form-wide-button">Request password change
                        </button>
                    </div>
                </div>

                <div className='row mmc-form-row mmc-form-options-row'>
                    <div>
                        <div className="mmc-form-options-label">Return to login? <Link to="/Login">Login here</Link></div>
                        <div className="mmc-form-options-label">Return to home page now? <Link to="/">Take me home</Link></div>
                    </div>
                </div>
            </form>
        </div>
    );
}
