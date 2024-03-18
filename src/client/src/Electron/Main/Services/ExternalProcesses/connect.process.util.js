const path = require('path');
const fs = require('fs');
const { logger } = require('../../Plugins/Logger');

/**
 * Connects the net_iot_util process and sets the PATH (*nix) and Path (windows) env variable
 */
export const connectUtilProcess = () => {
  if (!fs.existsSync(path.join(process.env.UTIL_EXEC_PATH, process.env.UTIL_EXEC_NAME))) {
    logger.error(`Utility program not found. Executable file, ${process.env.UTIL_EXEC_NAME} does not exist in ${process.env.UTIL_EXEC_PATH}.`);
  } else {
    // Append the directory to the current Path env variable
    // eslint-disable-next-line max-len
    // process.env.Path = `${process.env.UTIL_EXEC_PATH}${path.delimiter}${process.env.Path}`; // update correct path env for program started from Powershell/Cmd
    console.log('****************************');
    console.log('Updating PATH');
    console.log('****************************');
    process.env.PATH = `${process.env.UTIL_EXEC_PATH}${path.delimiter}${process.env.PATH}`; // update correct path env for program started from Bash/Zsh/Etc..
  }

  logger.info(`Successfully connected to 'net_iot_util' at, ${process.env.UTIL_EXEC_PATH}`);
};
