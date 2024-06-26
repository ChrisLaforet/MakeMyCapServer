import { useSharedContext } from '../../context/SharedContext';
import React, { useEffect, useState } from 'react';
import { Modal } from 'react-responsive-modal';
import { AdminApi } from '../../api/AdminApi';
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
    const [userName, setUserName] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    function openEditUserModal(toEdit: UserDto | null) {
        setCurrentRecord(toEdit);
        setTitle("Make My Cap: " + (toEdit ? "Edit User" : "Add New User"));
        setUserName(toEdit ? toEdit.userName : '');
        setEmail(toEdit ? toEdit.email : '');

        setEditUserOpen(true);
    }

    function closeEditUserModal() {
        setEditUserOpen(false);
    }

    function submitEditUser(e: any) {
        e.preventDefault();

        let isValid = true;
        if (!currentRecord && userName.length == 0) {
            Alerter.showWarning("User name must be provided before saving a user");
            isValid = false;
        }
        if (email.length == 0) {
            Alerter.showWarning("Email must be provided before saving a user");
            isValid = false;
        }
        if (!isValid) {
            return;
        }

        const loginForDto = currentRecord ? currentRecord.userName : userName;
        const dto = new UserDto(loginForDto, email, "");

        Alerter.showInfo("Attempting to save user...", Alerter.DEFAULT_TIMEOUT);
        if (currentRecord) {
            updateUser(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Updating user was successful!", Alerter.DEFAULT_TIMEOUT);
                    setUsersChanged(usersChanged => usersChanged + 1);
                    closeEditUserModal();
                } else {
                    Alerter.showError("Updating user failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT);
                }
            });
        } else {
            saveNewUser(dto).then(response => {
                if (response) {
                    Alerter.showSuccess("Saving new user was successful!", Alerter.DEFAULT_TIMEOUT);
                    users!.push(response);
                    setUsersChanged(usersChanged => usersChanged + 1);
                    closeEditUserModal();
                } else {
                    Alerter.showError("Saving new user failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT);
                }
            });
        }
    }

    const saveNewUser = async (user: UserDto): Promise<UserDto | null> => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            return await AdminApi.createNewUser(user, authenticatedUser);
        }
        console.log("Unable to find authenticated user to save new user!");
        return null;
    }

    const updateUser = async (user: UserDto): Promise<boolean> => {
        // TODO: CML - later possible implementation
        // const authenticatedUser = sharedContextData.getAuthenticatedUser();
        // if (authenticatedUser) {
        //     return await AdminApi.updateUser(user, authenticatedUser);
        // }
        // console.log("Unable to find authenticated user to update user!");
        return false;
    }

    function openPasswordModal(user: UserDto) {
        setCurrentRecord(user);
        setTitle("Artwork Generator: Set password for " + user.userName)
        setPasswordOpen(true);
    }

    function closePasswordModal() {
        setPasswordOpen(false);
    }

    function submitPassword(e: any) {
        // TODO: CML - later possible implementation
        e.preventDefault()
        // if (!currentRecord || password.length < 8) {
        //     Alerter.showWarning("Password cannot be shorter than 8 characters for a user");
        //     return;
        // }
        //
        // Alerter.showInfo("Attempting to set password for " + currentRecord.login + "...", Alerter.DEFAULT_TIMEOUT)
        // if (currentRecord) {
        //     resetPassword(currentRecord.login, password).then(response => {
        //         if (response) {
        //             Alerter.showSuccess("Password reset was successful!", Alerter.DEFAULT_TIMEOUT)
        //             closePasswordModal();
        //         } else {
        //             Alerter.showError("Updating user password failed.  Try to save again.", Alerter.DEFAULT_TIMEOUT)
        //         }
        //     });
        // }
    }

    const resetPassword = async (login: string, password: string): Promise<boolean> => {
        // TODO: CML - later possible implementation
        // const authenticatedUser = sharedContextData.getAuthenticatedUser();
        // if (authenticatedUser) {
        //     return await AdminApi.setPassword(login, password, authenticatedUser);
        // }
        // console.log("Unable to find authenticated user to set user password!");
        return false;
    }

    const loadUsers = async () => {
        const authenticatedUser = sharedContextData.getAuthenticatedUser();
        if (authenticatedUser) {
            const lookup = await AdminApi.loadUsers(authenticatedUser);
            if (lookup) {
                setUsers(lookup)
            } else {
                setUsers([]);
            }
        }
    }

    useEffect(() => {
        loadUsers().catch((error) => console.log(error));
    }, [usersChanged]);

    return (
        <div>
            <h2><span className="mmc-red">All Users</span></h2>
            {!users &&
                <div className="mmc-spinner-box spinner-border text-primary"><span className="sr-only"></span></div>}
            {users &&
                <div>
                    <div className="mmc-operation-primary-button">
                        <button className="btn btn-outline-primary" onClick={() => openEditUserModal(null)}>Add new user</button>
                    </div>
                    <div className="mmc-tabular-data-container">
                        <table className="table table-striped table-hover">
                            <thead>
                            <tr>
                                <th scope="col">User Name</th>
                                <th scope="col">Email</th>
                                <th scope="col">Create Date?</th>
                                <th scope="col">Operations</th>
                            </tr>
                            </thead>
                            <tbody>
                            {(users).map(function (user, key) {
                                return (
                                    <tr key={user.userName}>
                                        <th>{user.userName}</th>
                                        <td>{user.email}</td>
                                        <td>{user.createDate.slice(0,10)}</td>
                                        <td>
                                            <button className="btn btn-sm btn-outline-primary" onClick={() => openEditUserModal(user)}>Edit user</button>
                                            <button className="btn btn-sm btn-outline-primary mmc-operation-buttons" onClick={() => openPasswordModal(user)}>
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
                                <div className='row mmc-form-row'>
                                    <div>
                                        <label htmlFor="userName" className="col-form-label">Login:</label>
                                        <input type="text" id="userName" className="form-control"
                                               required={true}
                                               value={userName}
                                               maxLength={20}
                                               readOnly={currentRecord != null}
                                               disabled={currentRecord != null}
                                               onChange={e => setUserName(e.target.value)}/>
                                    </div>
                                </div>
                                <div className='row mmc-form-row'>
                                    <div>
                                        <label htmlFor="email" className="col-form-label">Email:</label>
                                        <input type="email" id="email" className="form-control" name="email"
                                               required={true}
                                               value={email}
                                               maxLength={100}
                                               onChange={e => setEmail(e.target.value)}/>
                                    </div>
                                </div>
                                <div className="row mmc-form-button-row">
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
                                <div className='row mmc-form-row'>
                                    <div>This permits you as administrator to reset a user's password immediately.</div>
                                </div>
                                <div className='row mmc-form-row'>
                                    <div>
                                        <label htmlFor="password" className="col-form-label">New Password:</label>
                                        <input type="password" id="login" className="form-control" name="login"
                                               required={true}
                                               value={password}
                                               maxLength={30}
                                               onChange={e => setPassword(e.target.value)}/>
                                    </div>
                                </div>
                                <div className="row mmc-form-button-row">
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
