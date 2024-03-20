/* eslint-disable max-len */
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
    const newPath = `${process.env.UTIL_EXEC_PATH}${path.delimiter}${process.env.PATH ?? process.env.Path}`; // PATH is set when running from bash like shells. Path is set when running from cmd/powershell in windows
    process.env.PATH = newPath; // update PATH for bash like shells (bash, zsh, etc)
    process.env.ORIGINAL_PATH = newPath; // update ORIGINAL_PATH for git bash on windows
    process.env.Path = newPath; // update Path for powershell/cmd on windows
  }

  logger.info(`Successfully connected to 'net_iot_util' at, ${process.env.UTIL_EXEC_PATH}`);
};
