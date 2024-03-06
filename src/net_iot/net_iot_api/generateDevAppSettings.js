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
        `\n\nData Source=${options.db_path}` + 
        `\n\nApi Base Port: ${options.api_appPort}` + 
        "\n\nPress <Enter> key to close..."
    );
};

/**
 * Launch Settings
 */
const buildLaunchSettings = (options) => {
    const launchSettings = {
        $schema: "https://json.schemastore.org/launchsettings.json",
        iisSettings: {
            windowsAuthentication: false,
            anonymousAuthentication: true,
            iisExpress: {
                applicationUrl: `http://localhost:${options.api_appPort}`,
                sslPort: options.api_sslPort
            }
        },
        profiles: {
            net_iot_api: {
                commandName: "Project",
                dotnetRunMessages: true,
                launchBrowser: true,
                launchUrl: "swagger",
                applicationUrl: `http://localhost:${options.api_appPort}`,
                environmentVariables: {
                    ASPNETCORE_ENVIRONMENT: "Development"
                }
            },
            'IIS Express': {
                commandName: "IISExpress",
                launchBrowser: true,
                launchUrl: "swagger",
                environmentVariables: {
                    ASPNETCORE_ENVIRONMENT: "Development"
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
        Database: {
            ConnectionString: `Data Source=${options.db_path}`
        },
        NLog: {
            throwConfigExceptions: true,
            throwExceptions: true,
            ...options.log_internal,
            targets: {
              async: true,
              console: {
                type: "ColoredConsole",
                rowHighlightingRules : [
                  {
                    condition: "level == LogLevel.Debug",
                    foregroundColor: "DarkGray"
                  },
                  {
                    condition: "level == LogLevel.Info",
                    foregroundColor: "Blue"
                  },
                  {
                    condition: "level == LogLevel.Warn",
                    foregroundColor: "Yellow"
                  },
                  {
                    condition: "level == LogLevel.Error",
                    foregroundColor: "Red"
                  },
                  {
                    condition: "level == LogLevel.Fatal",
                    foregroundColor: "Red",
                    backgroundColor: "White"
                  }
                ]
              },
              database: {
                type: "Database",
                dbProvider: "Microsoft.Data.Sqlite.SqliteConnection, Microsoft.Data.Sqlite",
                connectionString: `Data Source=${options.db_path}`,
                keepConnection: true,
                commandText: "INSERT INTO message_log (date, level, message, exception) VALUES (@addeddate, @level, @message, @exception);",
                parameters: [
                  {
                    name: "@addeddate",
                    layout: "${date}"
                  },
                  {
                    name: "@level",
                    layout: "${level}"
                  },
                  {
                    name: "@message",
                    layout: "${message}"
                  },
                  {
                    name: "@exception",
                    layout: "${exception:tostring}"
                  }
                ]
              }
            },
            rules: [
              {
                logger: "*",
                minLevel: options.log_con_level,
                writeTo: "console"
              },
              {
                logger: "*",
                minLevel: options.log_db_level,
                writeTo: "database"
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
    // Developer fills these out during creation of api
    const api_sslPort = 0;
    const api_appPort = 5100;

    // database
    console.log("\n\n-----Enter connection options for the database below-----");
    const db_path = await prompt(
        "\nEnter the path to the sqlite database: ",
        "/Users/joshbender/DocumentsLocal/Github/8051-programmer/src/net_iot/net_iot_data/sqlite.db");

    // nlog
    console.log("\n\n-----Enter connection options for nlog below-----");
    const log_db_level = await prompt("\nEnter the log level for the database target: (Trace|Debug|Info|Warn|Error): ", "Debug");
    const log_con_level = await prompt("\nEnter the log level for the console target: (Trace|Debug|Info|Warn|Error): ", "Debug");
    const log_use_internal = await prompt("\nSetup internal logging for nlog [Y/n]: ");
    let log_internal = {};
    if (log_use_internal == "Y" || log_use_internal == "y") {
      const log_internal_level = await prompt("\nEnter the internal log level: (Trace|Debug|Info|Warn|Error): ", "Error");
      const log_internal_path = await prompt(
        "\nEnter the path to the internal log file (i.e. absolute file path required): ",
        "/Users/joshbender/DocumentsLocal/Github/8051-programmer/src/net_iot/net_iot_data/sqlite.db");

      if (log_use_internal) {
        log_internal = {
          internalLogFile: log_internal_path,
          internalLogLevel: log_internal_level,
        }
      }
    }

    // create options
    const options = {
        db_path,
        log_internal,
        log_db_level,
        log_con_level,
        api_sslPort,
        api_appPort,
    }

    // builds the settings files
    buildSettings(options);
    buildLaunchSettings(options);

    // shows summary
    await summary(options);

    rl.close();
}

main();