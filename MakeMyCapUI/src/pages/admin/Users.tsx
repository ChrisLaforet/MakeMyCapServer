import { useSharedContext } from '../../context/SharedContext';
import React, { useEffect, useState } from 'react';
import { Modal } from 'react-responsive-modal';
import { UserApi } from '../../api/UserApi';
import { UserDto } from '../../api/dto/UserDto';
import { Alerter } from '../../layout/Alerter';
import 'react-responsive-modal/styles.css';
import '../Modal.css';


export default function Users() {
    const sharedContextData = useSharedContext();

    const [users, setUsers] = useState<UserDto[] | null>(() => null);
    const [usersChanged, setUsersChanged] = useState<number>(0);

    const [editUserOpen, setEditUserOpen] = useState<boolean>(false);
    const [passwordOpen, setPasswordOpen] = useState<boolean>(false);

    const [currentRecord, setCurrentRecord] = useState<UserDto | null>(null);
    const [title, setTitle] = useState<string>('');
    const [login, setLogin] = useState<string>('');
    const [code, setCode] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [name, setName] = useState<string>('');
    const [nextSequence, setNextSequence] = useState<number>(1);
    const [admin, setAdmin] = useState<boolean>(false);
    const [password, setPassword] = useState<string>('');
    const [artboardPath, setArtboardPath] = useState<string>('');
    const [artifactPath, setArtifactPath] = useState<string>('');
    const [outputPath, setOutputPath] = useState<string>('');

    function openEditUserModal(toEdit: UserDto | null) {
        setCurrentRecord(toEdit);
        setTitle("Artwork Generator: " + (toEdit ? "Edit User" : "Add New User"));
        setLogin(toEdit ? toEdit.login : '');
        setCode(toEdit ? toEdit.code : '');
        setEmail(toEdit ? toEdit.email : '');
        setName(toEdit ? toEdit.name : '');
        setNextSequence(toEdit ? toEdit.nextSequence : 1);
        setAdmin(toEdit? toEdit.admin : false);
        setArtboardPath(toEdit? toEdit.artboardPath : '~/Desktop');
        setArtifactPath(toEdit? toEdit.artifactPath : '~/Desktop');
        setOutputPath(toEdit? toEdit.outputPath : '~/Desktop');

        setEditUserOpen(true);
    }

    function updateSequence(value: string) {
        try {
            setNextSequence(parseInt(value, 10));
        } catch (e) {}
    }

    function toggleAdmin() {
        setAdmin(admin => !admin);
    }

    function closeEditUserModal() {
        setEditUserOpen(false);
    }

    function submitEditUser(e: any) {
        e.preventDefault();

        let isValid = true;
        if (!currentRecord && login.length == 0) {
            Alerter.showWarning("Login must be provided before saving a user");
            isValid = false;
        }
        if (code.length == 0) {
            Alerter.showWarning("Code/Initials must be provided before saving a user");
            isValid = false;
        }
        if (name.length == 0) {
            Alerter.showWarning("Name must be provided before saving a user");
            isValid = false;
        }
        if (email.length == 0) {
            Alerter.showWarning("Email must be provided before saving a user");
            isValid = false;
        }
        if (!isValid) {
            return;
        }

        const loginForDto = currentRecord ? currentRecord.login : login;
        const dto = new UserDto(loginForDto, code, name, email, nextSequence, admin, artboardPath, artifactPath, outputPath);

        Alerter.showInfo("Attempting to save user...", Alerter.DEFAULT_TIMEOUT)
        if (currentRecord) {
            updateUser(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Updating user was successful!", Alerter.DEFAULT_TIMEOUT)
                    setUsersChanged(usersChanged => usersChanged + 1);
                    closeEditUserModal();
                } else {
                    Alerter.showError("Updating user failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
                }
            });
        } else {
            saveNewUser(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Saving new user was successful!", Alerter.DEFAULT_TIMEOUT)
                    setUsersChanged(usersChanged => usersChanged + 1);
                    closeEditUserModal();
                } else {
                    Alerter.showError("Saving new user failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
                }
            });
        }
    }

    const saveNewUser = async (user: UserDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await UserApi.createNewUser(user, authenticatedUser);
        }
        console.log("Unable to find authenticated user to save new user!");
        return false;
    }

    const updateUser = async (user: UserDto): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await UserApi.updateUser(user, authenticatedUser);
        }
        console.log("Unable to find authenticated user to update user!");
        return false;
    }

    function openPasswordModal(user: UserDto) {
        setCurrentRecord(user);
        setTitle("Artwork Generator: Set password for " + user.login)
        setPasswordOpen(true);
    }

    function closePasswordModal() {
        setPasswordOpen(false);
    }

    function submitPassword(e: any) {
        e.preventDefault()
        if (!currentRecord || password.length < 8) {
            Alerter.showWarning("Password cannot be shorter than 8 characters for a user");
            return;
        }

        Alerter.showInfo("Attempting to set password for " + currentRecord.login + "...", Alerter.DEFAULT_TIMEOUT)
        if (currentRecord) {
            resetPassword(currentRecord.login, password).then(response => {
                if (response) {
                    Alerter.showSuccess("Password reset was successful!", Alerter.DEFAULT_TIMEOUT)
                    closePasswordModal();
                } else {
                    Alerter.showError("Updating user password failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
                }
            });
        }
    }

    const resetPassword = async (login: string, password: string): Promise<boolean> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await UserApi.setPassword(login, password, authenticatedUser);
        }
        console.log("Unable to find authenticated user to set user password!");
        return false;
    }

    const loadUsers = async () => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            const lookup = await UserApi.loadUsers(authenticatedUser);
            if (lookup) {
                setUsers(lookup)
            } else {
                setUsers([]);
            }
        }
    }

    useEffect(() => {
        loadUsers();
    }, [usersChanged]);

    return (
        <div>
            <h2><span className="ca-red">All</span> <span className="ca-blue">Users</span></h2>
            {!users &&
                <div className="ca-spinner-box spinner-border text-primary"><span className="sr-only"></span></div>}
            {users &&
                <div>
                    <div className="ca-operation-primary-button">
                        <button className="btn btn-outline-primary" onClick={() => openEditUserModal(null)}>Add new user</button>
                    </div>
                    <div className="ca_tabular-data-container">
                        <table className="table table-striped table-hover">
                            <thead>
                            <tr>
                                <th scope="col">Login</th>
                                <th scope="col">Code</th>
                                <th scope="col">Name</th>
                                <th scope="col">Email</th>
                                <th scope="col">Sequence #</th>
                                <th scope="col">Is admin?</th>
                                <th scope="col">Operations</th>
                            </tr>
                            </thead>
                            <tbody>
                            {(users).map(function (user, key) {
                                return (
                                    <tr key={user.login}>
                                        <th>{user.login}</th>
                                        <td>{user.code}</td>
                                        <td>{user.name}</td>
                                        <td>{user.email}</td>
                                        <td>{user.nextSequence}</td>
                                        <td>{user.admin ? "Y" : ""}</td>
                                        <td>
                                            <button className="btn btn-sm btn-outline-primary" onClick={() => openEditUserModal(user)}>Edit user</button>
                                            <button className="btn btn-sm btn-outline-primary ca-operation-buttons" onClick={() => openPasswordModal(user)}>
                                                Set password
                                            </button>
                                        </td>
                                    </tr>
                                );
                            })
                            }
                            </tbody>
                        </table>
                    </div>

                    <div>
                        <Modal open={editUserOpen}
                               onClose={closeEditUserModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'customUserModal',
                               }}>

                            <form onSubmit={submitEditUser}>
                                <div><h2>{title}</h2></div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="login" className="col-form-label">Login:</label>
                                        <input type="text" id="login" className="form-control" name="login"
                                               required={true}
                                               value={login}
                                               maxLength={30}
                                               readOnly={currentRecord != null}
                                               disabled={currentRecord != null}
                                               onChange={e => setLogin(e.target.value)}/>
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
                                        <label htmlFor="email" className="col-form-label">Email:</label>
                                        <input type="email" id="email" className="form-control" name="email"
                                               required={true}
                                               value={email}
                                               maxLength={100}
                                               onChange={e => setEmail(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="sequence" className="col-form-label">Next Sequence #:</label>
                                        <input type="number" id="sequence" className="form-control" name="sequence"
                                               required={true}
                                               value={nextSequence}
                                               onChange={e => updateSequence(e.target.value)}/>
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
                                <div className='row ca-form-row'>
                                    <div>
                                        <br />
                                        <input type="checkbox" id="admin" name="admin" value="Admin"
                                               checked={admin} onChange={() => toggleAdmin()}/>
                                        <label htmlFor="admin" className="col-form-checkbox-label">&nbsp;Is
                                            administrator?</label>
                                    </div>
                                </div>
                                <div className="row ca-form-button-row">
                                    <div className='modal-navigation-row justify-content-end'>
                                        <button className="btn btn-outline-primary" type="submit">Save</button>
                                        &nbsp;
                                        <button className="btn btn-outline-secondary"
                                                type="button"
                                                onClick={closeEditUserModal}>Cancel
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </Modal>
                    </div>

                    <div>
                        <Modal open={passwordOpen}
                               onClose={closePasswordModal}
                               closeOnEsc={false}
                               closeOnOverlayClick={false}
                               showCloseIcon={false}
                               classNames={{
                                   overlay: 'customOverlay',
                                   modal: 'customPasswordModal',
                               }}>

                            <form onSubmit={submitPassword}>
                                <div><h2>{title}</h2></div>
                                <div className='row ca-form-row'>
                                    <div>This permits you as administrator to reset a user's password immediately.</div>
                                </div>
                                <div className='row ca-form-row'>
                                    <div>
                                        <label htmlFor="password" className="col-form-label">New Password:</label>
                                        <input type="password" id="login" className="form-control" name="login"
                                               required={true}
                                               value={password}
                                               maxLength={30}
                                               onChange={e => setPassword(e.target.value)}/>
                                    </div>
                                </div>
                                <div className="row ca-form-button-row">
                                    <div className='modal-navigation-row justify-content-end'>
                                        <button className="btn btn-outline-primary" type="submit">Set password</button>
                                        &nbsp;
                                        <button className="btn btn-outline-secondary"
                                                type="button"
                                                onClick={closePasswordModal}>Cancel
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </Modal>
                    </div>
                </div>
            }
        </div>
    );
}
