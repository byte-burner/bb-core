## Getting Started

### How does the app work and what are its main components?

The application consists of three main parts:

1. Electron GUI: The user interface for the application, providing an intuitive way to interact with the core functionalities. Located in src/client.

2. Core Library (net_iot_core) and .NET REST API (net_iot_api): The Electron app communicates with the core library via a .NET REST API. This API facilitates seamless interaction between the GUI and underlying functionalities. Located in src/net_iot/net_iot_core.

3. Command-Line Utility (net_iot_util): Additionally, the application includes a simple command-line utility accessible via the embedded terminal within the Electron GUI. Users can perform various tasks using this utility, such as flashing their device by typing 'net_iot_util flash'. Located in src/net_iot/net_iot_util.

This structure provides users with flexibility in choosing their preferred method of interaction, whether through the graphical interface or command-line utility.

### What tools do I need installed for development?
  - VSCode or VS (IDE)
  - Nodejs - 20.11.1
  - Python - 3.12.2
    - run 'pip3 install setuptools' (must install 'setuptools' for native node modules like node-pty to work) 
  - dotnet SDK - 8.0.100
  - Helpful VSCode Extensions
    - C# Dev Kit
    - EditorConfig for VS Code
    - ESLint
    - GitLens
    - Vim (Optional - But good to learn it!)

### Building the app
 1. cd src/scripts
 2. Run 'bash unix-build.sh' for *nix OR './windows-build.ps1' for win32

### Starting the electron app
 1. cd src/client
 2. Hit F5 if using VSCode or VS OR run 'npm start'

### Starting the local rest api
 1. cd src/net_iot/net_iot_api
 2. node generateAppSettings.js (generates necassary appsettings)
 3. Hit F5 if using VSCode or VS OR run 'dotnet run' in terminal

### Publishing the command line tool so it can be used by electron
 1. cd src/net_iot/net_iot_util
 2. node generateAppSettings.js (generates necassary appsettings)
 3. dotnet publish
 4. cp ./src/net_iot/net_iot_util/bin/Debug/net8.0/publish/* ../../client/src/Resources/net_iot_util


## How can I add a device binding?

To add a device binding for a new programmable device start in src/net_iot/net_iot_core/Source/Services/DeviceProgramming/Devices.

Add a new binding, for example, AT89S51 that implements the IDevice interface. Use previous device bindings as a refernce point.

Add a test class in src/net_iot/net_iot_tests/Source/net_iot_core/DeviceProgramming/Devices. Write a test for each method and make sure the device can be correctly programmed using a simple blinky program as a test.

## How can I add a bridge device binding? 

To add a bridge device binding for a new programmable device start in src/net_iot/net_iot_core/Source/Services/DeviceProgramming/Bridges.

Add a new binding, for example, FT232RL that implements the IBridge interface. Use previous bridge device bindings as a refernce point.

Add a test class in src/net_iot/net_iot_tests/Source/net_iot_core/DeviceProgramming/Bridges. Write a test for each method and make sure the bridge device can be correctly written to and read from using a simple blinky program as a test.

## How can I add a minimal circuitry page? 

Roadmap Issue - minimal circuitry pages coming soon

## Coding Standards
#### Code comment standards for c# code
- For now, code comments will only be necassary in the core library located at src/net_iot/net_iot_core
- Code comments should be added to all class definitions, public methods, and constructors, and only private methods that are complicated enough to warrant an explanation.
- Code comments should be detailed, concise, and clear.
- Prefer brevity over verbosity
- Utilize the 'remarks' tag to explaian overtly complicated logic if necassary
- Use XML code comments only

#### Code comment standards for nodejs code
- For now, code comments will only be necassary in the Main folder located at src/client/src/Electron/Main
- Code comments should be added to all public functions and only private methods that are complicated enough to warrant an explanation.
- Code comments should be detailed, concise, and clear.
- Prefer brevity over verbosity
- Use JSDoc comments only

Remember, if you aren't a great commenter there's always chatGPT, and for especially complicated methods tell chat GPT to include a remarks section in the code comment explaining the codes logic!

## Adding Documentation

Roadmap Issue

## Tutorials

Roadmap Issue - YouTube tutorial coming soon