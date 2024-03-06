/* eslint-disable */
import React, {
  useState,
  createContext,
  useContext,
} from 'react';
import PropTypes from 'prop-types';
import { Outlet } from 'react-router-dom';

export const MainStoreContext = createContext(null);

export function useMainStore() {
  return useContext(MainStoreContext);
}

/**
 * By using this component we provide a centralizd state management store that
 * acts as the single source of truth for all of the state within the area/page.
 * Having a store like this for the entire page allows us to avoid inconvenient prop-drilling
 * Instead we can easily share/mutate state between parent and child components wihtout using props
 * Provider Pattern for React Architecture: https://mortenbarklund.com/blog/react-architecture-provider-pattern/
 */

export function MainStoreProvider() {
  /**
     * PLACE DEFAULT STATE HERE
      const _comment = 'completed';
     */

  /**
     * PLACE VARIABLE STATE HERE
     * ex: const [comment, setComment] = useState(comment);
     */

  /**
     * INITIALIZE VARIABLE STATE HERE
     * const store = {
     *  Team: [comment, setComment],
     * };
     */

  const store = {};

  return (
    <MainStoreContext.Provider value={store}>
      <Outlet />
    </MainStoreContext.Provider>
  );
}

MainStoreProvider.propTypess = {
  children: PropTypes.node.isRequired,
};

