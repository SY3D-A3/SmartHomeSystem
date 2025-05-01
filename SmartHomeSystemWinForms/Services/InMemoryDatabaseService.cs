using System;
using System.Collections.Generic;
using System.Linq;
using SmartHomeSystemWinForms.Models;

namespace SmartHomeSystemWinForms.Services
{
    /// <summary>
    /// In-memory database service for the smart home system
    /// </summary>
    public class InMemoryDatabaseService : IDatabaseService
    {
        private List<User> _users;
        private List<Room> _rooms;
        private Dictionary<int, SmartDevice> _devices;
        private Dictionary<int, Sensor> _sensors;
        private List<Schedule> _schedules;
        private Dictionary<int, List<SensorReading>> _sensorReadings;

        public InMemoryDatabaseService()
        {
            _users = new List<User>();
            _rooms = new List<Room>();
            _devices = new Dictionary<int, SmartDevice>();
            _sensors = new Dictionary<int, Sensor>();
            _schedules = new List<Schedule>();
            _sensorReadings = new Dictionary<int, List<SensorReading>>();
            
            InitializeDatabase();
        }

        /// <summary>
        /// Initializes the database with some sample data
        /// </summary>
        private void InitializeDatabase()
        {
            Console.WriteLine("Initializing in-memory database...");
            
            // Add some sample users
            _users.Add(new User(1, "John Doe", "Admin"));
            _users.Add(new User(2, "Jane Smith", "Regular"));
            _users.Add(new User(3, "Guest User", "Guest"));
            
            // Add some sample rooms
            _rooms.Add(new Room(1, "Living Room"));
            _rooms.Add(new Room(2, "Kitchen"));
            _rooms.Add(new Room(3, "Bedroom"));
            _rooms.Add(new Room(4, "Bathroom"));
            
            Console.WriteLine("In-memory database initialized successfully.");
        }

        #region User Methods

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">The user to add</param>
        public void AddUser(User user)
        {
            if (_users.Any(u => u.UserID == user.UserID))
            {
                throw new InvalidOperationException($"User with ID {user.UserID} already exists");
            }
            
            _users.Add(user);
            Console.WriteLine($"User {user.UserID} added to database");
        }

        /// <summary>
        /// Gets a user from the database by ID
        /// </summary>
        /// <param name="userID">The ID of the user to get</param>
        /// <returns>The user if found, null otherwise</returns>
        public User GetUser(int userID)
        {
            return _users.FirstOrDefault(u => u.UserID == userID);
        }

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all users</returns>
        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }

        #endregion

        #region Room Methods

        /// <summary>
        /// Adds a room to the database
        /// </summary>
        /// <param name="room">The room to add</param>
        public void AddRoom(Room room)
        {
            if (_rooms.Any(r => r.RoomID == room.RoomID))
            {
                throw new InvalidOperationException($"Room with ID {room.RoomID} already exists");
            }
            
            _rooms.Add(room);
            Console.WriteLine($"Room {room.RoomID} added to database");
        }

        /// <summary>
        /// Gets a room from the database by ID
        /// </summary>
        /// <param name="roomID">The ID of the room to get</param>
        /// <returns>The room if found, null otherwise</returns>
        public Room GetRoom(int roomID)
        {
            return _rooms.FirstOrDefault(r => r.RoomID == roomID);
        }

        /// <summary>
        /// Gets all rooms from the database
        /// </summary>
        /// <returns>A list of all rooms</returns>
        public List<Room> GetAllRooms()
        {
            return _rooms.ToList();
        }

        #endregion

        #region Device Methods

        /// <summary>
        /// Adds a device to the database
        /// </summary>
        /// <param name="device">The device to add</param>
        /// <param name="roomID">The ID of the room the device is in</param>
        public void AddDevice(SmartDevice device, int roomID)
        {
            if (_devices.ContainsKey(device.DeviceID))
            {
                throw new InvalidOperationException($"Device with ID {device.DeviceID} already exists");
            }
            
            Room room = GetRoom(roomID);
            if (room == null)
            {
                throw new InvalidOperationException($"Room with ID {roomID} does not exist");
            }
            
            _devices.Add(device.DeviceID, device);
            room.AddDevice(device);
            Console.WriteLine($"Device {device.DeviceID} added to database in room {roomID}");
        }

        /// <summary>
        /// Gets a device from the database by ID
        /// </summary>
        /// <param name="deviceID">The ID of the device to get</param>
        /// <returns>The device if found, null otherwise</returns>
        public SmartDevice GetDevice(int deviceID)
        {
            return _devices.TryGetValue(deviceID, out SmartDevice device) ? device : null;
        }

        /// <summary>
        /// Gets all devices from the database
        /// </summary>
        /// <returns>A list of all devices</returns>
        public List<SmartDevice> GetAllDevices()
        {
            return _devices.Values.ToList();
        }

        /// <summary>
        /// Gets all devices of a specific type
        /// </summary>
        /// <typeparam name="T">The type of device to get</typeparam>
        /// <returns>A list of devices of the specified type</returns>
        public List<T> GetDevicesOfType<T>() where T : SmartDevice
        {
            return _devices.Values.OfType<T>().ToList();
        }

        /// <summary>
        /// Updates a device's status in the database
        /// </summary>
        /// <param name="deviceID">The ID of the device to update</param>
        /// <param name="status">The new status</param>
        public void UpdateDeviceStatus(int deviceID, bool status)
        {
            if (!_devices.TryGetValue(deviceID, out SmartDevice device))
            {
                throw new InvalidOperationException($"Device with ID {deviceID} does not exist");
            }
            
            if (status)
            {
                device.TurnOn();
            }
            else
            {
                device.TurnOff();
            }
            
            Console.WriteLine($"Device {deviceID} status updated to {(status ? "ON" : "OFF")}");
        }

        #endregion

        #region Sensor Methods

        /// <summary>
        /// Adds a sensor to the database
        /// </summary>
        /// <param name="sensor">The sensor to add</param>
        /// <param name="roomID">The ID of the room the sensor is in</param>
        public void AddSensor(Sensor sensor, int roomID)
        {
            if (_sensors.ContainsKey(sensor.SensorID))
            {
                throw new InvalidOperationException($"Sensor with ID {sensor.SensorID} already exists");
            }
            
            Room room = GetRoom(roomID);
            if (room == null)
            {
                throw new InvalidOperationException($"Room with ID {roomID} does not exist");
            }
            
            _sensors.Add(sensor.SensorID, sensor);
            room.AddSensor(sensor);
            _sensorReadings.Add(sensor.SensorID, new List<SensorReading>());
            Console.WriteLine($"Sensor {sensor.SensorID} added to database in room {roomID}");
        }

        /// <summary>
        /// Gets a sensor from the database by ID
        /// </summary>
        /// <param name="sensorID">The ID of the sensor to get</param>
        /// <returns>The sensor if found, null otherwise</returns>
        public Sensor GetSensor(int sensorID)
        {
            return _sensors.TryGetValue(sensorID, out Sensor sensor) ? sensor : null;
        }

        /// <summary>
        /// Gets all sensors from the database
        /// </summary>
        /// <returns>A list of all sensors</returns>
        public List<Sensor> GetAllSensors()
        {
            return _sensors.Values.ToList();
        }

        /// <summary>
        /// Gets all sensors of a specific type
        /// </summary>
        /// <typeparam name="T">The type of sensor to get</typeparam>
        /// <returns>A list of sensors of the specified type</returns>
        public List<T> GetSensorsOfType<T>() where T : Sensor
        {
            return _sensors.Values.OfType<T>().ToList();
        }

        /// <summary>
        /// Records a sensor reading in the database
        /// </summary>
        /// <param name="sensorID">The ID of the sensor</param>
        /// <param name="readingValue">The reading value</param>
        public void RecordSensorReading(int sensorID, string readingValue)
        {
            if (!_sensors.TryGetValue(sensorID, out Sensor sensor))
            {
                throw new InvalidOperationException($"Sensor with ID {sensorID} does not exist");
            }
            
            if (!_sensorReadings.TryGetValue(sensorID, out List<SensorReading> readings))
            {
                readings = new List<SensorReading>();
                _sensorReadings.Add(sensorID, readings);
            }
            
            SensorReading reading = new SensorReading
            {
                SensorID = sensorID,
                ReadingValue = readingValue,
                ReadingTime = DateTime.Now
            };
            
            readings.Add(reading);
            Console.WriteLine($"Sensor {sensorID} reading recorded: {readingValue}");
        }

        /// <summary>
        /// Gets all readings for a sensor
        /// </summary>
        /// <param name="sensorID">The ID of the sensor</param>
        /// <returns>A list of readings for the sensor</returns>
        public List<SensorReading> GetSensorReadings(int sensorID)
        {
            return _sensorReadings.TryGetValue(sensorID, out List<SensorReading> readings) ? readings : new List<SensorReading>();
        }

        #endregion

        #region Schedule Methods

        /// <summary>
        /// Adds a schedule to the database
        /// </summary>
        /// <param name="schedule">The schedule to add</param>
        public void AddSchedule(Schedule schedule)
        {
            if (_schedules.Any(s => s.ScheduleID == schedule.ScheduleID))
            {
                throw new InvalidOperationException($"Schedule with ID {schedule.ScheduleID} already exists");
            }
            
            if (!_devices.ContainsKey(schedule.DeviceID))
            {
                throw new InvalidOperationException($"Device with ID {schedule.DeviceID} does not exist");
            }
            
            _schedules.Add(schedule);
            Console.WriteLine($"Schedule {schedule.ScheduleID} added to database");
        }

        /// <summary>
        /// Gets a schedule from the database by ID
        /// </summary>
        /// <param name="scheduleID">The ID of the schedule to get</param>
        /// <returns>The schedule if found, null otherwise</returns>
        public Schedule GetSchedule(int scheduleID)
        {
            return _schedules.FirstOrDefault(s => s.ScheduleID == scheduleID);
        }

        /// <summary>
        /// Gets all schedules from the database
        /// </summary>
        /// <returns>A list of all schedules</returns>
        public List<Schedule> GetAllSchedules()
        {
            return _schedules.ToList();
        }

        /// <summary>
        /// Gets all active schedules for the current time
        /// </summary>
        /// <returns>A list of active schedules</returns>
        public List<Schedule> GetActiveSchedules()
        {
            DateTime now = DateTime.Now;
            return _schedules.Where(s => s.IsActiveAt(now)).ToList();
        }

        #endregion
    }

    /// <summary>
    /// Represents a sensor reading
    /// </summary>
    public class SensorReading
    {
        public int SensorID { get; set; }
        public string ReadingValue { get; set; }
        public DateTime ReadingTime { get; set; }
    }
}
