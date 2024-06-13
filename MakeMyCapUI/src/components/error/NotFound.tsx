import React from "react";
import {Link} from "react-router-dom";
import './Error.css';

const NotFound = () => {
    return (
        <div className="not-found">
            <h2>Sorry</h2>
            <p>The requested page cannot be found.</p>
            <Link to="/">Back to the main page</Link>
        </div>
    );
}

export default NotFound;
