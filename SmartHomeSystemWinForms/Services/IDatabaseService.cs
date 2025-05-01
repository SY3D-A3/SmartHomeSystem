using System;
using System.Collections.Generic;
using SmartHomeSystemWinForms.Models;

namespace SmartHomeSystemWinForms.Services
{
    /// <summary>
    /// Interface for database services in the smart home system
    /// </summary>
    public interface IDatabaseService
    {
        #region User Methods
        void AddUser(User user);
        User GetUser(int userID);
        List<User> GetAllUsers();
        #endregion

        #region Room Methods
        void AddRoom(Room room);
        Room GetRoom(int roomID);
        List<Room> GetAllRooms();
        #endregion

        #region Device Methods
        void AddDevice(SmartDevice device, int roomID);
        SmartDevice GetDevice(int deviceID);
        List<SmartDevice> GetAllDevices();
        List<T> GetDevicesOfType<T>() where T : SmartDevice;
        void UpdateDeviceStatus(int deviceID, bool status);
        #endregion

        #region Sensor Methods
        void AddSensor(Sensor sensor, int roomID);
        Sensor GetSensor(int sensorID);
        List<Sensor> GetAllSensors();
        List<T> GetSensorsOfType<T>() where T : Sensor;
        void RecordSensorReading(int sensorID, string readingValue);
        List<SensorReading> GetSensorReadings(int sensorID);
        #endregion

        #region Schedule Methods
        void AddSchedule(Schedule schedule);
        Schedule GetSchedule(int scheduleID);
        List<Schedule> GetAllSchedules();
        List<Schedule> GetActiveSchedules();
        #endregion
    }
}
