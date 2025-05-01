using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Timer = System.Threading.Timer;
using SmartHomeSystemWinForms.Models;
using SmartHomeSystemWinForms.Services;
using SmartHomeSystemWinForms.Events;

namespace SmartHomeSystemWinForms.Controllers
{
    /// <summary>
    /// Main controller for the Smart Home Automation System
    /// </summary>
    public class SmartHomeController : ISmartHomeController
    {
        private readonly IDatabaseService _dbService;
        private readonly Dictionary<string, SmartDevice> _devicesByName;
        private readonly VoiceCommand _voiceCommand;
        private readonly List<Schedule> _activeSchedules;
        private readonly System.Threading.Timer _scheduleTimer;
        private readonly System.Threading.Timer _sensorReadingTimer;
        private bool _isRunning;

        // Event handlers for UI updates
        public event EventHandler<DeviceStatusChangedEventArgs> OnDeviceStatusChanged;
        public event EventHandler<SensorReadingEventArgs> OnSensorReadingTaken;
        public event EventHandler<ScheduleStatusChangedEventArgs> OnScheduleStatusChanged;
        public event EventHandler<string> OnLogMessage;

        public SmartHomeController(IDatabaseService dbService = null)
        {
            _dbService = dbService ?? new InMemoryDatabaseService();
            _devicesByName = new Dictionary<string, SmartDevice>(StringComparer.OrdinalIgnoreCase);
            _activeSchedules = new List<Schedule>();
            _voiceCommand = new VoiceCommand(_devicesByName);
            _isRunning = false;
            
            // Create a timer to check schedules every minute
            _scheduleTimer = new Timer(CheckSchedules, null, Timeout.Infinite, Timeout.Infinite);
            
            // Create a timer to take sensor readings every 10 seconds
            _sensorReadingTimer = new Timer(TakeSensorReadings, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Initializes the controller with sample devices and sensors
        /// </summary>
        public void Initialize()
        {
            LogMessage("Initializing Smart Home Controller...");
            
            // Add sample devices to rooms
            AddSampleDevices();
            
            // Add sample sensors to rooms
            AddSampleSensors();
            
            // Add sample schedules
            AddSampleSchedules();
            
            // Register device aliases for voice commands
            RegisterDeviceAliases();
            
            LogMessage("Smart Home Controller initialized successfully.");
        }

        /// <summary>
        /// Starts the controller
        /// </summary>
        public void Start()
        {
            if (_isRunning)
            {
                LogMessage("Smart Home Controller is already running.");
                return;
            }
            
            LogMessage("Starting Smart Home Controller...");
            
            // Start the schedule timer to check every minute
            _scheduleTimer.Change(0, 60000);
            
            // Start the sensor reading timer to take readings every 10 seconds
            _sensorReadingTimer.Change(0, 10000);
            
            _isRunning = true;
            LogMessage("Smart Home Controller started.");
        }

        /// <summary>
        /// Stops the controller
        /// </summary>
        public void Stop()
        {
            if (!_isRunning)
            {
                LogMessage("Smart Home Controller is not running.");
                return;
            }
            
            LogMessage("Stopping Smart Home Controller...");
            
            // Stop the timers
            _scheduleTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _sensorReadingTimer.Change(Timeout.Infinite, Timeout.Infinite);
            
            _isRunning = false;
            LogMessage("Smart Home Controller stopped.");
        }

        /// <summary>
        /// Processes a voice command
        /// </summary>
        /// <param name="commandString">The voice command string</param>
        /// <returns>The result of the command</returns>
        public string ProcessVoiceCommand(string commandString)
        {
            LogMessage($"Processing voice command: \"{commandString}\"");
            return _voiceCommand.ProcessCommand(commandString);
        }

        /// <summary>
        /// Gets all devices in the system
        /// </summary>
        /// <returns>A list of all devices</returns>
        public List<SmartDevice> GetAllDevices()
        {
            return _dbService.GetAllDevices();
        }

        /// <summary>
        /// Gets all devices of a specific type
        /// </summary>
        /// <typeparam name="T">The type of device to get</typeparam>
        /// <returns>A list of devices of the specified type</returns>
        public List<T> GetDevicesOfType<T>() where T : SmartDevice
        {
            return _dbService.GetDevicesOfType<T>();
        }

        /// <summary>
        /// Gets all sensors in the system
        /// </summary>
        /// <returns>A list of all sensors</returns>
        public List<Sensor> GetAllSensors()
        {
            return _dbService.GetAllSensors();
        }

        /// <summary>
        /// Gets all sensors of a specific type
        /// </summary>
        /// <typeparam name="T">The type of sensor to get</typeparam>
        /// <returns>A list of sensors of the specified type</returns>
        public List<T> GetSensorsOfType<T>() where T : Sensor
        {
            return _dbService.GetSensorsOfType<T>();
        }

        /// <summary>
        /// Gets all rooms in the system
        /// </summary>
        /// <returns>A list of all rooms</returns>
        public List<Room> GetAllRooms()
        {
            return _dbService.GetAllRooms();
        }

        /// <summary>
        /// Gets all schedules in the system
        /// </summary>
        /// <returns>A list of all schedules</returns>
        public List<Schedule> GetAllSchedules()
        {
            return _dbService.GetAllSchedules();
        }

        /// <summary>
        /// Gets all active schedules
        /// </summary>
        /// <returns>A list of active schedules</returns>
        public List<Schedule> GetActiveSchedules()
        {
            return _activeSchedules;
        }

        /// <summary>
        /// Adds a new user to the system
        /// </summary>
        /// <param name="user">The user to add</param>
        public void AddUser(User user)
        {
            _dbService.AddUser(user);
            LogMessage($"User {user.UserID} ({user.Name}) added to the system");
        }

        /// <summary>
        /// Adds a new room to the system
        /// </summary>
        /// <param name="room">The room to add</param>
        public void AddRoom(Room room)
        {
            _dbService.AddRoom(room);
            LogMessage($"Room {room.RoomID} ({room.Name}) added to the system");
            
            // Raise event for UI update
            OnLogMessage?.Invoke(this, $"Room {room.Name} added successfully");
        }

        /// <summary>
        /// Adds a new device to the system
        /// </summary>
        /// <param name="device">The device to add</param>
        /// <param name="roomID">The ID of the room to add the device to</param>
        /// <param name="deviceName">The name to use for the device in voice commands</param>
        public void AddDevice(SmartDevice device, int roomID, string deviceName)
        {
            _dbService.AddDevice(device, roomID);
            
            if (!string.IsNullOrEmpty(deviceName))
            {
                _devicesByName[deviceName] = device;
                _voiceCommand.AddDeviceAlias(deviceName, deviceName);
                LogMessage($"Device alias '{deviceName}' registered for device {device.DeviceID}");
            }
            
            // Raise event for UI update
            OnDeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(device));
        }

        /// <summary>
        /// Adds a new sensor to the system
        /// </summary>
        /// <param name="sensor">The sensor to add</param>
        /// <param name="roomID">The ID of the room to add the sensor to</param>
        public void AddSensor(Sensor sensor, int roomID)
        {
            _dbService.AddSensor(sensor, roomID);
            
            // If it's a motion sensor, subscribe to motion detection events
            if (sensor is MotionSensor motionSensor)
            {
                motionSensor.OnMotionDetected += HandleMotionDetected;
            }
            
            // If it's a temperature sensor, subscribe to threshold events
            if (sensor is TemperatureSensor tempSensor)
            {
                tempSensor.OnTemperatureThresholdReached += HandleTemperatureThreshold;
            }
            
            // Raise event for UI update
            OnSensorReadingTaken?.Invoke(this, new SensorReadingEventArgs(sensor, sensor.TakeReading().ToString()));
        }

        /// <summary>
        /// Adds a new schedule to the system
        /// </summary>
        /// <param name="schedule">The schedule to add</param>
        public void AddSchedule(Schedule schedule)
        {
            _dbService.AddSchedule(schedule);
            
            // Check if the schedule is active now
            if (schedule.IsActiveAt(DateTime.Now))
            {
                _activeSchedules.Add(schedule);
                ExecuteScheduledAction(schedule);
            }
            
            // Raise event for UI update
            OnScheduleStatusChanged?.Invoke(this, new ScheduleStatusChangedEventArgs(schedule, schedule.IsActiveAt(DateTime.Now)));
        }

        /// <summary>
        /// Adds a sample set of devices to the system
        /// </summary>
        private void AddSampleDevices()
        {
            // Living Room devices
            Light livingRoomLight = new Light(1, "Living Room", "Warm White");
            livingRoomLight.SetBrightness(70);
            AddDevice(livingRoomLight, 1, "living room light");
            
            Thermostat livingRoomThermostat = new Thermostat(2, "Living Room", 22.5);
            livingRoomThermostat.SetMode("Auto");
            AddDevice(livingRoomThermostat, 1, "living room thermostat");
            
            SecurityCamera livingRoomCamera = new SecurityCamera(3, "Living Room", 1080);
            AddDevice(livingRoomCamera, 1, "living room camera");
            
            // Kitchen devices
            Light kitchenLight = new Light(4, "Kitchen", "Cool White");
            kitchenLight.SetBrightness(100);
            AddDevice(kitchenLight, 2, "kitchen light");
            
            // Bedroom devices
            Light bedroomLight = new Light(5, "Bedroom", "Soft White");
            bedroomLight.SetBrightness(50);
            AddDevice(bedroomLight, 3, "bedroom light");
            
            Thermostat bedroomThermostat = new Thermostat(6, "Bedroom", 20.0);
            bedroomThermostat.SetMode("Heat");
            AddDevice(bedroomThermostat, 3, "bedroom thermostat");
            
            // Bathroom devices
            Light bathroomLight = new Light(7, "Bathroom", "Cool White");
            bathroomLight.SetBrightness(80);
            AddDevice(bathroomLight, 4, "bathroom light");
        }

        /// <summary>
        /// Adds a sample set of sensors to the system
        /// </summary>
        private void AddSampleSensors()
        {
            // Living Room sensors
            MotionSensor livingRoomMotion = new MotionSensor(1, "Living Room", 7);
            AddSensor(livingRoomMotion, 1);
            livingRoomMotion.Activate();
            
            TemperatureSensor livingRoomTemp = new TemperatureSensor(2, "Living Room", 22.5);
            livingRoomTemp.SetHighTemperatureThreshold(26.0);
            livingRoomTemp.SetLowTemperatureThreshold(18.0);
            AddSensor(livingRoomTemp, 1);
            livingRoomTemp.Activate();
            
            // Kitchen sensors
            MotionSensor kitchenMotion = new MotionSensor(3, "Kitchen", 8);
            AddSensor(kitchenMotion, 2);
            kitchenMotion.Activate();
            
            TemperatureSensor kitchenTemp = new TemperatureSensor(4, "Kitchen", 23.0);
            kitchenTemp.SetHighTemperatureThreshold(28.0);
            kitchenTemp.SetLowTemperatureThreshold(18.0);
            AddSensor(kitchenTemp, 2);
            kitchenTemp.Activate();
            
            // Bedroom sensors
            MotionSensor bedroomMotion = new MotionSensor(5, "Bedroom", 5);
            AddSensor(bedroomMotion, 3);
            bedroomMotion.Activate();
            
            TemperatureSensor bedroomTemp = new TemperatureSensor(6, "Bedroom", 20.0);
            bedroomTemp.SetHighTemperatureThreshold(24.0);
            bedroomTemp.SetLowTemperatureThreshold(16.0);
            AddSensor(bedroomTemp, 3);
            bedroomTemp.Activate();
        }

        /// <summary>
        /// Adds a sample set of schedules to the system
        /// </summary>
        private void AddSampleSchedules()
        {
            // Schedule to turn on living room lights in the morning
            DateTime morningStart = DateTime.Today.AddHours(7);
            DateTime morningEnd = DateTime.Today.AddHours(9);
            Schedule morningLights = new Schedule(1, morningStart, morningEnd, "TurnOn", 1);
            morningLights.SetRecurring("Weekdays");
            AddSchedule(morningLights);
            
            // Schedule to turn off living room lights at night
            DateTime nightStart = DateTime.Today.AddHours(23);
            DateTime nightEnd = DateTime.Today.AddHours(23).AddMinutes(30);
            Schedule nightLights = new Schedule(2, nightStart, nightEnd, "TurnOff", 1);
            nightLights.SetRecurring("Daily");
            AddSchedule(nightLights);
            
            // Schedule to set bedroom thermostat temperature at night
            DateTime eveningStart = DateTime.Today.AddHours(21);
            DateTime eveningEnd = DateTime.Today.AddHours(22);
            Schedule eveningTemp = new Schedule(3, eveningStart, eveningEnd, "SetTemperature:19", 6);
            eveningTemp.SetRecurring("Daily");
            AddSchedule(eveningTemp);
        }

        /// <summary>
        /// Registers device aliases for voice commands
        /// </summary>
        private void RegisterDeviceAliases()
        {
            // Add some additional aliases for devices
            _voiceCommand.AddDeviceAlias("living room light", "main light");
            _voiceCommand.AddDeviceAlias("living room light", "front room light");
            
            _voiceCommand.AddDeviceAlias("bedroom light", "sleep light");
            _voiceCommand.AddDeviceAlias("bedroom thermostat", "bedroom temp");
            
            _voiceCommand.AddDeviceAlias("bathroom light", "restroom light");
        }

        /// <summary>
        /// Timer callback to check schedules
        /// </summary>
        private void CheckSchedules(object state)
        {
            try
            {
                DateTime now = DateTime.Now;
                LogMessage($"Checking schedules at {now}...");
                
                // Clear the active schedules list
                _activeSchedules.Clear();
                
                // Get all schedules that are active at the current time
                List<Schedule> activeSchedules = _dbService.GetActiveSchedules();
                
                foreach (Schedule schedule in activeSchedules)
                {
                    _activeSchedules.Add(schedule);
                    ExecuteScheduledAction(schedule);
                    
                    // Raise event for UI update
                    OnScheduleStatusChanged?.Invoke(this, new ScheduleStatusChangedEventArgs(schedule, true));
                }
                
                if (activeSchedules.Count > 0)
                {
                    LogMessage($"Found {activeSchedules.Count} active schedules.");
                }
                else
                {
                    LogMessage("No active schedules found.");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error checking schedules: {ex.Message}");
            }
        }

        /// <summary>
        /// Executes a scheduled action
        /// </summary>
        /// <param name="schedule">The schedule to execute</param>
        private void ExecuteScheduledAction(Schedule schedule)
        {
            try
            {
                SmartDevice device = _dbService.GetDevice(schedule.DeviceID);
                if (device == null)
                {
                    LogMessage($"Error executing schedule {schedule.ScheduleID}: Device {schedule.DeviceID} not found");
                    return;
                }
                
                var (actionName, parameters) = schedule.ParseAction();
                
                LogMessage($"Executing scheduled action: {actionName} for device {device.DeviceID} ({device.GetType().Name})");
                
                switch (actionName)
                {
                    case "TurnOn":
                        device.TurnOn();
                        break;
                    
                    case "TurnOff":
                        device.TurnOff();
                        break;
                    
                    case "SetBrightness":
                        if (device is Light light && parameters.Length > 0 && int.TryParse(parameters[0], out int brightness))
                        {
                            light.SetBrightness(brightness);
                        }
                        break;
                    
                    case "SetTemperature":
                        if (device is Thermostat thermostat && parameters.Length > 0 && double.TryParse(parameters[0], out double temperature))
                        {
                            thermostat.SetTargetTemperature(temperature);
                        }
                        break;
                    
                    case "SetColor":
                        if (device is Light colorLight && parameters.Length > 0)
                        {
                            colorLight.SetColor(parameters[0]);
                        }
                        break;
                    
                    default:
                        LogMessage($"Unknown action: {actionName}");
                        break;
                }
                
                // Raise event for UI update
                OnDeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(device));
            }
            catch (Exception ex)
            {
                LogMessage($"Error executing schedule {schedule.ScheduleID}: {ex.Message}");
            }
        }

        /// <summary>
        /// Timer callback to take sensor readings
        /// </summary>
        private void TakeSensorReadings(object state)
        {
            try
            {
                LogMessage("Taking sensor readings...");
                
                // Get all sensors
                List<Sensor> sensors = _dbService.GetAllSensors();
                
                foreach (Sensor sensor in sensors)
                {
                    if (sensor.IsActive)
                    {
                        object reading = sensor.TakeReading();
                        _dbService.RecordSensorReading(sensor.SensorID, reading.ToString());
                        
                        // Raise event for UI update
                        OnSensorReadingTaken?.Invoke(this, new SensorReadingEventArgs(sensor, reading.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error taking sensor readings: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles motion detection events
        /// </summary>
        private void HandleMotionDetected(object sender, Models.MotionDetectedEventArgs e)
        {
            LogMessage($"Motion detected in {e.Location} at {e.DetectionTime}");
            
            // Get the room where motion was detected
            Room room = _dbService.GetAllRooms().FirstOrDefault(r => r.Name == e.Location);
            if (room == null)
            {
                return;
            }
            
            // Get all lights in the room
            var lights = room.GetDevicesOfType<Light>();
            
            // Turn on all lights in the room if they're off
            foreach (var light in lights)
            {
                if (!light.GetStatus())
                {
                    light.TurnOn();
                    LogMessage($"Automatically turned on {light.GetType().Name} in {e.Location} due to motion detection");
                    
                    // Raise event for UI update
                    OnDeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(light));
                }
            }
            
            // Get all cameras in the room
            var cameras = room.GetDevicesOfType<SecurityCamera>();
            
            // Start recording on all cameras in the room
            foreach (var camera in cameras)
            {
                if (camera.GetStatus() && !camera.IsRecording)
                {
                    camera.StartRecording();
                    LogMessage($"Automatically started recording on camera in {e.Location} due to motion detection");
                    
                    // Raise event for UI update
                    OnDeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(camera));
                }
            }
        }

        /// <summary>
        /// Handles temperature threshold events
        /// </summary>
        private void HandleTemperatureThreshold(object sender, Models.TemperatureThresholdEventArgs e)
        {
            LogMessage($"Temperature threshold reached in {e.Location}: {e.Temperature}°C ({e.ThresholdType})");
            
            // Get the room where the temperature threshold was reached
            Room room = _dbService.GetAllRooms().FirstOrDefault(r => r.Name == e.Location);
            if (room == null)
            {
                return;
            }
            
            // Get all thermostats in the room
            var thermostats = room.GetDevicesOfType<Thermostat>();
            
            // Adjust thermostats based on the threshold type
            foreach (var thermostat in thermostats)
            {
                if (thermostat.GetStatus())
                {
                    if (e.ThresholdType == Models.TemperatureThresholdType.High)
                    {
                        // If temperature is too high, set mode to Cool
                        thermostat.SetMode("Cool");
                        LogMessage($"Automatically set thermostat in {e.Location} to Cool mode due to high temperature");
                    }
                    else if (e.ThresholdType == Models.TemperatureThresholdType.Low)
                    {
                        // If temperature is too low, set mode to Heat
                        thermostat.SetMode("Heat");
                        LogMessage($"Automatically set thermostat in {e.Location} to Heat mode due to low temperature");
                    }
                    
                    // Raise event for UI update
                    OnDeviceStatusChanged?.Invoke(this, new DeviceStatusChangedEventArgs(thermostat));
                }
            }
        }

        /// <summary>
        /// Logs a message and raises an event for the UI
        /// </summary>
        /// <param name="message">The message to log</param>
        private void LogMessage(string message)
        {
            Console.WriteLine(message);
            OnLogMessage?.Invoke(this, message);
        }
    }
}
