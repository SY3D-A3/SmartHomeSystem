using System;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Abstract base class for all sensors in the system
    /// </summary>
    public abstract class Sensor
    {
        public int SensorID { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastReadingTime { get; private set; }

        public Sensor(int sensorID, string location)
        {
            SensorID = sensorID;
            Location = location;
            IsActive = false;
            LastReadingTime = DateTime.MinValue;
        }

        /// <summary>
        /// Activates the sensor
        /// </summary>
        public virtual void Activate()
        {
            IsActive = true;
            Console.WriteLine($"Sensor {SensorID} in {Location} activated");
        }

        /// <summary>
        /// Deactivates the sensor
        /// </summary>
        public virtual void Deactivate()
        {
            IsActive = false;
            Console.WriteLine($"Sensor {SensorID} in {Location} deactivated");
        }

        /// <summary>
        /// Updates the last reading time
        /// </summary>
        protected void UpdateLastReadingTime()
        {
            LastReadingTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the time elapsed since the last reading
        /// </summary>
        /// <returns>A TimeSpan representing the elapsed time</returns>
        public TimeSpan GetTimeSinceLastReading()
        {
            if (LastReadingTime == DateTime.MinValue)
            {
                return TimeSpan.MaxValue; // No reading has been taken yet
            }
            
            return DateTime.Now - LastReadingTime;
        }

        /// <summary>
        /// Abstract method to take a reading from the sensor
        /// </summary>
        /// <returns>The sensor reading as an object</returns>
        public abstract object TakeReading();
    }
}
