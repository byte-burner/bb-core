const { initCommandHandlers } = require('./commandController');
const { initTerminalHandlers } = require('./terminalController');
const { initTestHandlers } = require('./testController');

/**
 * Initializes all handlers established for inter-process communication between
 * the renderer and main electron processes
 */
export const initAllHandlers = () => {
  // more controllers here
  initCommandHandlers();
  initTerminalHandlers();
  initTestHandlers();
};
