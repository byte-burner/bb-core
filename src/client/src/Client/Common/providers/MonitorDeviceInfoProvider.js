import React, {
  useContext,
  createContext,
  useMemo,
  useState,
  useEffect,
} from 'react';
import PropTypes from 'prop-types';
import { useMonitorDeviceHub } from '../../Api/WebSocketApi';
import { useDeviceProgrammiingApi } from '../../Api/RestApi';

/**
 * THEME CONTEXT
 */
const MonitorDeviceInfoContext = createContext({});

export const useMonitorDeviceInfoContext = () => (useContext(MonitorDeviceInfoContext));

/**
 * THEME PROVIDER
 */
export function MonitorDeviceInfoProvider({ children }) {
  /** DATA */
  const [connectedBridges, setConnectedBridges] = useState([]);
  const [supportedDevices, setSupportedDevices] = useState([]);
  const {
    subscribeToReceiveBridgeInfo,
    unsubscribeFromReceiveBridgeInfo,
  } = useMonitorDeviceHub();
  const {
    startMonitoringBridgeEvents,
    getAllSupportedDevices,
  } = useDeviceProgrammiingApi();

  /** PUBLIC FUNCTIONS */

  /** EFFECTS */
  useEffect(() => {
    const runAsync = async () => {
      await subscribeToReceiveBridgeInfo((bridgeInfo) => {
        setConnectedBridges([...bridgeInfo]);
      });
      await startMonitoringBridgeEvents.request();
      const resData = await getAllSupportedDevices.request();
      setSupportedDevices([...resData]);
    };

    runAsync();

    return async () => {
      await unsubscribeFromReceiveBridgeInfo();
    };
  }, []);

  /** EXPOSE CONTEXT STATE */
  const providerValue = useMemo(
    () => ({
      // add more custom functionality here
      connectedBridges,
      supportedDevices,
    }),
    [
      // add more custom functionality here
      connectedBridges,
      supportedDevices,
    ],
  );

  return (
    <MonitorDeviceInfoContext.Provider value={providerValue}>
      {children}
    </MonitorDeviceInfoContext.Provider>
  );
}

MonitorDeviceInfoProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

MonitorDeviceInfoProvider.displayName = 'MonitorDeviceInfoProvider';
