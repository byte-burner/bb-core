import { useState } from 'react';
import { useNotificationContext } from '../../../Common/providers/NotificationProvider';

const DEFAULT_FETCH_OPTIONS = {
  headers: {
    'Content-Type': 'application/json',
  },
};

export const useFetch = ({ endpoint, method, defaultData }) => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState(defaultData);
  const { showNotifications } = useNotificationContext();

  const request = async ({
    body = null,
    paramList = '',
    options = {},
  } = {}) => {
    setLoading(true);

    const fetchOptions = {
      method,
      ...DEFAULT_FETCH_OPTIONS,
      ...options, // will override default fetch options
    };

    if (body) {
      fetchOptions.body = JSON.stringify(body);
    }

    if (paramList) {
      /* eslint-disable no-param-reassign */
      endpoint = `${endpoint}?${paramList}`;
    }

    try {
      const response = await fetch(`${window.env.REST_API_URL}${endpoint}`, fetchOptions);

      if (!response.ok) { // handle 400 and 500 response code
        const resData = await response.json();
        showNotifications(resData?.errors, 'error');
        return resData;
      } if (defaultData) { // handle 200 response code
        const resData = await response.json();
        setData(resData);
        return resData;
      }
    } catch (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    }

    setLoading(false);

    return defaultData;
  };

  const resetData = () => {
    setData([...defaultData]);
  };

  return {
    request,
    resetData,
    loading,
    data,
  };
};
