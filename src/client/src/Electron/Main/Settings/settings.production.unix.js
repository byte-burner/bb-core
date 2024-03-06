const path = require('path');

export const settings = {
  API_EXEC_PATH: path.join(process.resourcesPath, 'net_iot_api/net_iot_api'),
  UTIL_EXEC_NAME: 'net_iot_util',
  UDEV_FT232H_FILE: '/etc/udev/rules.d/99-FT232H.rules',
};
