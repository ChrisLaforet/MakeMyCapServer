import React from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import { ToastContainer } from 'react-toastify';        // NOTICE!!! NOTICE!!! do not remove from here!
import 'react-toastify/dist/ReactToastify.css';
import App from './components/app/App';
import { SharedContextData } from './context/SharedContextData';

// https://reactrouter.com/en/main/start/overview

const container = document.getElementById('root')!
const root = createRoot(container)

root.render(<App />);
