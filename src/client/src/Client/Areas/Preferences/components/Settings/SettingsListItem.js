import React from 'react';
import { PropTypes } from 'prop-types';
import {
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from '@mui/material';

export function SettingsListItem({
  text,
  onClick,
  icon,
}) {
  return (
    <ListItem key={text} disablePadding>
      <ListItemButton
        onClick={onClick}
      >
        <ListItemIcon>
          {icon}
        </ListItemIcon>
        <ListItemText primary={text} />
      </ListItemButton>
    </ListItem>
  );
}

SettingsListItem.propTypes = {
  text: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  icon: PropTypes.element.isRequired,
};
