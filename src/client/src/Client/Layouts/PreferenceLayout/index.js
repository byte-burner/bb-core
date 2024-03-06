import React from 'react';
import { Outlet } from 'react-router-dom';
import { LayoutBase } from '../../Common';

function Body() {
  return <Outlet />;
}

export function PreferenceLayout() {
  return (
    <LayoutBase
      body={<Body />}
    />
  );
}
