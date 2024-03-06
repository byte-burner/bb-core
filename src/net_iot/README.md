Device Datasheets:
- AT89S52: https://ww1.microchip.com/downloads/en/DeviceDoc/doc1919.pdf

Bridge Datasheets:
- UM232H: https://ftdichip.com/wp-content/uploads/2020/07/DS_UM232H.pdf
- FT232H: https://ftdichip.com/wp-content/uploads/2023/09/DS_FT232H.pdf


Roadmap Ideas:
- Use piping to catch the messages communicated by the gui process
- stick piped messages in a queue and process them efficiently and in the right order
- maybe we could use some sort of bus like MassTransit or NServiceBus
    - the renderer application sends data to the main application in electron then the main application sends the message to the bus
    where it's picked up and processed in c#
    - maybe it's best to just use plain ole rabbit mq since there isn't an implementation for MassTransit or NServiceBus in nodejs
- If things became really complicated, I could also use a REST API in C# for the communication.
    - could use a local sqlite db or redis cache as a data store
- Add a wireless bridge so that we don't have to connect to the PC to update firmware!

Projects:
- net_iot_api (allows inter process communication from our electron gui through an HTTP REST API)
- net_iot_core (core library/services that interact with the hardware and our client side data store)
- net_iot_tests (system tests for ALL projects)
- net_iot_util (command line tool program)