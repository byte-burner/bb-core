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
  Button,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';

export function BridgeSettings({
  supportedBridges,
  connectedBridges,
  loadConnectedBridges,
}) {
  return (
    <Box>
      <Accordion elevation={2} defaultExpanded>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          id="supportedBridges"
        >
          Supported Bridges
        </AccordionSummary>
        <AccordionDetails>
          <List>
            {supportedBridges.map((b) => (
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
      <Accordion elevation={2}>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          id="connectedBridges"
        >
          Connected Bridges
        </AccordionSummary>
        <AccordionDetails>
          <Button variant="contained" onClick={loadConnectedBridges}>Get Connected Bridges</Button>
          <List>
            {connectedBridges.map((b) => (
              <ListItem key={b}>
                <ListItemIcon>
                  <CheckCircleIcon color="success" />
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

BridgeSettings.propTypes = {
  supportedBridges: PropTypes.arrayOf(PropTypes.string),
  connectedBridges: PropTypes.arrayOf(PropTypes.string),
  loadConnectedBridges: PropTypes.func.isRequired,
};

BridgeSettings.defaultProps = {
  supportedBridges: [],
  connectedBridges: [],
};
