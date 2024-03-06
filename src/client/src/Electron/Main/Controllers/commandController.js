// eslint-disable-next-line import/no-extraneous-dependencies
const { ipcMain } = require('electron');
const commandService = require('../Services/CommandProcessing/commandService');

/**
 * DEFINE HANDLER FUNCTIONS FOR ENDPOINTS
 */

/**
 * Check if the current user has administrative privileges.
 * @returns {boolean} True if the user is an administrator, otherwise false.
 */
const isAdmin = () => commandService.isAdmin();

/**
 * Run a process asynchronously.
 * @param {*} _ - Unused parameter.
 * @param {string} command - The command to run.
 */
const runProcess = async (_, command) => {
  await commandService.runProcess(command);
};

/**
 * Run a process with elevated privileges asynchronously.
 * @param {*} _ - Unused parameter.
 * @param {string} command - The command to run with elevated privileges.
 */
const runProcessElevated = async (_, command) => {
  await commandService.runProcessElevated(command);
};

/**
 * EXPOSE HANDLER FUNCTIONS
 */

/**
 * Expose handler functions to handle command operations.
 */
export const initCommandHandlers = () => {
  ipcMain.handle('command/isAdmin', isAdmin);
  ipcMain.handle('command/runProcess', runProcess);
  ipcMain.handle('command/runProcessElevated', runProcessElevated);
};
