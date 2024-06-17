import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Alerter } from '../../layout/Alerter';
import { FieldValidator } from '../../utils/validate/FieldValidator';
import { PasswordApi } from '../../api/PasswordApi';


// @ts-ignore
export default function ChangePassword() {

    const [username, setUsername] = useState<string>();
    const [confirmationCode, setConfirmationCode] = useState<string>();
    const [password, setPassword] = useState<string>();
    const [confirmPassword, setConfirmPassword] = useState<string>();

    const navigate = useNavigate();

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        if (!username || !confirmationCode) {
            return;
        }

        if (!FieldValidator.ValidatePassword(password)) {
            Alerter.showInfo("Password must be at least 8 characters long", Alerter.DEFAULT_TIMEOUT);
            return;
        }

        if (password != confirmPassword) {
            Alerter.showError("The confirmation password does not match the password", Alerter.DEFAULT_TIMEOUT);
            return;
        }

        const sendRequest = async () => {
            console.log("Change password request")
            await PasswordApi.requestPasswordChange(username, confirmationCode, password!);
        }

        sendRequest().catch(console.error);

        navigate('/');
    }

    return (
        <div>
            <form className="container login-form-container" onSubmit={handleSubmit}>
                <div className="row mmc-form-row top-heading">
                    <div>
                        <h1 id="ScreenTitle" className="display-page-title"><span className="mmc-blue">Change Your Password</span></h1>
                    </div>
                </div>

                <div className='row mmc-form-row'>
                    <div>
                        <p className="mmc-form-options-label">Please enter your login and the confirmation code from the
                            email sent to you to reset your password.</p>
                    </div>
                </div>
                <div className='row mmc-form-row'>
                    <div>
                        <label htmlFor="loginName" className="col-form-label">Username (not Email)</label>
                        <input id="loginName" className="form-control" type="text" maxLength={20} required
                               onChange={e => setUsername(e.target.value)}/>
                    </div>
                </div>
                <div className='row mmc-form-row'>
                    <div>
                        <label htmlFor="confirmationCode" className="col-form-label">Emailed Confirmation Code</label>
                        <input id="confirmationCode" className="form-control" type="text" maxLength={100} required
                               onChange={e => setConfirmationCode(e.target.value)}/>
                    </div>
                </div>
                <div className='row mmc-form-row'>
                    <div>
                        <label htmlFor="password" className="col-form-label">Password</label>
                        <input id="password" className="form-control" type="password" maxLength={50} required
                               onChange={e => setPassword(e.target.value)}/>

                        <span className="mmc-form-field-instructions">Must be at least 8 characters</span>
                    </div>
                </div>
                <div className='row mmc-form-row'>
                    <div>
                        <label htmlFor="confirm" className="col-form-label">Confirm Password</label>
                        <input id="confirm" className="form-control" type="password" maxLength={50} required
                               onChange={e => setConfirmPassword(e.target.value)}/>
                    </div>
                </div>
                <div className='row mmc-form-row mmc-form-button-row'>
                    <div>
                        <button type="submit" className="btn btn-primary mmc-form-wide-button">Change password</button>
                    </div>
                </div>

                <div className='row mmc-form-row mmc-form-options-row'>
                    <div>
                        <div className="mmc-form-options-label">Return to the login page? <Link to="/Login">Login
                            here</Link></div>
                    </div>
                </div>
            </form>
        </div>
    )
}

