import { createTheme } from '@mui/material/styles';
import { components } from './components';
import { palette } from './palette';

/**
 * Material UI exposes an API to change the default themes
 * Default theme: https://mui.com/customization/default-theme/
 * Theming examples: https://mui.com/customization/theming/
 * Global style overrides: https://mui.com/customization/theme-components/
 */
export const LightTheme = createTheme({
  components,
  palette,
});
