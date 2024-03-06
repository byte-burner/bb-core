const path = require('path');
const { app } = require('electron');

export const global = {
  DOTNET_ENVIRONMENT: 'Production',
  ASPNETCORE_ENVIRONMENT: 'Production',
  BASE_REST_API_URL: 'http://localhost',
  Database__ConnectionString: `Data Source="${path.join(app.getPath('appData'), `${app.getName()}/net_iot_data.db`)}"`, // overwrites appsettings in dotnet
  NLog__targets__database__connectionString: `Data Source="${path.join(app.getPath('appData'), `${app.getName()}/net_iot_data.db`)}"`, // overwrites appsettings in dotnet
  UTIL_EXEC_PATH: path.join(process.resourcesPath, 'net_iot_util'),
  SCRIPTS_PATH: path.join(process.resourcesPath, 'scripts'),
  APP_NAME: 'Byte Burner',
  SERVER_START_SIGNAL: 'Application started',
};
