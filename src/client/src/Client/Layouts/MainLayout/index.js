import React from 'react';
import { Grid, Box, IconButton } from '@mui/material';
import { Link, Outlet } from 'react-router-dom';
import SettingsIcon from '@mui/icons-material/Settings';
import ListIcon from '@mui/icons-material/List';
import BoltIcon from '@mui/icons-material/Bolt';
import TerminalIcon from '@mui/icons-material/Terminal';
import { LayoutBase } from '../../Common';
import { useLayoutContext } from '../../Common/providers/LayoutProvider';

function Header() {
  const { logoImage } = useLayoutContext();

  return (
    <Grid container>
      <Grid item xs={5} />
      <Grid
        item
        xs={2}
        sx={{
          display: 'flex',
          alignItems: 'center',
        }}
      >
        <Box component="img" src={logoImage} />
      </Grid>
      <Grid
        item
        xs={5}
        sx={{
          textAlign: 'right',
          padding: 1,
        }}
      >
        <IconButton component={Link} to="terminal">
          <TerminalIcon />
        </IconButton>
        <IconButton component={Link} to="logging">
          <ListIcon />
        </IconButton>
        <IconButton component={Link} to="/">
          <BoltIcon />
        </IconButton>
        <IconButton component={Link} to="settings">
          <SettingsIcon />
        </IconButton>
      </Grid>
    </Grid>
  );
}

function Body() {
  return <Outlet />;
}

export function MainLayout() {
  return (
    <LayoutBase
      header={<Header />}
      body={<Body />}
    />
  );
}
