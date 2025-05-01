
using System;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents a temperature sensor that can measure ambient temperature
    /// </summary>
    public class TemperatureSensor : Sensor
    {
        public double CurrentTemperature { get; private set; }
        public double HighTemperatureThreshold { get; set; }
        public double LowTemperatureThreshold { get; set; }
        public event EventHandler<TemperatureThresholdEventArgs> OnTemperatureThresholdReached;

        public TemperatureSensor(int sensorID, string location, double initialTemp = 22.0) 
            : base(sensorID, location)
        {
            CurrentTemperature = initialTemp;
            HighTemperatureThreshold = 30.0; // Default high threshold
            LowTemperatureThreshold = 15.0;  // Default low threshold
        }

        /// <summary>
        /// Sets the high temperature threshold for alerts
        /// </summary>
        /// <param name="threshold">High temperature threshold in Celsius</param>
        public void SetHighTemperatureThreshold(double threshold)
        {
            HighTemperatureThreshold = threshold;
            Console.WriteLine($"Temperature sensor {SensorID} high threshold set to {threshold}°C");
        }

        /// <summary>
        /// Sets the low temperature threshold for alerts
        /// </summary>
        /// <param name="threshold">Low temperature threshold in Celsius</param>
        public void SetLowTemperatureThreshold(double threshold)
        {
            LowTemperatureThreshold = threshold;
            Console.WriteLine($"Temperature sensor {SensorID} low threshold set to {threshold}°C");
        }

        /// <summary>
        /// Updates the current temperature reading and checks thresholds
        /// </summary>
        /// <param name="temperature">The new temperature reading in Celsius</param>
        public void UpdateTemperature(double temperature)
        {
            if (!IsActive)
            {
                return;
            }

            double previousTemperature = CurrentTemperature;
            CurrentTemperature = temperature;
            UpdateLastReadingTime();
            
            Console.WriteLine($"Temperature sensor {SensorID} in {Location} reading: {temperature}°C");
            
            // Check if temperature crossed any thresholds
            CheckThresholds(previousTemperature);
        }

        /// <summary>
        /// Checks if the temperature has crossed any thresholds and raises events if needed
        /// </summary>
        /// <param name="previousTemperature">The previous temperature reading</param>
        private void CheckThresholds(double previousTemperature)
        {
            // Check if temperature crossed the high threshold
            if (previousTemperature <= HighTemperatureThreshold && CurrentTemperature > HighTemperatureThreshold)
            {
                Console.WriteLine($"High temperature threshold ({HighTemperatureThreshold}°C) exceeded in {Location}: {CurrentTemperature}°C");
                OnTemperatureThresholdReached?.Invoke(this, new TemperatureThresholdEventArgs(
                    SensorID, Location, CurrentTemperature, TemperatureThresholdType.High));
            }
            
            // Check if temperature crossed the low threshold
            if (previousTemperature >= LowTemperatureThreshold && CurrentTemperature < LowTemperatureThreshold)
            {
                Console.WriteLine($"Low temperature threshold ({LowTemperatureThreshold}°C) reached in {Location}: {CurrentTemperature}°C");
                OnTemperatureThresholdReached?.Invoke(this, new TemperatureThresholdEventArgs(
                    SensorID, Location, CurrentTemperature, TemperatureThresholdType.Low));
            }
        }

        /// <summary>
        /// Takes a temperature reading
        /// </summary>
        /// <returns>The current temperature as a double</returns>
        public override object TakeReading()
        {
            if (!IsActive)
            {
                Console.WriteLine($"Cannot take reading: Temperature sensor {SensorID} is not active");
                return CurrentTemperature;
            }
            
            // In a real implementation, this would read from actual hardware
            // For simulation, we'll slightly adjust the current temperature
            Random random = new Random();
            double variation = (random.NextDouble() * 2 - 1) * 0.5; // Random variation between -0.5 and +0.5
            double newTemperature = Math.Round(CurrentTemperature + variation, 1);
            
            UpdateTemperature(newTemperature);
            return CurrentTemperature;
        }

        /// <summary>
        /// Returns a string representation of the temperature sensor
        /// </summary>
        /// <returns>A string containing the sensor details</returns>
        public override string ToString()
        {
            string status = IsActive ? "Active" : "Inactive";
            return $"Temperature Sensor {SensorID} in {Location}: {status}, Current: {CurrentTemperature}°C, " +
                   $"Thresholds: Low {LowTemperatureThreshold}°C, High {HighTemperatureThreshold}°C";
        }
    }

    /// <summary>
    /// Enum for temperature threshold types
    /// </summary>
    public enum TemperatureThresholdType
    {
        High,
        Low
    }

    /// <summary>
    /// Event arguments for temperature threshold events
    /// </summary>
    public class TemperatureThresholdEventArgs : EventArgs
    {
        public int SensorID { get; }
        public string Location { get; }
        public double Temperature { get; }
        public TemperatureThresholdType ThresholdType { get; }

        public TemperatureThresholdEventArgs(int sensorID, string location, double temperature, TemperatureThresholdType thresholdType)
        {
            SensorID = sensorID;
            Location = location;
            Temperature = temperature;
            ThresholdType = thresholdType;
        }
    }
}
