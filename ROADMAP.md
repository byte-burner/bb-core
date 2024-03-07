## ROADMAP
- net_iot_core: Make sure PR: https://github.com/dotnet/iot/pull/2270 goes through and release is updated for iot library so I only call dispose once in Dispose method in UM2323H in net_iot_core project
- research spike: Cannot use reflection if we plan on trimming our published executables! MVC Asp.net doesn't work with trimming...
- tech debt: Get proxy middleware working so I don’t have to remove CSP (content security policy) and enable CORS on backend…
- tech debt: Add an install build tools script that checks to see if the necassary build tools and versions are installed before we run our build scripts (python 3.9.6, nodejs 20.10.0, dotnet 8.0.100 are the required build tools). If they aren't intalled then don't try building and inform the user that they should be installed on their system before beginning...
- tech debt: How to force versions of each build tool to be installed - like in dotnet iot library. Do the same for nodejs in client folder
- tech debt: convert all word doc TRDs to follow the template
- client: give the ability to add multiple providers to each page in AppRoutes.js
- client: create bridge info section in settings page that gets bridge and device info on button click
- client: add ipc api layer
- client: add css/style layer- add css and style layer to front end client folder
- client: Provide better UI/UX to user on terminal page by saving terminal pid list in local storage when leaving terminal page. Then when entering terminal page check the cache (local storage) and restore all terminals.
- client: Add a help page. It should just link to the github page for our app
- net_iot_api: update to publish as AOT to decrease binary size
- net_iot_util: update to publish as AOT to decrease binary size
- net_iot_util: update colors for err and success to be red and green respectively for terminal output in prompt manager
- net_iot_util: make validation for two arguments to depend on one another
- net_iot_util: to publish the net_iot_util as trimmed we must not use reflection in order to achieve this

## Future Programmable Devices

#### Fringe
- AT89S51
- 6502
- Z80
- Motorola 68000 Series

#### Mainstream
- STM32
- ESP8266
- ESP32
- RP2040 (Raspberry Pi Pico)
- ATmega328
- PIC16F877A
- Attiny85


## Future Bridge Devices
- FT232H
- FT232RL
- ESP32 (Wireless option)
- RP2040 (Raspberry Pi Pico) (Wireless option)