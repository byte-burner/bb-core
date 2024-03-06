const { app } = require('electron');
const { logger } = require('./Plugins/Logger');
const { initAllHandlers } = require('./Controllers');
const initializationService = require('./Services/InitializationProcessing/initializationService');

const {
  connectRestApiProcess,
  connectUtilProcess,
} = require('./Services/ExternalProcesses/externalProcessesService');

/**
 * Loads environment variable settings based on the current environment (NODE_ENV) and operating
 * system. The settings are loaded from specific configuration files for the current environment
 * and OS.
 */
const loadEnvironmentVariableSettings = () => {
  const os = process.platform === 'win32' ? 'win32' : 'unix';
  // eslint-disable-next-line import/no-dynamic-require, global-require
  const { settings } = require(`./Settings/settings.${process.env.NODE_ENV}.${os}`);
  // eslint-disable-next-line import/no-dynamic-require, global-require
  const { global } = require(`./Settings/settings.${process.env.NODE_ENV}.global`);
  // eslint-disable-next-line import/no-dynamic-require, global-require
  const { globalAll } = require('./Settings/settings.global');

  process.env = {
    ...process.env,
    ...globalAll,
    ...global,
    ...settings,
  };
};

/**
 * Startup function for the Electron application.
 * Performs initialization tasks such as loading environment variable settings,
 * connecting to API and utility processes, loading handlers, and preparing the OS environment.
 *
 * @throws {Error} Throws an error if there's an issue during the startup process.
 */
export const startup = async () => {
  try {
    // must load all env variables before anything else
    loadEnvironmentVariableSettings();

    // If the app is in a packaged state we assume we are running in prod,
    // otherwise we assume dev mode
    if (app.isPackaged) { // we are in release
      switch (process.env.NODE_ENV) {
        case 'production':
          await connectRestApiProcess();
          connectUtilProcess();
          logger.info('Electron is running in prod mode');
          break;
        case 'staging':
          logger.info("Staging environment isn't configured");
          break;
        case 'testing':
          logger.info("Testing environment isn't configured");
          break;

        default:
          throw new Error("Couldn't find an envrionment to startup");
      }
    } else { // we are in debug
      connectUtilProcess();
      logger.info('Electron is running in dev mode');
    }

    // load handlers
    initAllHandlers();

    // call initialization service to prepare the OS environment
    initializationService.prepareEnvironment();
  } catch (error) {
    throw new Error(error);
  }
};
