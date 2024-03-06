import React from 'react';
import { PropTypes } from 'prop-types';
import {
  IconButton,
  Box,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';

export function RemoveToolbarButton({ onClick }) {
  return (
    <Box>
      <IconButton size="small" onClick={onClick}>
        <DeleteIcon />
      </IconButton>
    </Box>
  );
}

RemoveToolbarButton.propTypes = {
  onClick: PropTypes.func.isRequired,
};
