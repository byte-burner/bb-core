import React, {
  createContext,
  useContext,
  useState,
  useMemo,
} from 'react';
import PropTypes from 'prop-types';
import { Snackbar, Alert } from '@mui/material';

/**
 * NOTIFICATION CONTEXT
 */
const NotificationContext = createContext();

export const useNotificationContext = () => (useContext(NotificationContext));

/**
 * NOTIFICATION PROVIDER
 */
export function NotificationProvider({ children }) {
  /** DATA */
  const [open, setOpen] = useState(false);
  const [errors, setErrors] = useState([]);
  const [severity, setSeverity] = useState('info');

  /** PUBLIC FUNCTIONS */
  const showNotifications = (newErrors, newSeverity = 'info') => {
    setErrors(newErrors);
    setSeverity(newSeverity);
    setOpen(true);
  };

  /** PRIVATE FUNCTIONS */
  const hideAlert = () => {
    setOpen(false);
  };

  /** EXPOSE CONTEXT STATE */
  const providerValue = useMemo(
    () => ({
      // add more custom functionality here
      showNotifications,
    }),
    [
      // add more custom functionality here
      showNotifications,
    ],
  );

  return (
    <NotificationContext.Provider value={providerValue}>
      {children}
      {errors.map((err) => (
        <Snackbar
          key={err}
          open={open}
          autoHideDuration={7000}
          onClose={hideAlert}
        >
          <Alert
            severity={severity}
          >
            {`${err?.code}: ${err?.message}`}
          </Alert>
        </Snackbar>
      ))}
    </NotificationContext.Provider>
  );
}

NotificationProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

NotificationProvider.displayName = 'NoficationProvider';
