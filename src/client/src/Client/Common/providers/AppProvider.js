import React from 'react';
import { ThemeProvider } from './ThemeProvider';
import { LayoutProvider } from './LayoutProvider';
import { NotificationProvider } from './NotificationProvider';
import { MonitorDeviceInfoProvider } from './MonitorDeviceInfoProvider';

/* eslint-disable */
const combineComponents = (components) => {
  return components.reduce(
    (AccumulatedComponents, CurrentComponent) => {
      return ({ children }) => {
        return (
          <AccumulatedComponents>
            <CurrentComponent>{children}</CurrentComponent>
          </AccumulatedComponents>
        );
      };
    },
    ({ children }) => <>{children}</>,
  );
};
/* eslint-enable */

/**
 * These providers must be in the correct order in the array below:
 * 1. Layout Provider (top level)
 * 2. Theme Provider (uses auth and layout provider)
 * 3. Other global providers that we may add...
 */

const providers = [
  LayoutProvider,
  ThemeProvider,
  NotificationProvider,
  MonitorDeviceInfoProvider,
  // add more global providers here
];

export const AppProvider = combineComponents(providers);
