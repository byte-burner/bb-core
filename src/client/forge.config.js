module.exports = {
  packagerConfig: {
    asar: {
      unpackDir: ".*/**/Release", // selects a folder in .webpack output folder to be included in the unpacked asar folder
                                  // (i.e. app.asar.unpacked). This is so we can find the the spawn-helper binary we need
                                  // for the node-pty external package
    },
    extraResource: [
      "./src/Resources/net_iot_api", // dotnet api
      "./src/Resources/net_iot_util", // dotnet util
      "./src/Resources/scripts", // linux platform scripts
    ],
    icon: './src/Client/Assets/icon' // no file extension required
  },
  rebuildConfig: {},
  makers: [
    {
      name: '@electron-forge/maker-squirrel', // windows
      config: {
        // The ICO file to use as the icon for the generated Setup.exe
        setupIcon: './src/Client/Assets/icon.ico'
      },
    },
    {
      name: '@electron-forge/maker-zip', // osx
      platforms: ['darwin'],
    },
    {
      name: '@electron-forge/maker-deb', // linux - debian
      config: {
        options: {
          icon: './src/Client/Assets/icon.png'
        }
      },
    }
  ],
  plugins: [
    {
      name: '@electron-forge/plugin-auto-unpack-natives',
      config: {},
    },
    {
      name: '@electron-forge/plugin-webpack',
      config: {
        mainConfig: './webpack.main.config.js',
        devContentSecurityPolicy: "connect-src 'self' http://localhost:5100 ws://localhost:5100 'unsafe-eval'",
        renderer: {
          config: './webpack.renderer.config.js',
          entryPoints: [
            {
              html: './src/Electron/Renderer/index.html',
              js: './src/Electron/Renderer/renderer.js',
              name: 'main_window',
              preload: {
                js: './src/Electron/Renderer/preload.js',
              },
            },
          ],
        },
      },
    },
  ],
};
