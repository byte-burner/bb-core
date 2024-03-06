import React, {
  useContext,
  createContext,
  useMemo,
} from 'react';
import PropTypes from 'prop-types';
import { ThemeProvider as MuiThemeProvider, useTheme } from '@mui/material/styles';
import {
  LightTheme,
  DarkTheme,
  UltraTheme,
} from '../themes';
import { useLayoutContext } from './LayoutProvider';

/**
 * CONSTANTS
 */
const themeMap = {
  light: { id: 'light', theme: LightTheme },
  dark: { id: 'dark', theme: DarkTheme },
  ultra: { id: 'ultra', theme: UltraTheme },
};

/**
 * THEME CONTEXT
 */
const ThemeContext = createContext({});

export const useThemeContext = () => {
  const muiTheme = useTheme();
  const theme = useContext(ThemeContext);

  return {
    ...muiTheme,
    ...theme,
  };
};

/**
 * THEME PROVIDER
 */
export function ThemeProvider({ children }) {
  /** DATA */
  const {
    layoutConfig,
    themeId,
    setLayoutConfig,
  } = useLayoutContext();

  /** PUBLIC FUNCTIONS */
  const toggleTheme = (newThemeId) => {
    localStorage.setItem('themeId', newThemeId);
    setLayoutConfig({
      ...layoutConfig,
      themeId: newThemeId,
    });
  };

  /** EXPOSE CONTEXT STATE */
  const providerValue = useMemo(
    () => ({
      // add more custom functionality here
      toggleTheme,
      themeMap,
    }),
    [
      // add more custom functionality here
      toggleTheme,
      themeMap,
    ],
  );

  return (
    <MuiThemeProvider theme={{ ...themeMap[themeId]?.theme }}>
      <ThemeContext.Provider value={providerValue}>
        {children}
      </ThemeContext.Provider>
    </MuiThemeProvider>
  );
}

ThemeProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

ThemeProvider.displayName = 'ThemeProvider';
