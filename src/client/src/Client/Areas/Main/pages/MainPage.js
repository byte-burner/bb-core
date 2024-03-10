import React, { useEffect, useState } from 'react';
import {
  Grid,
  Button,
  CircularProgress,
  Stack,
  Box,
} from '@mui/material';
// import { styled } from '@mui/material/styles';
import LocalFireDepartmentIcon from '@mui/icons-material/LocalFireDepartment';
import AddBoxIcon from '@mui/icons-material/AddBox';
import MemoryIcon from '@mui/icons-material/Memory';
import { Dropdown } from '../../../Common/components';
import { useDeviceProgrammiingApi } from '../../../Api/RestApi';
import { useMonitorDeviceInfoContext } from '../../../Common/providers';
import { ProgrammingInfo } from '../../../Api/models';

// eslint-disable-next-line react/prop-types
function Line({ sx }) {
  return (
    <Box
      sx={{
        marginTop: 2,
        bottom: 0,
        right: 0,
        height: '1px',
        backgroundColor: ({ palette }) => palette.text.primary,
        ...sx,
      }}
    />
  );
}

const buttonStyle = {
  textTransform: 'none', // Set text transformation to none for not capitalized
  fontSize: '0.875rem', // Adjust the font size to make it smaller
  width: '150px',
};

const autoCompleteStyle = {
  width: '150px',
};

export function MainPage() {
  const [addIconBold, setAddIconBold] = useState(false);
  const [fireIconBold, setFireIconBold] = useState(false);
  const [fileButtonDisabled, setFileButtonDisabled] = useState(true);
  const [burnButtonDisabled, setBurnButtonDisabled] = useState(true);
  const [device, setDevice] = useState('');
  const [bridge, setBridge] = useState('');
  const [file, setFile] = useState(null);
  const {
    programDevice,
    getSupportedFileExtensionsByDeviceType,
  } = useDeviceProgrammiingApi();
  const {
    connectedBridges: bridges,
    supportedDevices: devices,
  } = useMonitorDeviceInfoContext();

  const onFileChange = () => {
    // eslint-disable-next-line no-undef
    const selectedFile = document.getElementById('fileUploadInput').files[0];

    setFile(selectedFile);

    // reset file array so on change event triggers each time a file is selected not just when
    // it changes
    document.getElementById('fileUploadInput').value = '';
  };

  const getBoldStyle = (isBold) => (
    { opacity: isBold ? '' : '25%' }
  );

  const onBurnClick = () => {
    programDevice.request(new ProgrammingInfo({
      BridgeType: bridge.bridgeType,
      BridgeSerialNbr: bridge.bridgeSerialNbr,
      DeviceType: device.deviceType,
      ProgramFilePath: file?.path,
    }));
  };

  const enableBurnStep = (flag) => {
    setBurnButtonDisabled(!flag);
    setFireIconBold(flag);
  };

  const enableFileStep = async (flag) => {
    setFile(null);
    setFileButtonDisabled(!flag);
    setAddIconBold(flag);
  };

  useEffect(() => {
    if (file) {
      enableBurnStep(true);
    }
  }, [file]);

  useEffect(() => {
    const runAsync = async () => {
      if (bridge && device) {
        const res = await getSupportedFileExtensionsByDeviceType.request(device?.deviceType);

        if (!res.isError) {
          enableFileStep(true);
        } else {
          enableFileStep(false);
          enableBurnStep(false);
        }
      }
    };

    runAsync();
  }, [bridge, device]);

  useEffect(() => {
    enableFileStep(false);
    enableBurnStep(false);
    setDevice('');
    setBridge('');
  }, [bridges, devices]);

  return (
    <Grid container mt={15} columns={12}>
      <Grid item xs={4} sm={3}>
        <Stack direction="column" spacing={3} sx={{ alignItems: 'center' }}>
          <MemoryIcon fontSize="large" />
          <Dropdown
            size="small"
            data={bridges}
            onChange={(item) => {
              setBridge({ ...item });
            }}
            value={bridge}
            textId="bridgeDescription"
            keyId="bridgeSerialNbr"
            labelId="selectBridgeLabelId"
            id="selectBridgeId"
            label="Select Bridge"
            sx={autoCompleteStyle}
          />
          <Dropdown
            size="small"
            data={devices}
            onChange={(item) => {
              setDevice(item);
            }}
            value={device}
            textId="deviceType"
            keyId="deviceType"
            labelId="selectDeviceLabelId"
            id="selectDeviceId"
            label="Select Device"
            sx={autoCompleteStyle}
          />
        </Stack>
      </Grid>
      <Grid item xs={0} sm={1.5}>
        <Line sx={getBoldStyle(addIconBold)} />
      </Grid>
      <Grid item xs={4} sm={3}>
        <Stack direction="column" spacing={3} sx={{ alignItems: 'center' }}>
          <AddBoxIcon sx={getBoldStyle(addIconBold)} fontSize="large" />
          <Button
            size="large"
            variant="contained"
            sx={buttonStyle}
            component="label"
            disabled={fileButtonDisabled}
          >
            {(!file ? 'Select File' : file.name)}
            <input
              id="fileUploadInput"
              type="file"
              hidden
              accept={getSupportedFileExtensionsByDeviceType.data.join(',')}
              onChange={onFileChange}
            />
          </Button>
        </Stack>
      </Grid>
      <Grid item xs={0} sm={1.5}>
        <Line sx={getBoldStyle(addIconBold)} />
      </Grid>
      <Grid item xs={4} sm={3}>
        <Stack direction="column" spacing={3} sx={{ alignItems: 'center' }}>
          <LocalFireDepartmentIcon sx={getBoldStyle(fireIconBold)} fontSize="large" />
          <Button
            size="large"
            variant="contained"
            sx={buttonStyle}
            disabled={burnButtonDisabled}
            onClick={onBurnClick}
          >
            {(!programDevice.loading ? 'Burn device' : <CircularProgress size={25} />)}
          </Button>
        </Stack>
      </Grid>
    </Grid>
  );
}
