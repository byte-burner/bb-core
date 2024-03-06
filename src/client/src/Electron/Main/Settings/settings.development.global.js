const path = require('path');

export const global = {
  DOTNET_ENVIRONMENT: 'Development',
  ASPNETCORE_ENVIRONMENT: 'Development',
  REST_API_URL: 'http://localhost:5100',
  UTIL_EXEC_PATH: path.join(__dirname, '../../src/Resources/net_iot_util'),
  SCRIPTS_PATH: path.join(__dirname, '../../src/Resources/scripts'),
  APP_NAME: 'Byte Burner (Development)',
};
