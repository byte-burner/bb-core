const {
  createLogger,
  format,
  transports,
} = require('winston');

const {
  combine,
  timestamp,
  colorize,
  simple,
} = format;

const {
  Console,
} = transports;

export const logger = createLogger({
  exitOnError: true,
  defaultMeta: { service: 'global-logger' },
  transports: [
    new Console({
      level: 'debug',
      format: combine(colorize(), timestamp(), simple()),
    }),
  ],
});

// add more loggers here or in seperate files and import them wherever you want
// i.e. you could have a logger for each service or a group of service
// for now we are using a global logger“
