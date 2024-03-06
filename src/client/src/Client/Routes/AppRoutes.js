import React from 'react';
import {
  Route,
  Routes,
} from 'react-router-dom';
import {
  MainPage,
  MainStoreProvider,
  TerminalStoreProvider,
  TerminalPage,
  LoggingStoreProvider,
  LoggingPage,
  PreferenceStoreProvider,
  SettingsPage,
} from '../Areas';
import {
  MainLayout,
  PreferenceLayout,
} from '../Layouts';

export const AppRoutes = [

  {
    Key: 'MainArea',
    Provider: <MainStoreProvider />,
    Layout: <MainLayout />,
    Routes: [
      {
        path: '/',
        element: <MainPage />,
        requireAuth: false,
      },
    ],
  },
  {
    Key: 'TerminalArea',
    Provider: <TerminalStoreProvider />,
    Layout: <MainLayout />,
    Routes: [
      {
        path: '/terminal',
        element: <TerminalPage />,
        requireAuth: false,
      },
    ],
  },
  {
    Key: 'LoggingArea',
    Provider: <LoggingStoreProvider />,
    Layout: <MainLayout />,
    Routes: [
      {
        path: '/logging',
        element: <LoggingPage />,
        requireAuth: false,
      },
    ],
  },
  {
    Key: 'PreferencesAreaa',
    Provider: <PreferenceStoreProvider />,
    Layout: <PreferenceLayout />,
    Routes: [
      {
        path: '/settings',
        element: <SettingsPage />,
        requireAuth: false,
      },
    ],
  },
];

export const mapRoutes = () => (
  <Routes>
    {/**
      * MAP ROUTES
      */}
    {AppRoutes.map((Areas) => {
      const {
        Key,
        Provider,
        Layout,
      } = Areas;

      return (
        <Route key={Key} element={Provider}>
          <Route element={Layout}>
            {Areas.Routes.map((route) => {
              const { requireAuth, ...rest } = route;
              return <Route key={route} {...rest} />;
            })}
          </Route>
        </Route>
      );
    })}
  </Routes>
);
