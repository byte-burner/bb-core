import React from 'react';
import {
  HashRouter as Router,
} from 'react-router-dom';
import CssBaseline from '@mui/material/CssBaseline';
import { mapRoutes } from './Routes/AppRoutes';
import { AppProvider } from './Common/providers';
import './App.css';

export default function App() {
  return (
    <AppProvider>
      <CssBaseline />
      <Router>
        {mapRoutes()}
      </Router>
    </AppProvider>
  );
}
