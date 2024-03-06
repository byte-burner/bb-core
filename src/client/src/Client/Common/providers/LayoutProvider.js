import React, {
  useContext,
  createContext,
  useMemo,
  useState,
  useEffect,
} from 'react';
import PropTypes from 'prop-types';
import logoImageLight from '../../Assets/logo_light.svg';
import logoImageDark from '../../Assets/logo_dark.svg';

/**
 * THEME CONTEXT
 */
const LayoutContext = createContext({});

export const useLayoutContext = () => (useContext(LayoutContext));

/**
 * THEME PROVIDER
 */
export function LayoutProvider({ children }) {
  /** DATA */
  const [layoutConfig, setLayoutConfig] = useState({
    themeId: localStorage.getItem('themeId') ?? 'dark',
    logoImage: '',
  });

  /** PUBLIC FUNCTIONS */

  /** EFFECTS */
  useEffect(() => {
    const logoImage = layoutConfig?.themeId === 'light' ? logoImageLight : logoImageDark;
    setLayoutConfig({
      ...layoutConfig,
      logoImage,
    });
  }, [layoutConfig.themeId]);

  /** EXPOSE CONTEXT STATE */
  const providerValue = useMemo(
    () => ({
      // add more custom functionality here
      ...layoutConfig,
      layoutConfig,
      setLayoutConfig,
    }),
    [
      // add more custom functionality here
      layoutConfig,
      setLayoutConfig,
    ],
  );

  return (
    <LayoutContext.Provider value={providerValue}>
      {children}
    </LayoutContext.Provider>
  );
}

LayoutProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

LayoutProvider.displayName = 'LayoutProvider';
