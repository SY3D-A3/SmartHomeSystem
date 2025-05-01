using System;
using System.Collections.Generic;
using System.Linq;
using SmartHomeSystemWinForms.Models;
using SmartHomeSystemWinForms.Services;

namespace SmartHomeSystem.Tests.Mocks
{
    /// <summary>
    /// Mock database service for testing
    /// </summary>
    public class MockDatabaseService : IDatabaseService
    {
        private List<User> _users = new List<User>();
        private List<Room> _rooms = new List<Room>();
        private Dictionary<int, SmartDevice> _devices = new Dictionary<int, SmartDevice>();
        private Dictionary<int, Sensor> _sensors = new Dictionary<int, Sensor>();
        private List<Schedule> _schedules = new List<Schedule>();
        private Dictionary<int, List<SensorReading>> _sensorReadings = new Dictionary<int, List<SensorReading>>();

        public MockDatabaseService()
        {
            // Initialize with test data
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // Add test rooms
            _rooms.Add(new Room(1, "Test Room 1"));
            _rooms.Add(new Room(2, "Test Room 2"));
        }

        #region User Methods
        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public User GetUser(int userID)
        {
            return _users.FirstOrDefault(u => u.UserID == userID);
        }

        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }
        #endregion

        #region Room Methods
        public void AddRoom(Room room)
        {
            _rooms.Add(room);
        }

        public Room GetRoom(int roomID)
        {
            return _rooms.FirstOrDefault(r => r.RoomID == roomID);
        }

        public List<Room> GetAllRooms()
        {
            return _rooms.ToList();
        }
        #endregion

        #region Device Methods
        public void AddDevice(SmartDevice device, int roomID)
        {
            _devices[device.DeviceID] = device;
            Room room = GetRoom(roomID);
            if (room != null)
            {
                room.AddDevice(device);
            }
        }

        public SmartDevice GetDevice(int deviceID)
        {
            return _devices.TryGetValue(deviceID, out SmartDevice device) ? device : null;
        }

        public List<SmartDevice> GetAllDevices()
        {
            return _devices.Values.ToList();
        }

        public List<T> GetDevicesOfType<T>() where T : SmartDevice
        {
            return _devices.Values.OfType<T>().ToList();
        }

        public void UpdateDeviceStatus(int deviceID, bool status)
        {
            if (_devices.TryGetValue(deviceID, out SmartDevice device))
            {
                if (status)
                {
                    device.TurnOn();
                }
                else
                {
                    device.TurnOff();
                }
            }
        }
        #endregion

        #region Sensor Methods
        public void AddSensor(Sensor sensor, int roomID)
        {
            _sensors[sensor.SensorID] = sensor;
            Room room = GetRoom(roomID);
            if (room != null)
            {
                room.AddSensor(sensor);
            }
            _sensorReadings[sensor.SensorID] = new List<SensorReading>();
        }

        public Sensor GetSensor(int sensorID)
        {
            return _sensors.TryGetValue(sensorID, out Sensor sensor) ? sensor : null;
        }

        public List<Sensor> GetAllSensors()
        {
            return _sensors.Values.ToList();
        }

        public List<T> GetSensorsOfType<T>() where T : Sensor
        {
            return _sensors.Values.OfType<T>().ToList();
        }

        public void RecordSensorReading(int sensorID, string readingValue)
        {
            if (!_sensorReadings.TryGetValue(sensorID, out List<SensorReading> readings))
            {
                readings = new List<SensorReading>();
                _sensorReadings[sensorID] = readings;
            }

            SensorReading reading = new SensorReading
            {
                SensorID = sensorID,
                ReadingValue = readingValue,
                ReadingTime = DateTime.Now
            };

            readings.Add(reading);
        }

        public List<SensorReading> GetSensorReadings(int sensorID)
        {
            return _sensorReadings.TryGetValue(sensorID, out List<SensorReading> readings) ? readings : new List<SensorReading>();
        }
        #endregion

        #region Schedule Methods
        public void AddSchedule(Schedule schedule)
        {
            _schedules.Add(schedule);
        }

        public Schedule GetSchedule(int scheduleID)
        {
            return _schedules.FirstOrDefault(s => s.ScheduleID == scheduleID);
        }

        public List<Schedule> GetAllSchedules()
        {
            return _schedules.ToList();
        }

        public List<Schedule> GetActiveSchedules()
        {
            DateTime now = DateTime.Now;
            return _schedules.Where(s => s.IsActiveAt(now)).ToList();
        }
        #endregion
    }
}
