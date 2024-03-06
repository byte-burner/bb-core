'use strict'

/*
    This file will generate the appsettings.Development.json file to run the api locally.
    This file can be run by running 'node generateAppsettings.js'
*/

/**
 * Imports
 */
const readline = require('readline');
const os = require('os');
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});
const fs = require('fs');
const path = require('path');
const { exit } = require('process');

/**
 * Private Helper Functions
 */
const prompt = (input, defaultText) => {
    return new Promise((resolve) => {
        const answer = rl.question(input, (output) => {
            resolve(output);
        });

        if (defaultText) {
            rl.write(defaultText);            
        }
    });
};

const createJsonFile = async (targetDir, filename, settings) => {
    const fullFilePath = targetDir + '/' + filename;
    const folderExsts = fs.existsSync(targetDir);
    if (!folderExsts) {
        await fs.promises.mkdir(targetDir);
    }

    await fs.promises.writeFile(fullFilePath, JSON.stringify(settings, null, 2));
};

const createFile = async (targetDir, filename, options) => {
    const fullFilePath = targetDir + '/' + filename;
    const folderExsts = fs.existsSync(targetDir);
    if (!folderExsts) {
        await fs.promises.mkdir(targetDir);
    }

    await fs.promises.writeFile(fullFilePath, options);
};

const summary = async (options) => {
    await prompt(
        "\n\n-----Summary Info-----" +
        `\n\nNlog log file=${options.log_file}` + 
        "\n\nPress <Enter> key to close..."
    );
};

/**
 * Launch Settings
 */
const buildLaunchSettings = (options) => {
    const launchSettings = {
      $schema: "https://json.schemastore.org/launchsettings.json",
      profiles: {
        net_iot_util: {
          commandName: "Project",
          dotnetRunMessages: true,
          environmentVariables: {
            DOTNET_ENVIRONMENT: "Development"
          }
        }
      }
    };

    createJsonFile("./Properties", "launchSettings.json", launchSettings);
}

/**
 * App Settings
 */
const buildSettings = async (options) => {
  const settings = {
    NLog: {
        throwConfigExceptions: true,
        throwExceptions: true,
        ...options.log_internal,
        targets: {
          async: true,
          file: {
            type: "File",
            fileName: options.log_file
          }
        },
        rules: [
          {
            logger: "*",
            minLevel: options.log_file_level,
            writeTo: "file"
          }
        ]
      }
    }

    createJsonFile("./Settings", "appsettings.Development.json", settings);
};

/**
 * Main
 */
const main = async () => {
    // nlog
    console.log("\n\n-----Enter connection options for nlog below-----");
    const log_file = await prompt(
      "\nEnter the log file path for the file target (i.e. absolute file path required): ",
      "/Users/joshbender/DocumentsLocal/Github/bb-core/src/net_iot/net_iot_util/nlog.dump");
    const log_file_level = await prompt("\nEnter the log level for the file target: (Trace|Debug|Info|Warn|Error): ", "Debug");
    const log_use_internal = await prompt("\nSetup internal logging for nlog [Y/n]: ");
    let log_internal = {};
    if (log_use_internal == "Y" || log_use_internal == "y") {
      const log_internal_level = await prompt("\nEnter the internal log level: (Trace|Debug|Info|Warn|Error): ", "Error");
      const log_internal_path = await prompt(
        "\nEnter the path to the internal log file (i.e. absolute file path required): ",
        "/Users/joshbender/DocumentsLocal/Github/bb-core/src/net_iot/net_iot_util/nlog_internal.dump");

      if (log_use_internal) {
        log_internal = {
          internalLogFile: log_internal_path,
          internalLogLevel: log_internal_level,
        }
      }
    }

    // create options
    const options = {
      log_file,
      log_file_level,
      log_internal,
    }

    // builds the settings files
    buildSettings(options);
    buildLaunchSettings(options);

    // shows summary
    await summary(options);

    rl.close();
}

main();