const fs = require('fs');
const commandService = require('../CommandProcessing/commandService');
const { logger } = require('../../Plugins/Logger');

/**
 * PRIVATE HELPER FUNCTIONS
 */

/**
 * Attempts to find and run scripts to configure the linux environment correctly
 * @throws {Error} Throws an error if there's an issue preparing the Linux environment.
 */
const prepareLinuxEnvironment = () => {
  try {
    if (!fs.existsSync(process.env.UDEV_FT232H_FILE)) {
      const pathToShellScript = `${process.env.SCRIPTS_PATH}/linux/setupUDevRules.sh`;
      const bashCommand = `bash ${pathToShellScript}`;

      if (commandService.isAdmin()) {
        commandService.runProcess(bashCommand);
      } else {
        commandService.runProcessElevated(bashCommand);
      }
    }
  } catch (error) {
    throw new Error(error);
  }
};

/**
 * PUBLIC SERVICE FUNCTIONS
 */

/**
 * Prepares the environment based on the current platform.
 * Depending on the platform, it may configure specific environment settings.
 */
export const prepareEnvironment = () => {
  switch (process.platform) {
    case 'darwin':
    case 'freebsd':
      logger.debug('Nothing to configure for darwin');
      break;
    case 'linux':
      prepareLinuxEnvironment();
      break;
    case 'win32':
      logger.debug('Nothing to configure for win32');
      break;
    default:
      break;
  }
};
