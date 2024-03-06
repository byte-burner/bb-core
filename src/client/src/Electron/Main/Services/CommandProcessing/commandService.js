const isElevated = require('native-is-elevated');
const sudoPrompt = require('@vscode/sudo-prompt');
const { exec } = require('child_process');
const { logger } = require('../../Plugins/Logger');

const title = 'byteBurner'; // must be alphanumeric and <= 70 chars

/**
 * DEFINE PUBLIC SERVICE FUNCTIONS
 */

/**
 * Checks to see if the current shell is running as admin
 * @returns {boolean} true if the shell is admin, otherwise returns false
 */
export const isAdmin = () => (process.platform === 'win32' ? isElevated() : (process.getuid() === 0));

/**
 * Runs a command or process as a non-elevated, regular user
 * @param {string} command - the command to execute as a non-elevated user
 */
export const runProcess = async (command) => {
  logger.debug(`Running command: '${command}'`);

  return new Promise((resolve, reject) => {
    exec(command, { name: title }, (error, stdout, stderr) => {
      if (stdout) {
        logger.debug('Logging commands standard output to console');
        // eslint-disable-next-line no-console
        console.log(stdout);
      }

      if (stderr) {
        logger.debug('Logging commands standard error to console');
        // eslint-disable-next-line no-console
        console.error(stderr);
      }

      if (error) {
        reject(error);
      } else {
        resolve(stdout.toString());
      }
    });
  });
};

/**
 * Runs a command or process as an elevated user
 * @param {string} command - the command to execute as an elevated user
 */
export const runProcessElevated = async (command) => {
  logger.debug(`Running command as elevated: '${command}'`);

  return new Promise((resolve, reject) => {
    sudoPrompt.exec(command, { name: title }, (error, stdout, stderr) => {
      if (stdout) {
        logger.debug('Logging commands standard output to console');
        // eslint-disable-next-line no-console
        console.log(stdout);
      }
      if (stderr) {
        logger.debug('Logging commands standard error to console');
        // eslint-disable-next-line no-console
        console.error(stderr);
      }

      if (error) {
        reject(error);
      } else {
        resolve(stdout.toString());
      }
    });
  });
};
