import React, { useState } from 'react';
import {
  Box,
  AppBar,
  Toolbar,
  Drawer,
  List,
  Typography,
  Divider,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Button,
} from '@mui/material';
import { Link } from 'react-router-dom';
import NightlightIcon from '@mui/icons-material/Nightlight';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { ThemeSettings } from '../components';

const drawerWidth = 240;
const defaultTitle = 'Theme';

export function SettingsPage() {
  const [title, setTitle] = useState(defaultTitle);
  const [showThemeSettings, setShowThemeSettings] = useState(true);

  const onThemeSettingClick = () => {
    setShowThemeSettings(true);
    setTitle(defaultTitle);
  };

  const showSettings = () => {
    if (showThemeSettings) {
      return (<ThemeSettings />);
    }

    return null;
  };

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
          <ListItem key="theme" disablePadding>
            <ListItemButton
              onClick={onThemeSettingClick}
            >
              <ListItemIcon>
                <NightlightIcon />
              </ListItemIcon>
              <ListItemText primary="Theme" />
            </ListItemButton>
          </ListItem>
        </List>
      </Drawer>
      <Box
        component="main"
        sx={{ flexGrow: 1, p: 3 }}
      >
        <Toolbar />
        {showSettings()}
      </Box>
    </Box>
  );
}
