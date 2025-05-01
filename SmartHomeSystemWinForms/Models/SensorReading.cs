using System;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents a reading from a sensor
    /// </summary>
    public class SensorReading
    {
        public int ReadingID { get; set; }
        public int SensorID { get; set; }
        public string ReadingValue { get; set; }
        public DateTime ReadingTime { get; set; }
        
        // Reference to the sensor type for easier querying
        public string SensorType { get; set; }
        
        // Additional metadata for analytics
        public bool IsAnomalous { get; set; }
        public string Notes { get; set; }

        public SensorReading()
        {
            ReadingTime = DateTime.Now;
            IsAnomalous = false;
            Notes = string.Empty;
        }

        public SensorReading(int sensorID, string readingValue)
        {
            SensorID = sensorID;
            ReadingValue = readingValue;
            ReadingTime = DateTime.Now;
            IsAnomalous = false;
            Notes = string.Empty;
        }

        /// <summary>
        /// Checks if this reading is older than the specified timespan
        /// </summary>
        /// <param name="timeSpan">The timespan to check against</param>
        /// <returns>True if the reading is older than the timespan</returns>
        public bool IsOlderThan(TimeSpan timeSpan)
        {
            return DateTime.Now - ReadingTime > timeSpan;
        }

        /// <summary>
        /// Marks this reading as anomalous with optional notes
        /// </summary>
        /// <param name="notes">Notes explaining the anomaly</param>
        public void MarkAsAnomalous(string notes = "")
        {
            IsAnomalous = true;
            Notes = notes;
        }

        /// <summary>
        /// Returns a string representation of the reading
        /// </summary>
        /// <returns>A string containing the reading details</returns>
        public override string ToString()
        {
            string anomalyFlag = IsAnomalous ? " [ANOMALY]" : "";
            return $"Sensor {SensorID} reading at {ReadingTime}: {ReadingValue}{anomalyFlag}";
        }
    }
}
