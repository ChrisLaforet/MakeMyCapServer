import { useNavigate } from 'react-router-dom';
import { useContext, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { LoginApi } from '../../api/LoginApi';
import { SharedContext, useSharedContext } from '../../context/SharedContext';


// @ts-ignore
export default function Logoff() {

    const sharedContextData = useSharedContext();

    const navigate = useNavigate();

    useEffect(() => {
        const logOut = async () => {
            console.log("Logging out")

            const authenticatedUser = sharedContextData.getAuthenticatedUser();
            sharedContextData.setAuthenticatedUser(null)
            if (authenticatedUser) {
                await LoginApi.logoutUser(authenticatedUser).then(() => sharedContextData.setAuthenticatedUser(null));
            }
        }

        logOut().catch(console.error);
        setTimeout(() => {
            navigate('/');
        }, 1000);
    }, []);

    return (
        <div>
            <form className="container login-form-container">
                <div className="row mmc-form-row col-12 top-heading">
                    <div>
                        <h1 id="ScreenTitle" className="display-page-title">
                            <span className="mmc-red">Logoff</span> <span className="mmc-blue">Completed</span>
                        </h1>
                    </div>
                </div>
            </form>
        </div>
    );
}
