import React, { useEffect, useState } from 'react';
import {
  Grid,
  Button,
  Box,
  CircularProgress,
} from '@mui/material';
import { styled } from '@mui/material/styles';
import LocalFireDepartmentIcon from '@mui/icons-material/LocalFireDepartment';
import AddBoxIcon from '@mui/icons-material/AddBox';
import MemoryIcon from '@mui/icons-material/Memory';
import { Dropdown } from '../../../Common/components';
import { useDeviceProgrammiingApi } from '../../../Api/RestApi';
import { useMonitorDeviceInfoContext } from '../../../Common/providers';
import { ProgrammingInfo } from '../../../Api/models';

const Item = styled(Box)(() => ({
  position: 'relative',
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
}));

// eslint-disable-next-line react/prop-types
function Line({ sx }) {
  return (
    <Box
      sx={{
        position: 'absolute',
        top: 'calc(35px / 2)',
        bottom: 0,
        left: 'calc((266.66px / 2) + 35px)',
        right: 0,
        width: 'calc(266.66px - 70px)',
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
    <Grid container paddingTop={15}>

      <Grid
        container
      >
        <Grid item xs={4} position="relative">
          <Item>
            <MemoryIcon fontSize="large" />
          </Item>
          <Line sx={getBoldStyle(addIconBold)} />
        </Grid>

        <Grid item xs={4} position="relative" display="flex" justifyContent="center">
          <Item>
            <AddBoxIcon sx={getBoldStyle(addIconBold)} fontSize="large" />
          </Item>
          <Line sx={getBoldStyle(fireIconBold)} />
        </Grid>

        <Grid item xs={4} position="relative">
          <Item>
            <LocalFireDepartmentIcon sx={getBoldStyle(fireIconBold)} fontSize="large" />
          </Item>
        </Grid>

      </Grid>

      <Grid
        container
        paddingTop={5}
      >

        <Grid item xs={4}>
          <Item>
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
          </Item>
        </Grid>

        <Grid item xs={4}>
          <Item>
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
          </Item>
        </Grid>

        <Grid item xs={4}>
          <Item>
            <Button
              size="large"
              variant="contained"
              sx={buttonStyle}
              disabled={burnButtonDisabled}
              onClick={onBurnClick}
            >
              {(!programDevice.loading ? 'Burn device' : <CircularProgress size={25} />)}
            </Button>
          </Item>
        </Grid>

      </Grid>

      <Grid
        container
        paddingTop={3}
      >
        <Grid item xs={4}>
          <Item>
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
          </Item>
        </Grid>
      </Grid>

    </Grid>
  );
}
