Resources:
https://learn.microsoft.com/en-us/dotnet/standard/commandline/
https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage


BUSINESS RULES BELOW:

---------------------------------------------------------------------------------------------------------------
RULES FOR 'net_iot_util':
---------------------------------------------------------------------------------------------------------------
Usage: net_iot_util [options] [arguments]

Prints information about devices and bridges

Options:
- l|--list          Optional. List bridges connected to machine
- s|--supported     Optional. List of supported devices to flash

Usage: net_iot_util [command] [options] [arguments]

Executes a flash sub command

Commands:
flash               Flashes a hex file onto a device from a bridge device

Run 'net_iot_util [command] --help' for more information on a command.


---------------------------------------------------------------------------------------------------------------
RULES FOR 'net_iot_util flash':
---------------------------------------------------------------------------------------------------------------
Usage: net_iot_util flash [options] [arguments]

Flashes a hex file onto a device from a bridge device

options:
- d|--dev           Required. Specifies device to flash
- b|--bridge        Required. Specifies bridge to use for flashing
- h|--hex           Required. Specifies a hex file as the main program to be flashed
- e|--erase         Optional. Perform chip erase before flashing
- c|--conf          Optional. Specifies a configuration file to be used. Only specify config file



RAODMAP:
- The System.CommandLine library by .net is supposed to allow us to define all of our Commands, Arguments, Options, etc. seperately. So maybe I can write a conf.json file and wire them up in a for loop instead of defining a huge recursive .net object like I am doing now
- How to build a proper error manager for a console application in c# .net
- should I be passing params from my sub commands straigh to a program without doing much logic in the handler?
