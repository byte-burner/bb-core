import React from 'react';
import { PropTypes } from 'prop-types';
import {
  Box,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  List,
  ListItemText,
  ListItem,
  ListItemIcon,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';

export function DeviceSettings({ supportedDevices }) {
  return (
    <Box>
      <Accordion elevation={2} defaultExpanded>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          id="supportedDevices"
        >
          Supported Devices
        </AccordionSummary>
        <AccordionDetails>
          <List>
            {supportedDevices.map((b) => (
              <ListItem key={b}>
                <ListItemIcon>
                  <CheckCircleIcon color="primary" />
                </ListItemIcon>
                <ListItemText primary={b} />
              </ListItem>
            ))}
          </List>
        </AccordionDetails>
      </Accordion>
    </Box>
  );
}

DeviceSettings.propTypes = {
  supportedDevices: PropTypes.arrayOf(PropTypes.string),
};

DeviceSettings.defaultProps = {
  supportedDevices: [],
};
