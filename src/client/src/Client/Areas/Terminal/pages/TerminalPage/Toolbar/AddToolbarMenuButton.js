import React, { useState } from 'react';
import { PropTypes } from 'prop-types';
import {
  Typography,
  IconButton,
  Menu,
  MenuItem,
  Box,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

export function AddToolbarMenuButton({ onItemClick, items }) {
  const [anchorEl, setAnchorEl] = useState(null);
  const open = Boolean(anchorEl);

  const handleClick = (e) => {
    setAnchorEl(e.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleMenuItemClick = (type) => {
    onItemClick(type);
    handleClose();
  };

  return (
    <Box>
      <IconButton size="small" onClick={handleClick}>
        <AddIcon />
      </IconButton>
      <Menu
        id="terminalMenuId"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
      >
        {items.map((type) => (
          <MenuItem key={type} onClick={() => handleMenuItemClick(type)}>
            <Typography>{type}</Typography>
          </MenuItem>
        ))}
      </Menu>
    </Box>
  );
}

AddToolbarMenuButton.propTypes = {
  onItemClick: PropTypes.func.isRequired,
  items: PropTypes.arrayOf(PropTypes.string.isRequired).isRequired,
};
