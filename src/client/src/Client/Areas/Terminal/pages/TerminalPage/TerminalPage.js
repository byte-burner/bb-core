import React from 'react';
import {
  Box,
} from '@mui/material';
import {
  TerminalToolbar,
  AddToolbarMenuButton,
  OpenToolbarDrawerButton,
  RemoveToolbarButton,
} from './Toolbar';
import {
  Terminal,
  useTerminalManager,
} from '../../hooks';
import { useThemeContext } from '../../../../Common/providers';

export function TerminalPage() {
  const {
    add,
    open,
    remove,
    removeAll,
    terminal,
    terminals,
    terminalTypes,
  } = useTerminalManager();

  const { palette } = useThemeContext();

  return (
    <Box
      sx={{
        height: '100%',
        width: '100%',
        padding: 2,
      }}
    >
      <TerminalToolbar type={terminal?.getPidType() ?? ''}>
        <RemoveToolbarButton
          onClick={removeAll}
        />
        <OpenToolbarDrawerButton
          onItemClick={(selected) => open(selected)}
          onTrashClick={(selected) => remove(selected)}
          items={terminals}
          current={terminal}
        />
        <AddToolbarMenuButton
          onItemClick={(selected) => add(new Terminal({
            type: selected,
            containerId: 'terminalContainer', // needs to match the DOM id for terminal container div
            height: '315px',
            backgroundColor: palette.background.default,
            foregroundColor: palette.text.primary,
            terminalId: 'terminalId',
          }))}
          items={terminalTypes}
        />
      </TerminalToolbar>
      <Box id="terminalContainer" />
    </Box>
  );
}
