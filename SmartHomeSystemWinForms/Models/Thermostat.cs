using System;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents a smart thermostat device
    /// </summary>
    public class Thermostat : SmartDevice
    {
        public double CurrentTemperature { get; private set; }
        public double TargetTemperature { get; private set; }
        public string Mode { get; private set; } // "Heat", "Cool", "Auto"

        public Thermostat(int deviceID, string location, double currentTemp = 22.0) 
            : base(deviceID, location)
        {
            CurrentTemperature = currentTemp;
            TargetTemperature = currentTemp;
            Mode = "Auto";
        }

        /// <summary>
        /// Sets the target temperature for the thermostat
        /// </summary>
        /// <param name="temperature">Target temperature in Celsius</param>
        public void SetTargetTemperature(double temperature)
        {
            if (temperature < 10 || temperature > 32)
            {
                throw new ArgumentOutOfRangeException("Temperature must be between 10°C and 32°C");
            }

            TargetTemperature = temperature;
            Console.WriteLine($"Thermostat {DeviceID} target temperature set to {temperature}°C");
            
            // If the thermostat was off, turn it on when setting a target temperature
            if (!Status)
            {
                TurnOn();
            }
        }

        /// <summary>
        /// Updates the current temperature reading (would normally come from a sensor)
        /// </summary>
        /// <param name="temperature">Current temperature in Celsius</param>
        public void UpdateCurrentTemperature(double temperature)
        {
            CurrentTemperature = temperature;
            Console.WriteLine($"Thermostat {DeviceID} current temperature updated to {temperature}°C");
            
            // If the thermostat is on, check if it needs to activate heating/cooling
            if (Status)
            {
                AdjustClimateControl();
            }
        }

        /// <summary>
        /// Sets the operating mode of the thermostat
        /// </summary>
        /// <param name="mode">Mode: "Heat", "Cool", or "Auto"</param>
        public void SetMode(string mode)
        {
            if (mode != "Heat" && mode != "Cool" && mode != "Auto")
            {
                throw new ArgumentException("Mode must be 'Heat', 'Cool', or 'Auto'");
            }

            Mode = mode;
            Console.WriteLine($"Thermostat {DeviceID} mode set to {mode}");
            
            // If the thermostat is on, adjust based on the new mode
            if (Status)
            {
                AdjustClimateControl();
            }
        }

        /// <summary>
        /// Adjusts the climate control based on current temperature, target temperature, and mode
        /// </summary>
        private void AdjustClimateControl()
        {
            double diff = Math.Abs(CurrentTemperature - TargetTemperature);
            
            if (Mode == "Heat" || (Mode == "Auto" && CurrentTemperature < TargetTemperature))
            {
                if (diff >= 0.5)
                {
                    Console.WriteLine($"Thermostat {DeviceID} is heating to reach {TargetTemperature}°C");
                }
            }
            else if (Mode == "Cool" || (Mode == "Auto" && CurrentTemperature > TargetTemperature))
            {
                if (diff >= 0.5)
                {
                    Console.WriteLine($"Thermostat {DeviceID} is cooling to reach {TargetTemperature}°C");
                }
            }
            else
            {
                Console.WriteLine($"Thermostat {DeviceID} is maintaining temperature at {CurrentTemperature}°C");
            }
        }

        /// <summary>
        /// Turns the thermostat on and begins climate control
        /// </summary>
        public override void TurnOn()
        {
            base.TurnOn();
            Console.WriteLine($"Thermostat {DeviceID} activated in {Mode} mode, target: {TargetTemperature}°C");
            AdjustClimateControl();
        }

        /// <summary>
        /// Turns the thermostat off
        /// </summary>
        public override void TurnOff()
        {
            base.TurnOff();
            Console.WriteLine($"Thermostat {DeviceID} deactivated. Climate control stopped.");
        }
    }
}
