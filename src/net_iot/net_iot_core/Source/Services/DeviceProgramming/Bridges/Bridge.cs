using System.Runtime.InteropServices;
using net_iot_core.Services.DeviceProgramming.Constants;
using net_iot_core.Services.DeviceProgramming.Exceptions;
using net_iot_core.Services.DeviceProgramming.Models;
using Usb.Events;

namespace net_iot_core.Services.DeviceProgramming.Bridges
{
    /// <summary>
    /// Provides methods for managing communication of bridges with devices.
    /// </summary>
    public static class Bridge
    {
        private static readonly object _bridgeLock = new object();

        private static List<BridgeInfo> _connectedBridges = new List<BridgeInfo>();

        private static IUsbEventWatcher _usbEventWatcher = new UsbEventWatcher(
            startImmediately: false);

        private static List<BridgeInfo> _supportedBridges = new List<BridgeInfo>()
        {
            new BridgeInfo() { Type = "UM232H-FTD", ProductID = "6014", VendorID = "0403", DeviceID = 67330068 }, // FTDI - UM232H

            // add more supported bridge devices here
        };

        public static List<BridgeInfo> SupportedBridges { get { return _supportedBridges; } }

        /// <summary>
        /// Gets the bridge info for each connected bridge
        /// </summary>
        /// <returns>An instance of <see cref="BridgeInfo"/></returns>
        public static IEnumerable<BridgeInfo> GetAllConnectedBridges()
        {
            var allBridgeInfo = Enumerable.Empty<BridgeInfo>().ToList();

            var bridgeUM232HInfo = UM232H.GetAllConnectedBridges();
            allBridgeInfo.AddRange(bridgeUM232HInfo);

            // can add more bridges here

            return allBridgeInfo;
        }

        //
        // Summary:
        //  Opens the first connected bridge
        //
        // Parameters:
        //   millisecondsTimeout:
        //     The number of milliseconds for which the thread is suspended. If the value of
        //     the millisecondsTimeout argument is zero, the thread relinquishes the remainder
        //     of its time slice to any thread of equal priority that is ready to run. If there
        //     are no other threads of equal priority that are ready to run, execution of the
        //     current thread is not suspended.
        //
        // Returns:
        //  An instance of IBridge
        public static IBridge Open()
        {
            if (UM232H.IsBridgeConnected())
            {
                var bridgeUM232H = new UM232H();
                bridgeUM232H.Open();
                return bridgeUM232H;
            }

            // can add more bridges here

            throw new NotFoundException("The device was not found");
        }

        /// <summary>
        /// Opens the bridge with the specified type and serial number
        /// </summary>
        /// <param name="type">The type of bridge</param>
        /// <param name="serialNumber">The serial number of the bridge</param>
        /// <returns>An instance of <see cref="IBridge"/></returns>
        public static IBridge Open(string type, string serialNumber)
        {
            switch (type)
            {
                case SupportedBridgeType.UM232H_FTD:
                    var bridgeUM232H = new UM232H();
                    bridgeUM232H.Open(serialNumber);
                    return bridgeUM232H;
                
                // can add more bridges here
            }

            throw new NotFoundException("The device was not found");
        }

        /// <summary>
        /// Registers USB events so we can receive the connected/disconnected bridge info
        /// </summary>
        /// <param name="onBridgeEvent">A callback function to return the connected devices when a usb event arises</param>
        public static void StartMonitoringBridgeEvents(Action<IEnumerable<BridgeInfo>> onBridgeEvent)
        {
            lock (_bridgeLock)
            {
                // resets/disposes the events on the watcher before starting it again
                _usbEventWatcher.Dispose();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // starts the usb event watcher if it's not already started
                    _usbEventWatcher.Start();

                    RegisterEventsForLinux(_usbEventWatcher, onBridgeEvent);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    // starts the usb event watcher if it's not already started
                    _usbEventWatcher.Start();

                    RegisterEventsForOSX(_usbEventWatcher, onBridgeEvent);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // starts the usb event watcher if it's not already started
                    // usePnPEntity: true; detects small component devices on windows

                    _usbEventWatcher.Start(usePnPEntity: true); 

                    RegisterEventsForWindows(_usbEventWatcher, onBridgeEvent);
                }
            }
        }

        /// <summary>
        /// Registers USB events for macOS to receive notifications when a USB device is added or removed.
        /// </summary>
        /// <param name="usbEventWatcher">The USB event watcher instance.</param>
        /// <param name="onBridgeEvent">A callback function to handle bridge events.</param>
        private static void RegisterEventsForOSX(IUsbEventWatcher usbEventWatcher, Action<IEnumerable<BridgeInfo>> onBridgeEvent)
        {
            usbEventWatcher.UsbDeviceAdded += (_, device) => 
            {
                // on OSX the vid and pid come back to us in decimal string format, which is why we are passing the flag
                // in the ToDeviceId method below. We also want to convert vid and pid to their hex representation when 
                // storing them in the connected device list

                long devId = ToDeviceID(device.VendorID, device.ProductID, true);
                bool isASupportedDeviceType = _supportedBridges.Any((b) => b.DeviceID == devId);

                if (isASupportedDeviceType)
                {
                    bool isThisDeviceAlreadyConnected = _connectedBridges.Any((b) => b.SerialNbr == device.SerialNumber && b.DeviceID == devId);

                    if (!isThisDeviceAlreadyConnected)
                    {
                        _connectedBridges.Add(new BridgeInfo()
                        {
                            Type = GetDeviceTypeById(devId),
                            SerialNbr = device.SerialNumber,
                            ProductID = device.ProductID.ToHexStr(),
                            VendorID = device.VendorID.ToHexStr(),
                            DeviceID = devId,
                            DevicePath = device.DeviceSystemPath
                        });

                        onBridgeEvent(_connectedBridges);
                    }
                }
            };

            usbEventWatcher.UsbDeviceRemoved += (_, device) => 
            {
                long devId = ToDeviceID(device.VendorID, device.ProductID, true);
                var bridgeToDisconnect = _connectedBridges.FirstOrDefault((b) => b.SerialNbr == device.SerialNumber && b.DeviceID == devId);

                if (bridgeToDisconnect != null)
                {
                    _connectedBridges.Remove(bridgeToDisconnect);

                    onBridgeEvent(_connectedBridges);
                }
            };

            onBridgeEvent(_connectedBridges);
        }

        /// <summary>
        /// Registers USB events for Linux to receive notifications when a USB device is added or removed.
        /// </summary>
        /// <param name="usbEventWatcher">The USB event watcher instance.</param>
        /// <param name="onBridgeEvent">A callback function to handle bridge events.</param>
        private static void RegisterEventsForLinux(IUsbEventWatcher usbEventWatcher, Action<IEnumerable<BridgeInfo>> onBridgeEvent)
        {
            usbEventWatcher.UsbDeviceAdded += (_, device) => 
            {
                long devId = ToDeviceID(device.VendorID, device.ProductID);
                bool isASupportedDeviceType = _supportedBridges.Any((b) => b.DeviceID == devId);

                if (isASupportedDeviceType)
                {
                    bool isThisDeviceAlreadyConnected = _connectedBridges.Any((b) => b.SerialNbr == device.SerialNumber && b.DeviceID == devId);

                    if (!isThisDeviceAlreadyConnected)
                    {
                        _connectedBridges.Add(new BridgeInfo()
                        {
                            Type = GetDeviceTypeById(devId),
                            SerialNbr = device.SerialNumber,
                            ProductID = device.ProductID,
                            VendorID = device.VendorID,
                            DeviceID = devId,
                            DevicePath = device.DeviceSystemPath
                        });


                        onBridgeEvent(_connectedBridges);
                    }
                }
            };

            usbEventWatcher.UsbDeviceRemoved += (_, device) => 
            {
                // on linux each device will have a different device path. None of the other fields (i.e. vid & pid) can be used because they 
                // will be empty on removal...
                var bridgeToDisconnect = _connectedBridges.FirstOrDefault((b) => b.DevicePath == device.DeviceSystemPath);

                if (bridgeToDisconnect != null)
                {
                    _connectedBridges.Remove(bridgeToDisconnect);

                    onBridgeEvent(_connectedBridges);
                }
            };

            onBridgeEvent(_connectedBridges);
        }

        /// <summary>
        /// Registers USB events for Windows to receive notifications when a USB device is added or removed.
        /// </summary>
        /// <param name="usbEventWatcher">The USB event watcher instance.</param>
        /// <param name="onBridgeEvent">A callback function to handle bridge events.</param>
        private static void RegisterEventsForWindows(IUsbEventWatcher usbEventWatcher, Action<IEnumerable<BridgeInfo>> onBridgeEvent)
        {
            usbEventWatcher.UsbDeviceAdded += (_, device) => 
            {
                long devId = ToDeviceID(device.VendorID, device.ProductID);
                bool isASupportedDeviceType = _supportedBridges.Any((b) => b.DeviceID == devId);

                if (isASupportedDeviceType)
                {
                    bool isThisDeviceAlreadyConnected = _connectedBridges.Any((b) => b.SerialNbr == device.SerialNumber && b.DeviceID == devId);

                    if (!isThisDeviceAlreadyConnected)
                    {
                        _connectedBridges.Add(new BridgeInfo()
                        {
                            Type = GetDeviceTypeById(devId),
                            SerialNbr = device.SerialNumber,
                            ProductID = device.ProductID,
                            VendorID = device.VendorID,
                            DeviceID = devId,
                            DevicePath = device.DeviceSystemPath
                        });

                        onBridgeEvent(_connectedBridges);
                    }
                }
            };

            usbEventWatcher.UsbDeviceRemoved += (_, device) => 
            {
                long devId = ToDeviceID(device.VendorID, device.ProductID);
                var bridgeToDisconnect = _connectedBridges.FirstOrDefault((b) => b.SerialNbr == device.SerialNumber && b.DeviceID == devId);

                if (bridgeToDisconnect != null)
                {
                    _connectedBridges.Remove(bridgeToDisconnect);

                    onBridgeEvent(_connectedBridges);
                }
            };

            onBridgeEvent(_connectedBridges);
        }

        #region Private Helper Methods

        private static string ToHexStr(this string str) => int.Parse(str).ToString("X").PadLeft(4, '0');

        private static string? GetDeviceTypeById(long id) => _supportedBridges.FirstOrDefault(b => b.DeviceID == id)?.Type;

        /// <summary>
        /// Converts vendor and product IDs (VID and PID) to a device ID that uniquely IDs the device.
        /// Concatenates VID + PID into a string and then converts it to decimal
        /// </summary>
        /// <param name="vid">The vendor ID.</param>
        /// <param name="pid">The product ID.</param>
        /// <param name="decimalFormat">Flag indicating whether the input VID and PID are in decimal format. Default is false, indicating hexadecimal format.</param>
        /// <returns>The device ID as a long integer.</returns>
        private static long ToDeviceID(string vid, string pid, bool decimalFormat = false)
        {
            // if vid and pid are in decimal format, convert them to hex
            if (decimalFormat)
            {
                pid = Convert.ToString(int.Parse(pid), 16);
                vid = Convert.ToString(int.Parse(vid), 16);
            }

            return Convert.ToInt64(vid + pid, 16);
        }

        #endregion Private Helper Methods
    }
}


