using System;

namespace SmartHomeSystem.Models
{
    /// <summary>
    /// Represents a motion sensor that can detect movement
    /// </summary>
    public class MotionSensor : Sensor
    {
        public bool MotionDetected { get; private set; }
        public int Sensitivity { get; set; } // 1-10, with 10 being most sensitive
        public event EventHandler<MotionDetectedEventArgs> OnMotionDetected;

        public MotionSensor(int sensorID, string location, int sensitivity = 5) 
            : base(sensorID, location)
        {
            MotionDetected = false;
            Sensitivity = sensitivity;
        }

        /// <summary>
        /// Sets the sensitivity level of the motion sensor
        /// </summary>
        /// <param name="level">Sensitivity level (1-10)</param>
        public void SetSensitivity(int level)
        {
            if (level < 1 || level > 10)
            {
                throw new ArgumentOutOfRangeException("Sensitivity must be between 1 and 10");
            }

            Sensitivity = level;
            Console.WriteLine($"Motion sensor {SensorID} sensitivity set to {level}");
        }

        /// <summary>
        /// Simulates motion detection
        /// </summary>
        /// <param name="hasMotion">Whether motion is detected</param>
        public void DetectMotion(bool hasMotion)
        {
            if (!IsActive)
            {
                return;
            }

            MotionDetected = hasMotion;
            UpdateLastReadingTime();
            
            if (hasMotion)
            {
                Console.WriteLine($"Motion detected by sensor {SensorID} in {Location}");
                OnMotionDetected?.Invoke(this, new MotionDetectedEventArgs(SensorID, Location, DateTime.Now));
            }
            else
            {
                Console.WriteLine($"No motion detected by sensor {SensorID} in {Location}");
            }
        }

        /// <summary>
        /// Takes a reading from the motion sensor
        /// </summary>
        /// <returns>A boolean indicating whether motion is detected</returns>
        public override object TakeReading()
        {
            if (!IsActive)
            {
                Console.WriteLine($"Cannot take reading: Motion sensor {SensorID} is not active");
                return false;
            }
            
            // In a real implementation, this would read from actual hardware
            // For simulation, we'll randomly detect motion based on sensitivity
            Random random = new Random();
            bool detected = random.Next(1, 11) <= Sensitivity;
            
            DetectMotion(detected);
            return MotionDetected;
        }

        /// <summary>
        /// Returns a string representation of the motion sensor
        /// </summary>
        /// <returns>A string containing the sensor details</returns>
        public override string ToString()
        {
            string status = IsActive ? "Active" : "Inactive";
            string motion = MotionDetected ? "Motion detected" : "No motion";
            return $"Motion Sensor {SensorID} in {Location}: {status}, {motion}, Sensitivity: {Sensitivity}";
        }
    }

    /// <summary>
    /// Event arguments for motion detection events
    /// </summary>
    public class MotionDetectedEventArgs : EventArgs
    {
        public int SensorID { get; }
        public string Location { get; }
        public DateTime DetectionTime { get; }

        public MotionDetectedEventArgs(int sensorID, string location, DateTime detectionTime)
        {
            SensorID = sensorID;
            Location = location;
            DetectionTime = detectionTime;
        }
    }
}
