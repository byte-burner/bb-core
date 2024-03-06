// eslint-disable-next-line import/no-extraneous-dependencies
const { BrowserWindow } = require('electron');

/**
 * Initialize the main window of the application.
 * Creates a browser window with specified dimensions and settings,
 * loads the application's index.html file, and optionally opens DevTools in development mode.
 */
export const initMainWindow = async () => {
  // Create the browser window.
  const mainWindow = new BrowserWindow({
    width: 800,
    height: 480,
    title: process.env.APP_NAME,
    titleBarStyle: 'hiddenInset',
    titleBarOverlay: true,
    autoHideMenuBar: true,
    webPreferences: {
      // eslint-disable-next-line no-undef
      preload: MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY,
    },
    resizable: false,
    icon: process.env.ICON_PATH,
  });

  // and load the index.html of the app.
  // eslint-disable-next-line no-undef
  mainWindow.loadURL(MAIN_WINDOW_WEBPACK_ENTRY);

  // Open the DevTools.
  if (process.env.NODE_ENV === 'development') {
    mainWindow.webContents.openDevTools();
  }
};
