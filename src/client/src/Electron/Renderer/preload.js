// eslint-disable-next-line import/no-extraneous-dependencies
const { contextBridge, ipcRenderer } = require('electron');

/**
 * EXPOSE PRELOAD FUNCTIONS
 */

contextBridge.exposeInMainWorld('bridge', {
  // send to channel
  send: (channel, ...args) => {
    ipcRenderer.send(channel, ...args);
  },
  // receive from channel
  receive: (channel, listener) => {
    ipcRenderer.on(channel, (event, ...args) => listener(...args));
  },
  // invoke channel
  invoke: async (channel, ...args) => ipcRenderer.invoke(channel, ...args),
});

/**
 * EXPOSE MAIN PROCESS ENV VARIABLES
 */

contextBridge.exposeInMainWorld('env', {
  REST_API_URL: process.env.REST_API_URL,
});
