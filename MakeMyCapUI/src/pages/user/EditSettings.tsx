import { useSharedContext } from '../../context/SharedContext';
import React, { useEffect, useState } from 'react';
import { Alerter } from '../../layout/Alerter';
import { UserDto } from '../../api/dto/UserDto';
import { UserApi } from '../../api/UserApi';
import { AuthenticatedUser } from '../../security/auth/AuthenticatedUser';

export default function EditSettings() {

    const sharedContextData = useSharedContext();

    const [code, setCode] = useState<string>('');
    const [name, setName] = useState<string>('');
    const [artboardPath, setArtboardPath] = useState<string>('');
    const [artifactPath, setArtifactPath] = useState<string>('');
    const [outputPath, setOutputPath] = useState<string>('');

    function submitEditSettings(e: any) {
        e.preventDefault();

        let isValid = true;
        if (code.length == 0) {
            Alerter.showWarning("Code/Initials must be provided before saving your settings");
            isValid = false;
        }
        if (name.length == 0) {
            Alerter.showWarning("Name must be provided before saving your settings");
            isValid = false;
        }
        if (!isValid) {
            return;
        }

        const user = sharedContextData.getAuthenticatedUser()!;
        const dto = new UserDto(user.userName, code, name, user.email, user.nextSequence, sharedContextData.isUserAnAdministrator(), artboardPath, artifactPath, outputPath);

        Alerter.showInfo("Attempting to save your updated settings...", Alerter.DEFAULT_TIMEOUT)
        updateUser(dto).then(response => {
            if (response) {
                Alerter.showSuccess("Updating your settings was successful!", Alerter.DEFAULT_TIMEOUT)
                const newUser = new AuthenticatedUser(user.token, user.userName, code, user.email, name, artboardPath, artifactPath, outputPath, user.nextSequence);
                sharedContextData.setAuthenticatedUser(newUser);
            } else {
                Alerter.showError("Updating your settings failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
            }
        });
    }

    const updateUser = async (user: UserDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await UserApi.updateUser(user, authenticatedUser);
        }
        console.log("Unable to find authenticated user to update user!");
        return false;
    }

    useEffect(() => {
        const user = sharedContextData.getAuthenticatedUser();
        if (user != null) {
            setCode(user.code);
            setName(user.name);
            setArtboardPath(user.artboardPath);
            setArtifactPath(user.artifactPath);
            setOutputPath(user.outputPath);
        }
    }, []);

    return (
        <div className='container'>
            <form className="container settings-form-container" onSubmit={submitEditSettings}>
                <div className="row da-form-row top-heading">
                    <div>
                        <h1 id="ScreenTitle" className="display-page-title">
                            <span className="ca-red">Edit</span> <span className="ca-blue">My Settings</span>
                        </h1>
                    </div>
                </div>
                <div className='row ca-form-row'>
                    <div>
                        <label htmlFor="code" className="col-form-label">Code/Initials:</label>
                        <input type="text" id="code" className="form-control" name="code"
                               required={true}
                               value={code}
                               maxLength={5}
                               onChange={e => setCode(e.target.value)}/>
                    </div>
                </div>
                <div className='row ca-form-row'>
                    <div>
                        <label htmlFor="name" className="col-form-label">Name:</label>
                        <input type="text" id="name" className="form-control" name="name"
                               required={true}
                               value={name}
                               maxLength={100}
                               onChange={e => setName(e.target.value)}/>
                    </div>
                </div>
                <div className='row ca-form-row'>
                    <div>
                        <label htmlFor="artworkpath" className="col-form-label">Default artwork path:</label>
                        <input type="artworkpath" id="artworkpath" className="form-control" name="artworkpath"
                               required={true}
                               value={artboardPath}
                               maxLength={100}
                               onChange={e => setArtboardPath(e.target.value)}/>
                    </div>
                </div>
                <div className='row ca-form-row'>
                    <div>
                        <label htmlFor="artifactpath" className="col-form-label">Default artifact path:</label>
                        <input type="artifactpath" id="artifactpath" className="form-control" name="artifactpath"
                               required={true}
                               value={artifactPath}
                               maxLength={100}
                               onChange={e => setArtifactPath(e.target.value)}/>
                    </div>
                </div>
                <div className='row ca-form-row'>
                    <div>
                        <label htmlFor="outputpath" className="col-form-label">Default output path:</label>
                        <input type="outputpath" id="outputpath" className="form-control" name="outputpath"
                               required={true}
                               value={outputPath}
                               maxLength={100}
                               onChange={e => setOutputPath(e.target.value)}/>
                    </div>
                </div>
                <div className="row ca-form-button-row">
                    <div className='modal-navigation-row justify-content-end'>
                        <button className="btn btn-outline-primary" type="submit">Save</button>
                    </div>
                </div>
            </form>
        </div>
    );
}
