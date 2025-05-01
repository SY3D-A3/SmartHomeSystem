using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Handles voice commands for controlling smart devices
    /// </summary>
    public class VoiceCommand
    {
        private readonly Dictionary<string, SmartDevice> _devices;
        private readonly Dictionary<string, string> _deviceAliases;

        public VoiceCommand(Dictionary<string, SmartDevice> devices)
        {
            _devices = devices;
            _deviceAliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Adds an alias for a device to make voice commands more natural
        /// </summary>
        /// <param name="deviceKey">The device key in the devices dictionary</param>
        /// <param name="alias">The alias to use for the device</param>
        public void AddDeviceAlias(string deviceKey, string alias)
        {
            _deviceAliases[alias] = deviceKey;
        }

        /// <summary>
        /// Parses a voice command string into an action and parameters
        /// </summary>
        /// <param name="command">The voice command string</param>
        /// <returns>A tuple containing the action, device, and parameters</returns>
        public (string Action, string Device, string[] Parameters) ParseCommand(string command)
        {
            // Convert to lowercase for easier matching
            command = command.ToLower();
            
            // Define patterns for different types of commands
            var turnOnPattern = new Regex(@"turn on (the )?([\w\s]+)");
            var turnOffPattern = new Regex(@"turn off (the )?([\w\s]+)");
            var setBrightnessPattern = new Regex(@"set (the )?([\w\s]+) brightness to (\d+)");
            var setTemperaturePattern = new Regex(@"set (the )?([\w\s]+) temperature to (\d+)");
            var setColorPattern = new Regex(@"set (the )?([\w\s]+) color to ([\w\s]+)");
            var startRecordingPattern = new Regex(@"start recording (on )?(the )?([\w\s]+)");
            var stopRecordingPattern = new Regex(@"stop recording (on )?(the )?([\w\s]+)");
            
            // Check for turn on command
            var match = turnOnPattern.Match(command);
            if (match.Success)
            {
                string device = match.Groups[2].Value.Trim();
                return ("TurnOn", device, new string[0]);
            }
            
            // Check for turn off command
            match = turnOffPattern.Match(command);
            if (match.Success)
            {
                string device = match.Groups[2].Value.Trim();
                return ("TurnOff", device, new string[0]);
            }
            
            // Check for set brightness command
            match = setBrightnessPattern.Match(command);
            if (match.Success)
            {
                string device = match.Groups[2].Value.Trim();
                string brightness = match.Groups[3].Value;
                return ("SetBrightness", device, new[] { brightness });
            }
            
            // Check for set temperature command
            match = setTemperaturePattern.Match(command);
            if (match.Success)
            {
                string device = match.Groups[2].Value.Trim();
                string temperature = match.Groups[3].Value;
                return ("SetTemperature", device, new[] { temperature });
            }
            
            // Check for set color command
            match = setColorPattern.Match(command);
            if (match.Success)
            {
                string device = match.Groups[2].Value.Trim();
                string color = match.Groups[3].Value;
                return ("SetColor", device, new[] { color });
            }
            
            // Check for start recording command
            match = startRecordingPattern.Match(command);
            if (match.Success)
            {
                string device = match.Groups[3].Value.Trim();
                return ("StartRecording", device, new string[0]);
            }
            
            // Check for stop recording command
            match = stopRecordingPattern.Match(command);
            if (match.Success)
            {
                string device = match.Groups[3].Value.Trim();
                return ("StopRecording", device, new string[0]);
            }
            
            // If no pattern matches, return an unknown action
            return ("Unknown", "", new string[0]);
        }

        /// <summary>
        /// Executes a parsed voice command
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <param name="deviceName">The name or alias of the device</param>
        /// <param name="parameters">Parameters for the action</param>
        /// <returns>A string indicating the result of the command</returns>
        public string ExecuteCommand(string action, string deviceName, string[] parameters)
        {
            // Resolve device alias if one exists
            string deviceKey = deviceName;
            if (_deviceAliases.ContainsKey(deviceName))
            {
                deviceKey = _deviceAliases[deviceName];
            }
            
            // Find the device
            if (!_devices.ContainsKey(deviceKey))
            {
                return $"Device '{deviceName}' not found";
            }
            
            SmartDevice device = _devices[deviceKey];
            
            // Execute the appropriate action
            switch (action)
            {
                case "TurnOn":
                    device.TurnOn();
                    return $"{deviceName} turned on";
                
                case "TurnOff":
                    device.TurnOff();
                    return $"{deviceName} turned off";
                
                case "SetBrightness":
                    if (device is Light light && parameters.Length > 0 && int.TryParse(parameters[0], out int brightness))
                    {
                        light.SetBrightness(brightness);
                        return $"{deviceName} brightness set to {brightness}%";
                    }
                    return $"Cannot set brightness for {deviceName}";
                
                case "SetTemperature":
                    if (device is Thermostat thermostat && parameters.Length > 0 && double.TryParse(parameters[0], out double temperature))
                    {
                        thermostat.SetTargetTemperature(temperature);
                        return $"{deviceName} temperature set to {temperature}°C";
                    }
                    return $"Cannot set temperature for {deviceName}";
                
                case "SetColor":
                    if (device is Light colorLight && parameters.Length > 0)
                    {
                        colorLight.SetColor(parameters[0]);
                        return $"{deviceName} color set to {parameters[0]}";
                    }
                    return $"Cannot set color for {deviceName}";
                
                case "StartRecording":
                    if (device is SecurityCamera camera)
                    {
                        camera.StartRecording();
                        return $"Started recording on {deviceName}";
                    }
                    return $"Cannot start recording on {deviceName}";
                
                case "StopRecording":
                    if (device is SecurityCamera stopCamera)
                    {
                        stopCamera.StopRecording();
                        return $"Stopped recording on {deviceName}";
                    }
                    return $"Cannot stop recording on {deviceName}";
                
                default:
                    return $"Unknown command: {action}";
            }
        }

        /// <summary>
        /// Processes a voice command string from start to finish
        /// </summary>
        /// <param name="commandString">The voice command string</param>
        /// <returns>A string indicating the result of the command</returns>
        public string ProcessCommand(string commandString)
        {
            Console.WriteLine($"Processing voice command: \"{commandString}\"");
            
            var (action, device, parameters) = ParseCommand(commandString);
            
            if (action == "Unknown")
            {
                return "Sorry, I didn't understand that command";
            }
            
            return ExecuteCommand(action, device, parameters);
        }
    }
}
