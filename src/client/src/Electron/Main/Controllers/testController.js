import { logger } from '../Plugins/Logger';

// eslint-disable-next-line import/no-extraneous-dependencies
const { ipcMain } = require('electron');

/**
 * DEFINE HANDLER FUNCTIONS FOR ENDPOINTS
 */

/**
 * Log the provided data and stringify it.
 * @param {*} event - IPC event object.
 * @param {*} data - Data to log and stringify.
 */
const sendToMainFromReactTest = (event, data) => {
  logger.info(JSON.stringify(data));
};

/**
 * EXPOSE HANDLER FUNCTIONS
 */

/**
 * Expose handler function for simple test to send data over IPC from renderer to main process
 */
export const initTestHandlers = () => {
  ipcMain.on('test/sendToMainFromReactTest', sendToMainFromReactTest);
};
