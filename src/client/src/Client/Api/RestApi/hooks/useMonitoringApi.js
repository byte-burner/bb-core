import { useFetch } from './useFetch';

// REQUESTS
const useAddTestLogs = () => {
  const { request: _request, loading } = useFetch({
    endpoint: '/Monitoring/AddTestLogs',
    method: 'GET',
    defaultData: null,
  });

  const request = async () => _request();

  return {
    request,
    loading,
  };
};

const useGetAllLogs = () => {
  const {
    request: _request, resetData, loading, data,
  } = useFetch({
    endpoint: '/Monitoring/GetAllLogs',
    method: 'GET',
    defaultData: [],
  });

  const request = async () => _request();

  return {
    request,
    resetData,
    loading,
    data,
  };
};

const useDeleteAllLogs = () => {
  const { request: _request, loading } = useFetch({
    endpoint: '/Monitoring/DeleteAllLogs',
    method: 'GET',
    defaultData: null,
  });

  const request = async () => _request();

  return {
    request,
    loading,
  };
};

// API
export const useMonitoringApi = () => {
  const addTestLogs = useAddTestLogs();
  const getAllLogs = useGetAllLogs();
  const deleteAllLogs = useDeleteAllLogs();

  return {
    addTestLogs,
    getAllLogs,
    deleteAllLogs,
  };
};
