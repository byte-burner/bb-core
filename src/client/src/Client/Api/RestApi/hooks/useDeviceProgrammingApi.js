import { useFetch } from './useFetch';

// REQUESTS
const useStartMonitoringBridgeEvents = () => {
  const { request: _request, loading } = useFetch({
    endpoint: '/Programming/StartMonitoringBridgeEvents',
    method: 'GET',
  });

  const request = async () => _request();

  return {
    request,
    loading,
  };
};

const useGetAllSupportedDevices = () => {
  const { request: _request, loading, data } = useFetch({
    endpoint: '/Programming/GetAllSupportedDevices',
    method: 'GET',
    defaultData: [],
  });

  const request = async () => _request();

  return {
    request,
    loading,
    data,
  };
};

const useGetAllConnectedBridges = () => {
  const { request: _request, loading, data } = useFetch({
    endpoint: '/Programming/GetAllConnectedBridges',
    method: 'GET',
    defaultData: [],
  });

  const request = async () => _request();

  return {
    request,
    loading,
    data,
  };
};

const useProgramDevice = () => {
  const { request: _request, loading } = useFetch({
    endpoint: '/Programming/ProgramDevice',
    method: 'POST',
    defaultData: null,
  });

  const request = async (body) => _request({ body });

  return {
    request,
    loading,
  };
};

const useGetSupportedFileExtensionsByDeviceType = () => {
  const {
    request: _request,
    loading,
    data,
    resetData,
  } = useFetch({
    endpoint: '/Programming/GetSupportedFileExtensionsByDeviceType',
    method: 'GET',
    defaultData: [],
  });

  const request = async (type) => _request({ paramList: `type=${type}` });

  return {
    request,
    loading,
    data,
    resetData,
  };
};

// API
export const useDeviceProgrammiingApi = () => {
  const startMonitoringBridgeEvents = useStartMonitoringBridgeEvents();
  const programDevice = useProgramDevice();
  const getAllConnectedBridges = useGetAllConnectedBridges();
  const getAllSupportedDevices = useGetAllSupportedDevices();
  const getSupportedFileExtensionsByDeviceType = useGetSupportedFileExtensionsByDeviceType();

  return {
    startMonitoringBridgeEvents,
    programDevice,
    getAllConnectedBridges,
    getAllSupportedDevices,
    getSupportedFileExtensionsByDeviceType,
  };
};
