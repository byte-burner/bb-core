import { useSignalR } from './useSignalR';

// API
export const useMonitorDeviceHub = () => {
  const {
    subscribe,
    unsubscribe,
  } = useSignalR({ hubEndpoint: 'MonitorDeviceHub' });

  const subscribeToReceiveBridgeInfo = async (callback) => subscribe('ReceiveBridgeInfo', callback);
  const unsubscribeFromReceiveBridgeInfo = async () => unsubscribe('ReceiveBridgeInfo');

  return {
    subscribeToReceiveBridgeInfo,
    unsubscribeFromReceiveBridgeInfo,
  };
};
