import React, { useState, useEffect } from 'react';
import {
  Box,
  AppBar,
  Toolbar,
  Drawer,
  List,
  Typography,
  Divider,
  Button,
} from '@mui/material';
import { Link } from 'react-router-dom';
import MemoryIcon from '@mui/icons-material/Memory';
import NightlightIcon from '@mui/icons-material/Nightlight';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import HelpIcon from '@mui/icons-material/Help';
import { ThemeSettings, SettingsListItem } from '../components';
import { BridgeSettings } from '../components/Settings/BridgeSettings';
import { DeviceSettings } from '../components/Settings/DeviceSettings';
import { HelpSettings } from '../components/Settings/HelpSettings';
import { useDeviceProgrammiingApi } from '../../../Api/RestApi';

const drawerWidth = 240;
const settings = {
  themeSettings: false,
  bridgeSettings: false,
  deviceSettings: false,
  helpSettings: false,
};
const titleMap = {
  themeSettings: 'Theme',
  bridgeSettings: 'Bridge Info',
  deviceSettings: 'Device Info',
  helpSettings: 'Help',
};

export function SettingsPage() {
  const [title, setTitle] = useState('');
  const [showSettings, setShowSettings] = useState({ ...settings });
  const {
    getAllSupportedBridges,
    getAllSupportedDevices,
    getAllConnectedBridges,
  } = useDeviceProgrammiingApi();

  const onThemeSettingClick = () => {
    setShowSettings({
      ...settings,
      themeSettings: true,
    });
    setTitle(titleMap.themeSettings);
  };

  const onBridgeSettingClick = async () => {
    setShowSettings({
      ...settings,
      bridgeSettings: true,
    });
    setTitle(titleMap.bridgeSettings);

    // when the user opens the bridge settings page we want to check for connected bridges
    getAllConnectedBridges.request();
  };

  const onDeviceSettingClick = () => {
    setShowSettings({
      ...settings,
      deviceSettings: true,
    });
    setTitle(titleMap.deviceSettings);
  };

  const onHelpSettingClick = () => {
    setShowSettings({
      ...settings,
      helpSettings: true,
    });
    setTitle(titleMap.helpSettings);
  };

  const showSettingsPage = () => {
    if (showSettings.themeSettings) {
      return (<ThemeSettings />);
    } if (showSettings.bridgeSettings) {
      return (
        <BridgeSettings
          supportedBridges={getAllSupportedBridges.data.map((b) => b.bridgeType)}
          connectedBridges={getAllConnectedBridges.data.map((b) => b.bridgeType)}
          loadConnectedBridges={getAllConnectedBridges.request}
        />
      );
    } if (showSettings.deviceSettings) {
      return (
        <DeviceSettings
          supportedDevices={getAllSupportedDevices.data.map((b) => b.deviceType)}
        />
      );
    } if (showSettings.helpSettings) {
      return (<HelpSettings />);
    }

    return null;
  };

  useEffect(() => {
    setShowSettings({ ...settings, themeSettings: true });
    setTitle(titleMap.themeSettings);

    // make initial data requests
    getAllSupportedBridges.request();
    getAllSupportedDevices.request();
  }, []);

  return (
    <Box sx={{ display: 'flex', height: '100%' }}>
      <AppBar
        position="fixed"
        sx={{ width: `calc(100% - ${drawerWidth}px)`, ml: `${drawerWidth}px` }}
      >
        <Toolbar sx={{ justifyContent: 'space-between' }}>
          <Typography variant="h6" noWrap component="div">
            {title}
          </Typography>
          <Button
            sx={{
              color: 'white',
              ':hover': {
                backgroundColor: 'rgba(255, 255, 255, 0.2)',
              },
            }}
            size="small"
            component={Link}
            to="/"
            startIcon={<ArrowBackIcon />}
          >
            Back
          </Button>
        </Toolbar>
      </AppBar>
      <Drawer
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
        variant="permanent"
        anchor="left"
      >
        <Toolbar />
        <Divider />
        <List>
          <SettingsListItem
            text="Theme"
            onClick={onThemeSettingClick}
            icon={<NightlightIcon />}
          />
          <SettingsListItem
            text="Bridge Info"
            onClick={onBridgeSettingClick}
            icon={<MemoryIcon />}
          />
          <SettingsListItem
            text="Device Info"
            onClick={onDeviceSettingClick}
            icon={<MemoryIcon />}
          />
          <SettingsListItem
            text="Help"
            onClick={onHelpSettingClick}
            icon={<HelpIcon />}
          />
        </List>
      </Drawer>
      <Box
        component="main"
        sx={{ flexGrow: 1, p: 3 }}
      >
        <Toolbar />
        {showSettingsPage()}
      </Box>
    </Box>
  );
}
