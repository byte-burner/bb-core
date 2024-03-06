import React from 'react';
import { PropTypes } from 'prop-types';
import {
  Grid,
  Typography,
} from '@mui/material';

export function TerminalToolbar({ children, type }) {
  return (
    <Grid
      sx={{
        justifyContent: 'center',
        alignItems: 'center',
      }}
      container
    >
      <Grid
        item
        xs={8}
      >
        <Typography>{`Running terminal is: ${type}`}</Typography>
      </Grid>
      <Grid
        item
        xs={4}
        sx={{
          display: 'flex',
          justifyContent: 'flex-end',
        }}
      >
        {children}
      </Grid>
    </Grid>
  );
}

TerminalToolbar.propTypes = {
  children: PropTypes.node.isRequired,
  type: PropTypes.string.isRequired,
};
