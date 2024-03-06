import React from 'react';
import {
  FormControl,
  FormLabel,
  RadioGroup,
  FormControlLabel,
  Radio,
} from '@mui/material';
import {
  useLayoutContext,
  useThemeContext,
} from '../../../../Common/providers';

export function ThemeSettings() {
  const {
    themeId,
  } = useLayoutContext();

  const {
    toggleTheme,
  } = useThemeContext();

  return (
    <FormControl>
      <FormLabel id="themeLabel">Themes</FormLabel>
      <RadioGroup
        name="themeRadioGroup"
        value={themeId}
        onChange={(e) => toggleTheme(e.target.value)}
      >
        <FormControlLabel value="ultra" control={<Radio />} label="Ultra" />
        <FormControlLabel value="light" control={<Radio />} label="Light" />
        <FormControlLabel value="dark" control={<Radio />} label="Dark" />
      </RadioGroup>
    </FormControl>
  );
}
