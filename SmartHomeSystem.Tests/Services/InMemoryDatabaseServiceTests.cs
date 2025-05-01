using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartHomeSystemWinForms.Models;
using SmartHomeSystemWinForms.Services;

namespace SmartHomeSystem.Tests.Services
{
    [TestClass]
    public class InMemoryDatabaseServiceTests
    {
        private InMemoryDatabaseService _dbService;

        [TestInitialize]
        public void Setup()
        {
            _dbService = new InMemoryDatabaseService();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeWithSampleData()
        {
            // Assert
            Assert.IsTrue(_dbService.GetAllRooms().Count > 0, "No rooms were initialized");
            Assert.IsTrue(_dbService.GetAllUsers().Count > 0, "No users were initialized");
        }

        [TestMethod]
        public void AddUser_ShouldAddUserToDatabase()
        {
            // Arrange
            User testUser = new User(100, "Test User", "Test Role");
            
            // Act
            _dbService.AddUser(testUser);
            
            // Assert
            User retrievedUser = _dbService.GetUser(100);
            Assert.IsNotNull(retrievedUser, "User was not added to the database");
            Assert.AreEqual("Test User", retrievedUser.Name, "User name does not match");
            Assert.AreEqual("Test Role", retrievedUser.Role, "User role does not match");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddUser_ShouldThrowExceptionForDuplicateID()
        {
            // Arrange
            User testUser1 = new User(100, "Test User 1", "Test Role 1");
            User testUser2 = new User(100, "Test User 2", "Test Role 2");
            
            // Act
            _dbService.AddUser(testUser1);
            _dbService.AddUser(testUser2); // Should throw exception
        }

        [TestMethod]
        public void AddRoom_ShouldAddRoomToDatabase()
        {
            // Arrange
            Room testRoom = new Room(100, "Test Room");
            
            // Act
            _dbService.AddRoom(testRoom);
            
            // Assert
            Room retrievedRoom = _dbService.GetRoom(100);
            Assert.IsNotNull(retrievedRoom, "Room was not added to the database");
            Assert.AreEqual("Test Room", retrievedRoom.Name, "Room name does not match");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddRoom_ShouldThrowExceptionForDuplicateID()
        {
            // Arrange
            Room testRoom1 = new Room(100, "Test Room 1");
            Room testRoom2 = new Room(100, "Test Room 2");
            
            // Act
            _dbService.AddRoom(testRoom1);
            _dbService.AddRoom(testRoom2); // Should throw exception
        }

        [TestMethod]
        public void AddDevice_ShouldAddDeviceToDatabase()
        {
            // Arrange
            Light testLight = new Light(100, "Test Location", "Test Color");
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            
            // Act
            _dbService.AddDevice(testLight, 100);
            
            // Assert
            SmartDevice retrievedDevice = _dbService.GetDevice(100);
            Assert.IsNotNull(retrievedDevice, "Device was not added to the database");
            Assert.IsInstanceOfType(retrievedDevice, typeof(Light), "Retrieved device is not a Light");
            
            // Check if the device was added to the room
            Room retrievedRoom = _dbService.GetRoom(100);
            Assert.IsTrue(retrievedRoom.GetDevices().Contains(testLight), "Device was not added to the room");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDevice_ShouldThrowExceptionForDuplicateID()
        {
            // Arrange
            Light testLight1 = new Light(100, "Test Location 1", "Test Color 1");
            Light testLight2 = new Light(100, "Test Location 2", "Test Color 2");
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            
            // Act
            _dbService.AddDevice(testLight1, 100);
            _dbService.AddDevice(testLight2, 100); // Should throw exception
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDevice_ShouldThrowExceptionForNonExistentRoom()
        {
            // Arrange
            Light testLight = new Light(100, "Test Location", "Test Color");
            
            // Act
            _dbService.AddDevice(testLight, 999); // Room 999 doesn't exist
        }

        [TestMethod]
        public void AddSensor_ShouldAddSensorToDatabase()
        {
            // Arrange
            TemperatureSensor testSensor = new TemperatureSensor(100, "Test Location", 22.0);
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            
            // Act
            _dbService.AddSensor(testSensor, 100);
            
            // Assert
            Sensor retrievedSensor = _dbService.GetSensor(100);
            Assert.IsNotNull(retrievedSensor, "Sensor was not added to the database");
            Assert.IsInstanceOfType(retrievedSensor, typeof(TemperatureSensor), "Retrieved sensor is not a TemperatureSensor");
            
            // Check if the sensor was added to the room
            Room retrievedRoom = _dbService.GetRoom(100);
            Assert.IsTrue(retrievedRoom.GetSensors().Contains(testSensor), "Sensor was not added to the room");
        }

        [TestMethod]
        public void RecordSensorReading_ShouldAddReadingToDatabase()
        {
            // Arrange
            TemperatureSensor testSensor = new TemperatureSensor(100, "Test Location", 22.0);
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            _dbService.AddSensor(testSensor, 100);
            
            // Act
            _dbService.RecordSensorReading(100, "22.5");
            
            // Assert
            List<SensorReading> readings = _dbService.GetSensorReadings(100);
            Assert.AreEqual(1, readings.Count, "Sensor reading was not added");
            Assert.AreEqual("22.5", readings[0].ReadingValue, "Sensor reading value does not match");
        }

        [TestMethod]
        public void AddSchedule_ShouldAddScheduleToDatabase()
        {
            // Arrange
            Light testLight = new Light(100, "Test Location", "Test Color");
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            _dbService.AddDevice(testLight, 100);
            
            DateTime startTime = DateTime.Now.AddMinutes(5);
            DateTime endTime = DateTime.Now.AddMinutes(10);
            Schedule testSchedule = new Schedule(100, startTime, endTime, "TurnOn", 100);
            
            // Act
            _dbService.AddSchedule(testSchedule);
            
            // Assert
            Schedule retrievedSchedule = _dbService.GetSchedule(100);
            Assert.IsNotNull(retrievedSchedule, "Schedule was not added to the database");
            Assert.AreEqual(startTime, retrievedSchedule.StartTime, "Schedule start time does not match");
            Assert.AreEqual(endTime, retrievedSchedule.EndTime, "Schedule end time does not match");
            Assert.AreEqual("TurnOn", retrievedSchedule.Action, "Schedule action does not match");
            Assert.AreEqual(100, retrievedSchedule.DeviceID, "Schedule device ID does not match");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddSchedule_ShouldThrowExceptionForNonExistentDevice()
        {
            // Arrange
            DateTime startTime = DateTime.Now.AddMinutes(5);
            DateTime endTime = DateTime.Now.AddMinutes(10);
            Schedule testSchedule = new Schedule(100, startTime, endTime, "TurnOn", 999); // Device 999 doesn't exist
            
            // Act
            _dbService.AddSchedule(testSchedule);
        }

        [TestMethod]
        public void GetActiveSchedules_ShouldReturnOnlyActiveSchedules()
        {
            // Arrange
            Light testLight = new Light(100, "Test Location", "Test Color");
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            _dbService.AddDevice(testLight, 100);
            
            // Active schedule (starts 5 minutes ago, ends 5 minutes from now)
            DateTime pastStart = DateTime.Now.AddMinutes(-5);
            DateTime futureEnd = DateTime.Now.AddMinutes(5);
            Schedule activeSchedule = new Schedule(100, pastStart, futureEnd, "TurnOn", 100);
            
            // Inactive schedule (starts and ends in the future)
            DateTime futureStart = DateTime.Now.AddMinutes(5);
            DateTime futureEnd2 = DateTime.Now.AddMinutes(10);
            Schedule inactiveSchedule = new Schedule(101, futureStart, futureEnd2, "TurnOff", 100);
            
            _dbService.AddSchedule(activeSchedule);
            _dbService.AddSchedule(inactiveSchedule);
            
            // Act
            List<Schedule> activeSchedules = _dbService.GetActiveSchedules();
            
            // Assert
            Assert.AreEqual(1, activeSchedules.Count, "Wrong number of active schedules returned");
            Assert.AreEqual(100, activeSchedules[0].ScheduleID, "Wrong schedule returned as active");
        }

        [TestMethod]
        public void GetDevicesOfType_ShouldReturnOnlyRequestedType()
        {
            // Arrange
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            
            Light testLight1 = new Light(101, "Test Location 1", "Test Color");
            Light testLight2 = new Light(102, "Test Location 2", "Test Color");
            Thermostat testThermostat = new Thermostat(103, "Test Location 3", 22.0);
            
            _dbService.AddDevice(testLight1, 100);
            _dbService.AddDevice(testLight2, 100);
            _dbService.AddDevice(testThermostat, 100);
            
            // Act
            List<Light> lights = _dbService.GetDevicesOfType<Light>();
            
            // Assert
            Assert.AreEqual(2, lights.Count, "Wrong number of lights returned");
            Assert.IsTrue(lights.All(d => d is Light), "Non-Light devices were returned");
        }

        [TestMethod]
        public void GetSensorsOfType_ShouldReturnOnlyRequestedType()
        {
            // Arrange
            Room testRoom = new Room(100, "Test Room");
            _dbService.AddRoom(testRoom);
            
            TemperatureSensor testTempSensor1 = new TemperatureSensor(101, "Test Location 1", 22.0);
            TemperatureSensor testTempSensor2 = new TemperatureSensor(102, "Test Location 2", 23.0);
            MotionSensor testMotionSensor = new MotionSensor(103, "Test Location 3", 5);
            
            _dbService.AddSensor(testTempSensor1, 100);
            _dbService.AddSensor(testTempSensor2, 100);
            _dbService.AddSensor(testMotionSensor, 100);
            
            // Act
            List<TemperatureSensor> tempSensors = _dbService.GetSensorsOfType<TemperatureSensor>();
            
            // Assert
            Assert.AreEqual(2, tempSensors.Count, "Wrong number of temperature sensors returned");
            Assert.IsTrue(tempSensors.All(s => s is TemperatureSensor), "Non-TemperatureSensor sensors were returned");
        }
    }
}
