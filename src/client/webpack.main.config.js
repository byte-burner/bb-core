const CopyWebpackPlugin = require('copy-webpack-plugin');
const PermissionsOutputPlugin = require('webpack-permissions-plugin');

module.exports = {
  /**
   * This is the main entry point for your application, it's the first file
   * that runs in the main process.
   */
  entry: './src/Electron/Main/main.js',
  // Put your normal webpack config below here
  module: {
    rules: require('./webpack.rules'),
  },

  /**
   * Configures node-pty as an external package, copying the required binaries to be used in the main electron process.
   * The config below copies dependencies/binaries to the correct locations, ensuring their permissions are preserved
   */
  plugins: [
    new CopyWebpackPlugin({
      patterns: [
        {
          from: './node_modules/node-pty',
          to: './node_modules/node-pty',
        }
      ],
    }),
    new PermissionsOutputPlugin({
      buildFolders: [
        {
          path: './.webpack/main/node_modules/node-pty/build/Release', // Everything under Release/ gets these modes
          fileMode: '755',
          dirMode: '755'
        },
      ],
    })
  ],
  externals: {
    'node-pty': 'commonjs node-pty',
  },
};
