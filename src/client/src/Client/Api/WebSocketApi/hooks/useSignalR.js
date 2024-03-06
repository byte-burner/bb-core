import * as signalR from '@microsoft/signalr';
import { useEffect, useRef, useState } from 'react';

const SocketState = {
  Connected: 'Connected',
  Disconnected: 'Disconnected',
  Disconnecting: 'Disconnecting',
  Connecting: 'Connecting',
};

export const useSignalR = ({ hubEndpoint }) => {
  const [connected, setConnected] = useState(false);
  let { current: hubConnection } = useRef(null);

  useEffect(() => {
    if (hubConnection) {
      // noop for consistent return
      return () => {};
    }

    hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${window.env.REST_API_URL}/${hubEndpoint}`)
      .withAutomaticReconnect()
      .build();

    return () => {
      // cleanup initial hub connection
      setConnected(false);
      hubConnection?.stop();
    };
  }, []);

  const ensureConnected = async (hub) => new Promise((resolve, reject) => {
    try {
      if (hub.state === SocketState.Connected) {
        resolve(hub);
      } else if (hub.state === SocketState.Disconnected) {
        hub.start()
          .then(() => {
            setConnected(true);
            resolve(hub);
          })
          .catch((err) => {
            reject(err);
          });
      } else if (hub.state === SocketState.Connecting || hub.state === SocketState.Disconnecting) {
        setTimeout(() => {
          ensureConnected(hub)
            .then(() => {
              resolve(hub);
            })
            .catch((err) => {
              reject(err);
            });
        }, 100);
      }
    } catch (err) {
      reject(err);
    }
  });

  const invoke = async (endpoint, ...args) => {
    try {
      if (hubConnection) {
        const connectedHub = await ensureConnected(hubConnection);
        connectedHub.invoke(endpoint, ...args);
      }
    } catch (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    }
  };

  const subscribe = async (endpoint, callback) => {
    try {
      if (hubConnection) {
        const connectedHub = await ensureConnected(hubConnection);

        connectedHub.on(endpoint, (data) => {
          callback(data);
        });
      }
    } catch (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    }
  };

  const unsubscribe = async (endpoint) => {
    try {
      if (hubConnection) {
        const connectedHub = await ensureConnected(hubConnection);

        connectedHub.off(endpoint);
      }
    } catch (error) {
      // eslint-disable-next-line no-console
      console.error(error);
    }
  };

  return {
    connected,
    invoke,
    subscribe,
    unsubscribe,
  };
};
