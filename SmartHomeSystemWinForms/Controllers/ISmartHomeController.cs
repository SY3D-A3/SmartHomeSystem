using System;
using System.Collections.Generic;
using SmartHomeSystemWinForms.Models;
using SmartHomeSystemWinForms.Events;

namespace SmartHomeSystemWinForms.Controllers
{
    /// <summary>
    /// Interface for the Smart Home Controller
    /// </summary>
    public interface ISmartHomeController
    {
        // Event handlers for UI updates
        event EventHandler<DeviceStatusChangedEventArgs> OnDeviceStatusChanged;
        event EventHandler<SensorReadingEventArgs> OnSensorReadingTaken;
        event EventHandler<ScheduleStatusChangedEventArgs> OnScheduleStatusChanged;
        event EventHandler<string> OnLogMessage;

        /// <summary>
        /// Initializes the controller with sample devices and sensors
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts the controller
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the controller
        /// </summary>
        void Stop();

        /// <summary>
        /// Processes a voice command
        /// </summary>
        /// <param name="commandString">The voice command string</param>
        /// <returns>The result of the command</returns>
        string ProcessVoiceCommand(string commandString);

        /// <summary>
        /// Gets all devices in the system
        /// </summary>
        /// <returns>A list of all devices</returns>
        List<SmartDevice> GetAllDevices();

        /// <summary>
        /// Gets all devices of a specific type
        /// </summary>
        /// <typeparam name="T">The type of device to get</typeparam>
        /// <returns>A list of devices of the specified type</returns>
        List<T> GetDevicesOfType<T>() where T : SmartDevice;

        /// <summary>
        /// Gets all sensors in the system
        /// </summary>
        /// <returns>A list of all sensors</returns>
        List<Sensor> GetAllSensors();

        /// <summary>
        /// Gets all sensors of a specific type
        /// </summary>
        /// <typeparam name="T">The type of sensor to get</typeparam>
        /// <returns>A list of sensors of the specified type</returns>
        List<T> GetSensorsOfType<T>() where T : Sensor;

        /// <summary>
        /// Gets all rooms in the system
        /// </summary>
        /// <returns>A list of all rooms</returns>
        List<Room> GetAllRooms();

        /// <summary>
        /// Gets all schedules in the system
        /// </summary>
        /// <returns>A list of all schedules</returns>
        List<Schedule> GetAllSchedules();

        /// <summary>
        /// Gets all active schedules
        /// </summary>
        /// <returns>A list of active schedules</returns>
        List<Schedule> GetActiveSchedules();

        /// <summary>
        /// Adds a new user to the system
        /// </summary>
        /// <param name="user">The user to add</param>
        void AddUser(User user);

        /// <summary>
        /// Adds a new room to the system
        /// </summary>
        /// <param name="room">The room to add</param>
        void AddRoom(Room room);

        /// <summary>
        /// Adds a new device to the system
        /// </summary>
        /// <param name="device">The device to add</param>
        /// <param name="roomID">The ID of the room to add the device to</param>
        /// <param name="deviceName">The name to use for the device in voice commands</param>
        void AddDevice(SmartDevice device, int roomID, string deviceName);

        /// <summary>
        /// Adds a new sensor to the system
        /// </summary>
        /// <param name="sensor">The sensor to add</param>
        /// <param name="roomID">The ID of the room to add the sensor to</param>
        void AddSensor(Sensor sensor, int roomID);

        /// <summary>
        /// Adds a new schedule to the system
        /// </summary>
        /// <param name="schedule">The schedule to add</param>
        void AddSchedule(Schedule schedule);
    }
}
