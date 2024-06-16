import { Link } from 'react-router-dom';

export default function PasswordChangeInstructions() {
    return (
        <div className='container'>
            <form className="container login-form-container">
                <div className="row da-form-row top-heading">
                    <div>
                        <h1 id="ScreenTitle" className="display-page-title">Password Change Requested</h1>
                    </div>
                </div>

                <div className='row mmc-form-row'>
                    <div>
                        This places a request to change your password. If your login username or Email exists, you will
                        receive an Email prompting you to follow a link to set your password.
                    </div>
                </div>

                <div className='row mmc-form-row mmc-form-options-row'>
                    <div>
                        <div className="mmc-form-options-label">Return to login? <Link to="/Login">Login here</Link>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    );
}
