using System;
using System.Threading;
using SmartHomeSystem.Controllers;
using SmartHomeSystem.Models;

namespace SmartHomeSystem
{
    class Program
    {
        private static SmartHomeController _controller;
        private static bool _running = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Smart Home Automation System");
            Console.WriteLine("============================");
            Console.WriteLine();

            // Initialize the controller
            _controller = new SmartHomeController();
            _controller.Initialize();
            
            // Start the controller
            _controller.Start();
            
            // Display the main menu
            while (_running)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine().Trim();
                ProcessMenuChoice(choice);
            }
            
            // Stop the controller before exiting
            _controller.Stop();
        }

        /// <summary>
        /// Displays the main menu
        /// </summary>
        private static void DisplayMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. List Devices");
            Console.WriteLine("2. List Sensors");
            Console.WriteLine("3. List Rooms");
            Console.WriteLine("4. List Schedules");
            Console.WriteLine("5. Voice Command");
            Console.WriteLine("6. Simulate Motion Detection");
            Console.WriteLine("7. Simulate Temperature Change");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice (1-8): ");
        }

        /// <summary>
        /// Processes the user's menu choice
        /// </summary>
        /// <param name="choice">The user's choice</param>
        private static void ProcessMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    ListDevices();
                    break;
                
                case "2":
                    ListSensors();
                    break;
                
                case "3":
                    ListRooms();
                    break;
                
                case "4":
                    ListSchedules();
                    break;
                
                case "5":
                    ProcessVoiceCommand();
                    break;
                
                case "6":
                    SimulateMotionDetection();
                    break;
                
                case "7":
                    SimulateTemperatureChange();
                    break;
                
                case "8":
                    _running = false;
                    Console.WriteLine("Exiting Smart Home Automation System...");
                    break;
                
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            
            // Pause to let the user read the output
            if (_running)
            {
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        /// <summary>
        /// Lists all devices in the system
        /// </summary>
        private static void ListDevices()
        {
            Console.WriteLine();
            Console.WriteLine("Devices:");
            Console.WriteLine("--------");
            
            var devices = _controller.GetAllDevices();
            if (devices.Count == 0)
            {
                Console.WriteLine("No devices found.");
                return;
            }
            
            foreach (var device in devices)
            {
                string status = device.GetStatus() ? "ON" : "OFF";
                Console.WriteLine($"ID: {device.DeviceID}, Type: {device.GetType().Name}, Location: {device.Location}, Status: {status}");
                
                // Display device-specific properties
                if (device is Light light)
                {
                    Console.WriteLine($"  Brightness: {light.Brightness}%, Color: {light.Color}");
                }
                else if (device is Thermostat thermostat)
                {
                    Console.WriteLine($"  Current Temperature: {thermostat.CurrentTemperature}°C, Target: {thermostat.TargetTemperature}°C, Mode: {thermostat.Mode}");
                }
                else if (device is SecurityCamera camera)
                {
                    string recording = camera.IsRecording ? "Yes" : "No";
                    string motionDetection = camera.MotionDetectionEnabled ? "Enabled" : "Disabled";
                    Console.WriteLine($"  Recording: {recording}, Motion Detection: {motionDetection}, Resolution: {camera.Resolution}p");
                }
            }
        }

        /// <summary>
        /// Lists all sensors in the system
        /// </summary>
        private static void ListSensors()
        {
            Console.WriteLine();
            Console.WriteLine("Sensors:");
            Console.WriteLine("--------");
            
            var sensors = _controller.GetAllSensors();
            if (sensors.Count == 0)
            {
                Console.WriteLine("No sensors found.");
                return;
            }
            
            foreach (var sensor in sensors)
            {
                string status = sensor.IsActive ? "Active" : "Inactive";
                Console.WriteLine($"ID: {sensor.SensorID}, Type: {sensor.GetType().Name}, Location: {sensor.Location}, Status: {status}");
                
                // Display sensor-specific properties
                if (sensor is MotionSensor motionSensor)
                {
                    string motion = motionSensor.MotionDetected ? "Detected" : "Not Detected";
                    Console.WriteLine($"  Motion: {motion}, Sensitivity: {motionSensor.Sensitivity}");
                }
                else if (sensor is TemperatureSensor tempSensor)
                {
                    Console.WriteLine($"  Current Temperature: {tempSensor.CurrentTemperature}°C");
                    Console.WriteLine($"  Thresholds: Low {tempSensor.LowTemperatureThreshold}°C, High {tempSensor.HighTemperatureThreshold}°C");
                }
            }
        }

        /// <summary>
        /// Lists all rooms in the system
        /// </summary>
        private static void ListRooms()
        {
            Console.WriteLine();
            Console.WriteLine("Rooms:");
            Console.WriteLine("------");
            
            var rooms = _controller.GetAllRooms();
            if (rooms.Count == 0)
            {
                Console.WriteLine("No rooms found.");
                return;
            }
            
            foreach (var room in rooms)
            {
                Console.WriteLine($"ID: {room.RoomID}, Name: {room.Name}");
                
                // Display devices in the room
                var devices = room.GetDevices();
                if (devices.Count > 0)
                {
                    Console.WriteLine("  Devices:");
                    foreach (var device in devices)
                    {
                        string status = device.GetStatus() ? "ON" : "OFF";
                        Console.WriteLine($"    - {device.GetType().Name} (ID: {device.DeviceID}), Status: {status}");
                    }
                }
                
                // Display sensors in the room
                var sensors = room.GetSensors();
                if (sensors.Count > 0)
                {
                    Console.WriteLine("  Sensors:");
                    foreach (var sensor in sensors)
                    {
                        string status = sensor.IsActive ? "Active" : "Inactive";
                        Console.WriteLine($"    - {sensor.GetType().Name} (ID: {sensor.SensorID}), Status: {status}");
                    }
                }
            }
        }

        /// <summary>
        /// Lists all schedules in the system
        /// </summary>
        private static void ListSchedules()
        {
            Console.WriteLine();
            Console.WriteLine("Schedules:");
            Console.WriteLine("----------");
            
            var schedules = _controller.GetAllSchedules();
            if (schedules.Count == 0)
            {
                Console.WriteLine("No schedules found.");
                return;
            }
            
            foreach (var schedule in schedules)
            {
                string recurring = schedule.IsRecurring ? $" (Recurring: {schedule.RecurrencePattern})" : "";
                Console.WriteLine($"ID: {schedule.ScheduleID}, Device ID: {schedule.DeviceID}, Action: {schedule.Action}");
                Console.WriteLine($"  Time: {schedule.StartTime.ToShortTimeString()} - {schedule.EndTime.ToShortTimeString()}{recurring}");
                
                // Check if the schedule is active
                bool isActive = schedule.IsActiveAt(DateTime.Now);
                Console.WriteLine($"  Status: {(isActive ? "Active" : "Inactive")}");
            }
            
            // Display active schedules
            var activeSchedules = _controller.GetActiveSchedules();
            if (activeSchedules.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Currently Active Schedules:");
                foreach (var schedule in activeSchedules)
                {
                    Console.WriteLine($"  - Schedule {schedule.ScheduleID}: {schedule.Action} for Device {schedule.DeviceID}");
                }
            }
        }

        /// <summary>
        /// Processes a voice command
        /// </summary>
        private static void ProcessVoiceCommand()
        {
            Console.WriteLine();
            Console.WriteLine("Voice Command:");
            Console.WriteLine("-------------");
            Console.WriteLine("Enter a voice command (e.g., 'turn on the living room light'):");
            string command = Console.ReadLine().Trim();
            
            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine("Command cannot be empty.");
                return;
            }
            
            string result = _controller.ProcessVoiceCommand(command);
            Console.WriteLine($"Result: {result}");
        }

        /// <summary>
        /// Simulates motion detection in a room
        /// </summary>
        private static void SimulateMotionDetection()
        {
            Console.WriteLine();
            Console.WriteLine("Simulate Motion Detection:");
            Console.WriteLine("-------------------------");
            
            // Get all motion sensors
            var motionSensors = _controller.GetSensorsOfType<MotionSensor>();
            if (motionSensors.Count == 0)
            {
                Console.WriteLine("No motion sensors found.");
                return;
            }
            
            // Display available motion sensors
            Console.WriteLine("Available Motion Sensors:");
            foreach (var sensor in motionSensors)
            {
                Console.WriteLine($"ID: {sensor.SensorID}, Location: {sensor.Location}, Status: {(sensor.IsActive ? "Active" : "Inactive")}");
            }
            
            // Ask for sensor ID
            Console.Write("Enter sensor ID to simulate motion detection: ");
            if (!int.TryParse(Console.ReadLine().Trim(), out int sensorID))
            {
                Console.WriteLine("Invalid sensor ID.");
                return;
            }
            
            // Find the sensor
            var motionSensor = motionSensors.Find(s => s.SensorID == sensorID);
            if (motionSensor == null)
            {
                Console.WriteLine($"Motion sensor with ID {sensorID} not found.");
                return;
            }
            
            // Simulate motion detection
            Console.WriteLine($"Simulating motion detection for sensor {sensorID} in {motionSensor.Location}...");
            motionSensor.DetectMotion(true);
            
            // Wait a moment
            Thread.Sleep(2000);
            
            // Simulate motion stopping
            motionSensor.DetectMotion(false);
            Console.WriteLine("Motion simulation completed.");
        }

        /// <summary>
        /// Simulates a temperature change in a room
        /// </summary>
        private static void SimulateTemperatureChange()
        {
            Console.WriteLine();
            Console.WriteLine("Simulate Temperature Change:");
            Console.WriteLine("---------------------------");
            
            // Get all temperature sensors
            var tempSensors = _controller.GetSensorsOfType<TemperatureSensor>();
            if (tempSensors.Count == 0)
            {
                Console.WriteLine("No temperature sensors found.");
                return;
            }
            
            // Display available temperature sensors
            Console.WriteLine("Available Temperature Sensors:");
            foreach (var sensor in tempSensors)
            {
                Console.WriteLine($"ID: {sensor.SensorID}, Location: {sensor.Location}, Current: {sensor.CurrentTemperature}°C");
                Console.WriteLine($"  Thresholds: Low {sensor.LowTemperatureThreshold}°C, High {sensor.HighTemperatureThreshold}°C");
            }
            
            // Ask for sensor ID
            Console.Write("Enter sensor ID to simulate temperature change: ");
            if (!int.TryParse(Console.ReadLine().Trim(), out int sensorID))
            {
                Console.WriteLine("Invalid sensor ID.");
                return;
            }
            
            // Find the sensor
            var tempSensor = tempSensors.Find(s => s.SensorID == sensorID);
            if (tempSensor == null)
            {
                Console.WriteLine($"Temperature sensor with ID {sensorID} not found.");
                return;
            }
            
            // Ask for new temperature
            Console.Write("Enter new temperature (°C): ");
            if (!double.TryParse(Console.ReadLine().Trim(), out double temperature))
            {
                Console.WriteLine("Invalid temperature.");
                return;
            }
            
            // Simulate temperature change
            Console.WriteLine($"Simulating temperature change for sensor {sensorID} in {tempSensor.Location}...");
            Console.WriteLine($"Changing temperature from {tempSensor.CurrentTemperature}°C to {temperature}°C");
            tempSensor.UpdateTemperature(temperature);
            
            Console.WriteLine("Temperature simulation completed.");
        }
    }
}
