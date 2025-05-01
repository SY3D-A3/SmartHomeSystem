using System;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Abstract base class for all smart devices in the system
    /// </summary>
    public abstract class SmartDevice
    {
        public int DeviceID { get; set; }
        public bool Status { get; set; } // true = on, false = off
        public string Location { get; set; }

        public SmartDevice(int deviceID, string location)
        {
            DeviceID = deviceID;
            Location = location;
            Status = false; // Default to off
        }

        /// <summary>
        /// Turns the device on
        /// </summary>
        public virtual void TurnOn()
        {
            Status = true;
            Console.WriteLine($"Device {DeviceID} in {Location} turned ON");
        }

        /// <summary>
        /// Turns the device off
        /// </summary>
        public virtual void TurnOff()
        {
            Status = false;
            Console.WriteLine($"Device {DeviceID} in {Location} turned OFF");
        }

        /// <summary>
        /// Gets the current status of the device
        /// </summary>
        /// <returns>True if the device is on, false if it's off</returns>
        public bool GetStatus()
        {
            return Status;
        }

        /// <summary>
        /// Returns a string representation of the device status
        /// </summary>
        /// <returns>A string indicating whether the device is on or off</returns>
        public string GetStatusString()
        {
            return Status ? "ON" : "OFF";
        }
    }
}
