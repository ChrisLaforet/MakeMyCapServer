import { Bounce, toast } from 'react-toastify';

// See: https://fkhadra.github.io/react-toastify/introduction/

export class Alerter {

    public static DEFAULT_TIMEOUT = 5000;

    public static showSuccess(message: string, closeTimeoutMSec = 0) {
        toast.success(message, {
            position: "bottom-right",
            autoClose: closeTimeoutMSec > 0 ? closeTimeoutMSec : false,
            hideProgressBar: false,
            theme: "light",
            transition: Bounce,
        });
    }

    public static showWarning(message: string, closeTimeoutMSec = 0) {
        toast.warn(message, {
            position: "bottom-right",
            autoClose: closeTimeoutMSec > 0 ? closeTimeoutMSec : false,
            hideProgressBar: false,
            theme: "light",
            transition: Bounce,
        });
    }

    public static showError(message: string, closeTimeoutMSec = 0) {
        toast.error(message, {
            position: "bottom-right",
            autoClose: closeTimeoutMSec > 0 ? closeTimeoutMSec : false,
            hideProgressBar: false,
            theme: "light",
            transition: Bounce,
        });
    }

    public static showInfo(message: string, closeTimeoutMSec = 0) {
        toast.info(message, {
            position: "bottom-right",
            autoClose: closeTimeoutMSec > 0 ? closeTimeoutMSec : false,
            hideProgressBar: false,
            theme: "light",
            transition: Bounce,
        });
    }
}
