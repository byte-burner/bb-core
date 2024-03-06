/* eslint-disable react/no-unescaped-entities */
/* eslint-disable max-len */
import React from 'react';
import {
  Box,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Typography,
  ListItem,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

export function HelpSettings() {
  return (
    <Box>
      <Accordion elevation={2}>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          id="deviceHelp"
        >
          <Typography>What is a device?</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <ListItem style={{ display: 'list-item' }}>
            A device refers to a microcontroller unit (MCU) or programmable device that can be programmed or flashed with firmware or software. These devices are the target hardware on which the compiled programs will be burned. Devices come in various types and models, each with its own specifications and capabilities. They are essential components in embedded systems, IoT (Internet of Things) devices, and various electronic applications, serving as the brains of the operation.
          </ListItem>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          id="bridgeHelp"
        >
          <Typography>What is a bridge?</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <ListItem style={{ display: 'list-item' }}>
            A bridge device acts as an intermediary between the computer running the flashing software and the programmable devices to be programmed. It establishes a communication link and facilitates data transfer between the computer and the target devices. The bridge device typically provides the necessary hardware interface and protocol conversion to communicate with different types of MCUs. It plays a crucial role in the programming process by ensuring compatibility and enabling reliable data transmission, ultimately simplifying the flashing procedure for the user.
          </ListItem>
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          id="mainProcessFlowHelp"
        >
          <Typography>How does the main process flow work?</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <ListItem style={{ display: 'list-item' }}>
            The main process flow of our application involves three key steps: device selection, file selection, and flashing. Firstly, the user selects the bridge device and the specific programmable devices to be programmed from the available options. This ensures that the flashing software is configured correctly for the target hardware. Next, the user selects a file containing the compiled program, ensuring compatibility with the chosen programmable devices. Finally, the user initiates the flashing process by clicking the "Burn" button. During flashing, the software communicates with the selected devices via the bridge device, transferring the program data and verifying the programming operation. Any error messages encountered during flashing are displayed to the user, providing feedback on the success or failure of the programming process. Upon successful completion, a confirmation message is shown, indicating that the device has been successfully programmed and is ready for use in its intended application.
          </ListItem>
        </AccordionDetails>
      </Accordion>
    </Box>
  );
}
