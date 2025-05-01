using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartHomeSystem.Tests.Mocks;
using SmartHomeSystemWinForms.Controllers;
using SmartHomeSystemWinForms.Events;
using SmartHomeSystemWinForms.Models;
using SmartHomeSystemWinForms.Services;

namespace SmartHomeSystem.Tests.Controllers
{
    [TestClass]
    public class SmartHomeControllerTests
    {
        private MockDatabaseService _mockDbService;
        private SmartHomeController _controller;
        private List<string> _logMessages;
        private List<SmartDevice> _deviceStatusChanges;
        private List<Sensor> _sensorReadings;
        private List<Schedule> _scheduleStatusChanges;

        [TestInitialize]
        public void Setup()
        {
            // Create a mock database service
            _mockDbService = new MockDatabaseService();
            
            // Create the controller with the mock database service
            _controller = new SmartHomeController(_mockDbService);
            
            // Initialize tracking collections
            _logMessages = new List<string>();
            _deviceStatusChanges = new List<SmartDevice>();
            _sensorReadings = new List<Sensor>();
            _scheduleStatusChanges = new List<Schedule>();
            
            // Subscribe to controller events
            _controller.OnLogMessage += (sender, message) => _logMessages.Add(message);
            _controller.OnDeviceStatusChanged += (sender, e) => _deviceStatusChanges.Add(e.Device);
            _controller.OnSensorReadingTaken += (sender, e) => _sensorReadings.Add(e.Sensor);
            _controller.OnScheduleStatusChanged += (sender, e) => _scheduleStatusChanges.Add(e.Schedule);
        }

        [TestMethod]
        public void Initialize_ShouldAddSampleDevicesAndSensors()
        {
            // Act
            _controller.Initialize();
            
            // Assert
            Assert.IsTrue(_mockDbService.GetAllDevices().Count > 0, "No devices were added during initialization");
            Assert.IsTrue(_mockDbService.GetAllSensors().Count > 0, "No sensors were added during initialization");
            Assert.IsTrue(_mockDbService.GetAllSchedules().Count > 0, "No schedules were added during initialization");
            Assert.IsTrue(_logMessages.Count > 0, "No log messages were generated during initialization");
        }

        [TestMethod]
        public void AddDevice_ShouldAddDeviceToDatabase()
        {
            // Arrange
            Light testLight = new Light(100, "Test Location", "Test Color");
            int roomId = 1;
            string deviceName = "test light";
            
            // Act
            _controller.AddDevice(testLight, roomId, deviceName);
            
            // Assert
            SmartDevice retrievedDevice = _mockDbService.GetDevice(100);
            Assert.IsNotNull(retrievedDevice, "Device was not added to the database");
            Assert.AreEqual(1, _deviceStatusChanges.Count, "Device status changed event was not raised");
            Assert.AreEqual(testLight, _deviceStatusChanges[0], "Device in event args is not the same as the added device");
        }

        [TestMethod]
        public void AddSensor_ShouldAddSensorToDatabase()
        {
            // Arrange
            TemperatureSensor testSensor = new TemperatureSensor(100, "Test Location", 22.0);
            int roomId = 1;
            
            // Act
            _controller.AddSensor(testSensor, roomId);
            
            // Assert
            Sensor retrievedSensor = _mockDbService.GetSensor(100);
            Assert.IsNotNull(retrievedSensor, "Sensor was not added to the database");
            Assert.AreEqual(1, _sensorReadings.Count, "Sensor reading event was not raised");
            Assert.AreEqual(testSensor, _sensorReadings[0], "Sensor in event args is not the same as the added sensor");
        }

        [TestMethod]
        public void AddSchedule_ShouldAddScheduleToDatabase()
        {
            // Arrange
            DateTime startTime = DateTime.Now.AddMinutes(5);
            DateTime endTime = DateTime.Now.AddMinutes(10);
            Schedule testSchedule = new Schedule(100, startTime, endTime, "TurnOn", 1);
            
            // Act
            _controller.AddSchedule(testSchedule);
            
            // Assert
            Schedule retrievedSchedule = _mockDbService.GetSchedule(100);
            Assert.IsNotNull(retrievedSchedule, "Schedule was not added to the database");
            Assert.AreEqual(1, _scheduleStatusChanges.Count, "Schedule status changed event was not raised");
            Assert.AreEqual(testSchedule, _scheduleStatusChanges[0], "Schedule in event args is not the same as the added schedule");
        }

        [TestMethod]
        public void ProcessVoiceCommand_ShouldReturnValidResponse()
        {
            // Arrange
            _controller.Initialize(); // Initialize to add sample devices
            
            // Act
            string result = _controller.ProcessVoiceCommand("turn on living room light");
            
            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result), "Voice command returned empty result");
            Assert.IsTrue(_logMessages.Any(m => m.Contains("Processing voice command")), "Voice command processing was not logged");
        }

        [TestMethod]
        public void GetDevicesOfType_ShouldReturnOnlyRequestedType()
        {
            // Arrange
            _controller.Initialize(); // Initialize to add sample devices
            
            // Add a mix of devices
            _controller.AddDevice(new Light(101, "Test Location 1", "Test Color"), 1, "test light 1");
            _controller.AddDevice(new Thermostat(102, "Test Location 2", 22.0), 1, "test thermostat");
            _controller.AddDevice(new Light(103, "Test Location 3", "Test Color"), 2, "test light 2");
            
            // Act
            List<Light> lights = _controller.GetDevicesOfType<Light>();
            
            // Assert
            Assert.IsTrue(lights.Count > 0, "No lights were returned");
            Assert.IsTrue(lights.All(d => d is Light), "Non-Light devices were returned");
        }

        [TestMethod]
        public void GetSensorsOfType_ShouldReturnOnlyRequestedType()
        {
            // Arrange
            _controller.Initialize(); // Initialize to add sample sensors
            
            // Add a mix of sensors
            _controller.AddSensor(new TemperatureSensor(101, "Test Location 1", 22.0), 1);
            _controller.AddSensor(new MotionSensor(102, "Test Location 2", 5), 1);
            _controller.AddSensor(new TemperatureSensor(103, "Test Location 3", 23.0), 2);
            
            // Act
            List<TemperatureSensor> tempSensors = _controller.GetSensorsOfType<TemperatureSensor>();
            
            // Assert
            Assert.IsTrue(tempSensors.Count > 0, "No temperature sensors were returned");
            Assert.IsTrue(tempSensors.All(s => s is TemperatureSensor), "Non-TemperatureSensor sensors were returned");
        }

        [TestMethod]
        public void Start_ShouldSetIsRunningToTrue()
        {
            // Arrange - controller is created in Setup
            
            // Act
            _controller.Start();
            
            // Assert
            Assert.IsTrue(_logMessages.Any(m => m.Contains("Smart Home Controller started")), "Controller start was not logged");
            
            // Stop the controller to clean up
            _controller.Stop();
        }

        [TestMethod]
        public void Stop_ShouldSetIsRunningToFalse()
        {
            // Arrange
            _controller.Start();
            
            // Act
            _controller.Stop();
            
            // Assert
            Assert.IsTrue(_logMessages.Any(m => m.Contains("Smart Home Controller stopped")), "Controller stop was not logged");
        }
    }
}
