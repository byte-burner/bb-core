import { logger } from '../Plugins/Logger';

// eslint-disable-next-line import/no-extraneous-dependencies
const { ipcMain } = require('electron');
const os = require('os');
const fs = require('fs');
const pty = require('node-pty');

/**
 * CONSTANTS
 */

const executableTypeList = [
  'bash',
  'zsh',
  'powershell',
  'cmd',
];

const windowsTypeShellMap = (type) => {
  switch (type) {
    case 'bash':
      return { path: 'C:\\Program Files\\Git\\usr\\bin\\bash.exe', args: [] };
    case 'cmd':
      return { path: 'C:\\Windows\\System32\\cmd.exe', args: [] };
    case 'powershell':
      return { path: 'C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe', args: [] };
    default:
      return null;
  }
};

const unixTypeShellMap = (type) => {
  switch (type) {
    case 'bash':
      return { path: '/bin/bash', args: ['--rcfile', `${process.env.SCRIPTS_PATH}/linux/setupBashShell.sh`] };
    case 'zsh':
      return { path: '/bin/zsh', args: [] };
    default:
      return null;
  }
};

/**
 * PROPERTIES
 */

const ptyProcessList = [];

/**
 * PRIVATE HELPER FUNCTIONS
 */

/**
 * Get the platform-specific shell configuration from the type.
 * @param {string} type - The type of shell (e.g., 'bash', 'cmd').
 * @returns {Object|null} The shell configuration or null if not found.
 */
const getPlatformShellFromType = (type) => {
  const shell = (os.platform() === 'win32') ? windowsTypeShellMap(type) : unixTypeShellMap(type);
  return fs.existsSync(shell?.path) ? shell : null;
};

/**
 * Get the shell configuration by type.
 * @param {string} type - The type of shell (e.g., 'bash', 'cmd').
 * @returns {Object|null} The shell configuration or null if not found.
 */
const getShellByType = (type) => getPlatformShellFromType(type);

/**
 * DEFINE HANDLER FUNCTIONS FOR ENDPOINTS
 */

/**
 * Create a new PTY (Pseudo Terminal).
 * @param {*} _ - Unused parameter.
 * @param {string} conf - Configuration for creating PTY.
 * @returns {number|null} The process ID (PID) of the created PTY or null if creation failed.
 */
const create = (_, conf) => {
  const config = JSON.parse(conf);
  if (!config) {
    throw new Error('Unable to create PTY. Check config');
  }

  try {
    const shell = getShellByType(config?.type);

    const ptyProcess = pty.spawn(shell?.path, shell?.args, {
      name: 'xterm-256color',
      cols: config.cols,
      rows: config.rows,
      cwd: os.homedir(), // login at the home directory
      env: process.env,
      useConpty: false, // use winpty instead for windows platforms only
    });

    ptyProcessList.push(ptyProcess);

    return ptyProcess.pid;
  } catch (error) {
    logger.error(`Couldn't create terminal with config: ${conf}.`, error);
  }

  return null;
};

/**
 * Write data to the PTY with specified PID.
 * @param {*} _ - Unused parameter.
 * @param {number} pid - Process ID (PID) of the PTY.
 * @param {string} data - Data to write to the PTY.
 */
const write = (_, pid, data) => {
  try {
    const ptyProcess = ptyProcessList.find((p) => p.pid === pid);

    if (ptyProcess) {
      ptyProcess.write(data);
    }
  } catch (error) {
    logger.error(`Couldn't read from terminal with pid: ${pid}.`, error);
  }
};

/**
 * Read data from the PTY with specified PID and send it over IPC.
 * @param {*} event - IPC event object.
 * @param {number} pid - Process ID (PID) of the PTY.
 * @param {string} channel - Channel name to send the data over IPC.
 */
const read = (event, pid, channel) => {
  try {
    const ptyProcess = ptyProcessList.find((p) => p.pid === pid);

    if (ptyProcess) {
      ptyProcess.onData((data) => {
        event.sender.send(channel, data);
      });
    }
  } catch (error) {
    logger.error(`Couldn't read from terminal with pid: ${pid}.`, error);
  }
};

/**
 * Get all active PTY processes.
 * @returns {Array} Array of active PTY processes.
 */
const getAll = () => ptyProcessList.filter((p) => p.pid);

/**
 * Dispose of a PTY process with the specified PID.
 * @param {*} _ - Unused parameter.
 * @param {number} pid - Process ID (PID) of the PTY to dispose.
 */
const dispose = (_, pid) => {
  try {
    const ptyProcess = ptyProcessList.find((p) => p.pid === pid);

    if (ptyProcess) {
      ptyProcess.kill();
      ptyProcessList.splice(ptyProcessList.indexOf(ptyProcess), 1);
    }
  } catch (error) {
    logger.error(`Couldn't dispose terminal with pid: : ${pid}.`, error);
  }
};

/**
 * Dispose all active PTY processes.
 */
const disposeAll = () => {
  try {
    ptyProcessList.map((p) => p.kill());
    ptyProcessList.splice(0, ptyProcessList.length);
  } catch (error) {
    logger.error(`Couldn't dispose all terminals: ${ptyProcessList}.`, error);
  }
};

/**
 * Get available shell types based on executableTypeList and system configurations.
 * @returns {Array} Array of available shell types.
 */
const getAvailableShells = () => (executableTypeList.filter((t) => Boolean(getShellByType(t))));

/**
 * EXPOSE HANDLER FUNCTIONS
 */

/**
 * Expose handler functions to handle terminal operations.
 */
export const initTerminalHandlers = () => {
  ipcMain.handle('terminal/create', create);
  ipcMain.on('terminal/write', write);
  ipcMain.handle('terminal/read', read);
  ipcMain.handle('terminal/getAll', getAll);
  ipcMain.handle('terminal/dispose', dispose);
  ipcMain.handle('terminal/disposeAll', disposeAll);
  ipcMain.handle('terminal/getAvailableShells', getAvailableShells);
};
