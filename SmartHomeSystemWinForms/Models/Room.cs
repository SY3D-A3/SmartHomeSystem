using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents a room in the smart home with associated devices
    /// </summary>
    public class Room
    {
        public int RoomID { get; set; }
        public string Name { get; set; }
        private List<SmartDevice> _devices;
        private List<Sensor> _sensors;

        public Room(int roomID, string name)
        {
            RoomID = roomID;
            Name = name;
            _devices = new List<SmartDevice>();
            _sensors = new List<Sensor>();
        }

        /// <summary>
        /// Gets all devices in the room
        /// </summary>
        /// <returns>A read-only collection of devices</returns>
        public IReadOnlyCollection<SmartDevice> GetDevices()
        {
            return _devices.AsReadOnly();
        }

        /// <summary>
        /// Gets all sensors in the room
        /// </summary>
        /// <returns>A read-only collection of sensors</returns>
        public IReadOnlyCollection<Sensor> GetSensors()
        {
            return _sensors.AsReadOnly();
        }

        /// <summary>
        /// Adds a device to the room
        /// </summary>
        /// <param name="device">The device to add</param>
        public void AddDevice(SmartDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            // Update the device's location to match the room
            device.Location = Name;
            _devices.Add(device);
            Console.WriteLine($"Added {device.GetType().Name} (ID: {device.DeviceID}) to {Name}");
        }

        /// <summary>
        /// Adds a sensor to the room
        /// </summary>
        /// <param name="sensor">The sensor to add</param>
        public void AddSensor(Sensor sensor)
        {
            if (sensor == null)
            {
                throw new ArgumentNullException(nameof(sensor));
            }

            // Update the sensor's location to match the room
            sensor.Location = Name;
            _sensors.Add(sensor);
            Console.WriteLine($"Added {sensor.GetType().Name} (ID: {sensor.SensorID}) to {Name}");
        }

        /// <summary>
        /// Removes a device from the room
        /// </summary>
        /// <param name="deviceID">The ID of the device to remove</param>
        /// <returns>True if the device was removed, false if it wasn't found</returns>
        public bool RemoveDevice(int deviceID)
        {
            SmartDevice device = _devices.FirstOrDefault(d => d.DeviceID == deviceID);
            if (device != null)
            {
                _devices.Remove(device);
                Console.WriteLine($"Removed device {deviceID} from {Name}");
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Removes a sensor from the room
        /// </summary>
        /// <param name="sensorID">The ID of the sensor to remove</param>
        /// <returns>True if the sensor was removed, false if it wasn't found</returns>
        public bool RemoveSensor(int sensorID)
        {
            Sensor sensor = _sensors.FirstOrDefault(s => s.SensorID == sensorID);
            if (sensor != null)
            {
                _sensors.Remove(sensor);
                Console.WriteLine($"Removed sensor {sensorID} from {Name}");
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Gets a device by ID
        /// </summary>
        /// <param name="deviceID">The ID of the device to get</param>
        /// <returns>The device if found, null otherwise</returns>
        public SmartDevice GetDevice(int deviceID)
        {
            return _devices.FirstOrDefault(d => d.DeviceID == deviceID);
        }

        /// <summary>
        /// Gets a sensor by ID
        /// </summary>
        /// <param name="sensorID">The ID of the sensor to get</param>
        /// <returns>The sensor if found, null otherwise</returns>
        public Sensor GetSensor(int sensorID)
        {
            return _sensors.FirstOrDefault(s => s.SensorID == sensorID);
        }

        /// <summary>
        /// Gets all devices of a specific type
        /// </summary>
        /// <typeparam name="T">The type of device to get</typeparam>
        /// <returns>A collection of devices of the specified type</returns>
        public IEnumerable<T> GetDevicesOfType<T>() where T : SmartDevice
        {
            return _devices.OfType<T>();
        }

        /// <summary>
        /// Gets all sensors of a specific type
        /// </summary>
        /// <typeparam name="T">The type of sensor to get</typeparam>
        /// <returns>A collection of sensors of the specified type</returns>
        public IEnumerable<T> GetSensorsOfType<T>() where T : Sensor
        {
            return _sensors.OfType<T>();
        }

        /// <summary>
        /// Turns on all devices in the room
        /// </summary>
        public void TurnOnAllDevices()
        {
            foreach (var device in _devices)
            {
                device.TurnOn();
            }
            
            Console.WriteLine($"All devices in {Name} turned ON");
        }

        /// <summary>
        /// Turns off all devices in the room
        /// </summary>
        public void TurnOffAllDevices()
        {
            foreach (var device in _devices)
            {
                device.TurnOff();
            }
            
            Console.WriteLine($"All devices in {Name} turned OFF");
        }

        /// <summary>
        /// Activates all sensors in the room
        /// </summary>
        public void ActivateAllSensors()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Activate();
            }
            
            Console.WriteLine($"All sensors in {Name} activated");
        }

        /// <summary>
        /// Deactivates all sensors in the room
        /// </summary>
        public void DeactivateAllSensors()
        {
            foreach (var sensor in _sensors)
            {
                sensor.Deactivate();
            }
            
            Console.WriteLine($"All sensors in {Name} deactivated");
        }

        /// <summary>
        /// Returns a string representation of the room
        /// </summary>
        /// <returns>A string containing the room details</returns>
        public override string ToString()
        {
            return $"Room {RoomID}: {Name} - {_devices.Count} devices, {_sensors.Count} sensors";
        }
    }
}
