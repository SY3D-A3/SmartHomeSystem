using System;
using SmartHomeSystemWinForms.Models;

namespace SmartHomeSystemWinForms.Events
{
    /// <summary>
    /// Event arguments for device status changes
    /// </summary>
    public class DeviceStatusChangedEventArgs : EventArgs
    {
        public SmartDevice Device { get; }

        public DeviceStatusChangedEventArgs(SmartDevice device)
        {
            Device = device;
        }
    }

    /// <summary>
    /// Event arguments for sensor readings
    /// </summary>
    public class SensorReadingEventArgs : EventArgs
    {
        public Sensor Sensor { get; }
        public string Reading { get; }

        public SensorReadingEventArgs(Sensor sensor, string reading)
        {
            Sensor = sensor;
            Reading = reading;
        }
    }

    /// <summary>
    /// Event arguments for schedule status changes
    /// </summary>
    public class ScheduleStatusChangedEventArgs : EventArgs
    {
        public Schedule Schedule { get; }
        public bool IsActive { get; }

        public ScheduleStatusChangedEventArgs(Schedule schedule, bool isActive)
        {
            Schedule = schedule;
            IsActive = isActive;
        }
    }
}
