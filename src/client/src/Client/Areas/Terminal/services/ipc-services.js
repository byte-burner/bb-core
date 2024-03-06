/* eslint-disable no-undef */

export const write = (pid, data) => {
  window.bridge.send('terminal/write', pid, data);
};

export const read = (pid, channel) => {
  window.bridge.invoke('terminal/read', pid, channel);
};

export const readCallback = (channel, callback) => {
  window.bridge.receive(channel, callback);
};

export const create = async (config) => window.bridge.invoke('terminal/create', JSON.stringify(config));

export const dispose = (pid) => {
  window.bridge.invoke('terminal/dispose', pid);
};

export const disposeAll = () => {
  window.bridge.invoke('terminal/disposeAll');
};

export const getAvailableShells = async () => window.bridge.invoke('terminal/getAvailableShells');
