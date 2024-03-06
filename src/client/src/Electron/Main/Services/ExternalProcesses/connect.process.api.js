import chalk from 'chalk';

const { spawn } = require('child_process');
const fs = require('fs');
const portfinder = require('portfinder');
const { logger } = require('../../Plugins/Logger');

/**
 * Connect to the REST API process.
 * Verifies the existence of the API executable file, finds a free port to use,
 * sets up environment variables for the API process, spawns the API process,
 * and listens for the start signal of the server process.
 *
 * @returns {Promise<void>} A promise that resolves when the API process is successfully
 * connected and started.
 * @throws {Error} Throws an error if the API executable file doesn't exist or a free port
 * couldn't be found.
 */
export const connectRestApiProcess = async () => {
  if (!fs.existsSync(process.env.API_EXEC_PATH)) {
    throw new Error(`API not started. Executable file, ${process.env.API_EXEC_PATH} does not exist.`);
  }

  // find a port that isn't in use
  portfinder.setBasePort(5000); // default: 8000
  const freePort = await portfinder.getPortPromise();

  if (!freePort) {
    throw new Error(`API not started. Port, ${process.env.BASE_REST_API_URL}:${freePort} could not be used.`);
  }

  // add new env variables
  process.env.ASPNETCORE_URLS = `${process.env.BASE_REST_API_URL}:${freePort}`; // used by the local asp.net web api
  process.env.REST_API_URL = `${process.env.BASE_REST_API_URL}:${freePort}`;

  return new Promise((resolve, reject) => {
    try {
      // start the api process
      const apiChildProcess = spawn(process.env.API_EXEC_PATH, [], {
        env: process.env,
        stdio: 'pipe', // could use 'inherit' to bind child process stdio to parent stdio but events on stdout will not work, so we use 'pipe', instead!!
        // eslint-disable-next-line max-len
        windowsHide: true, // Hide the subprocess console window that would normally be created on windows systems
      });

      // listen for the exit event on the parent process
      process.on('exit', () => {
        // kill the child process when the parent is about to exit
        logger.info(`Quitting connected processes with pid list ${[apiChildProcess.pid]}`);
        apiChildProcess.kill();
      });

      // handle other signals to gracefully exit the child process
      ['SIGINT', 'SIGTERM', 'SIGQUIT'].forEach((signal) => {
        process.on(signal, () => {
          logger.info(`Quitting connected processes with pid list ${[apiChildProcess.pid]}`);
          apiChildProcess.kill(signal);
          process.exit();
        });
      });

      logger.info('Listening for the server start signal');

      // listen for the start signal of the server process
      apiChildProcess.stdout.on('data', (data) => {
        // stringify the data
        const output = data.toString();

        // forward the child process stdout to parent process stdout
        process.stdout.write(chalk.blue(output));

        if (output.includes(process.env.SERVER_START_SIGNAL)) {
          // eslint-disable-next-line max-len
          logger.info(`Successfully connected to 'net_iot_api'. Now listening on, ${process.env.BASE_REST_API_URL}:${freePort}`);
          resolve();
        }
      });
    } catch (error) {
      reject(error);
    }
  });
};
