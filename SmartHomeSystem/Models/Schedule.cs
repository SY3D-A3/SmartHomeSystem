using System;

namespace SmartHomeSystem.Models
{
    /// <summary>
    /// Represents a scheduled action for a smart device
    /// </summary>
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Action { get; set; } // e.g., "TurnOn", "TurnOff", "SetTemperature:22"
        public int DeviceID { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; } // e.g., "Daily", "Weekdays", "Weekend", "Monday,Wednesday,Friday"

        public Schedule(int scheduleID, DateTime startTime, DateTime endTime, string action, int deviceID)
        {
            ScheduleID = scheduleID;
            StartTime = startTime;
            EndTime = endTime;
            Action = action;
            DeviceID = deviceID;
            IsRecurring = false;
            RecurrencePattern = string.Empty;
        }

        /// <summary>
        /// Sets the schedule to recurring with the specified pattern
        /// </summary>
        /// <param name="pattern">Recurrence pattern (e.g., "Daily", "Weekdays")</param>
        public void SetRecurring(string pattern)
        {
            IsRecurring = true;
            RecurrencePattern = pattern;
        }

        /// <summary>
        /// Checks if the schedule is active at the specified time
        /// </summary>
        /// <param name="time">The time to check</param>
        /// <returns>True if the schedule is active, false otherwise</returns>
        public bool IsActiveAt(DateTime time)
        {
            // Check if the time is between start and end times
            bool isTimeInRange = time >= StartTime && time <= EndTime;
            
            if (!isTimeInRange)
            {
                return false;
            }
            
            // If not recurring, just check the date
            if (!IsRecurring)
            {
                return time.Date == StartTime.Date;
            }
            
            // Check recurrence pattern
            switch (RecurrencePattern.ToLower())
            {
                case "daily":
                    return true;
                
                case "weekdays":
                    return time.DayOfWeek != DayOfWeek.Saturday && time.DayOfWeek != DayOfWeek.Sunday;
                
                case "weekend":
                    return time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday;
                
                default:
                    // Check if the current day of week is in the pattern (e.g., "Monday,Wednesday,Friday")
                    string[] days = RecurrencePattern.Split(',');
                    foreach (string day in days)
                    {
                        if (Enum.TryParse<DayOfWeek>(day.Trim(), true, out DayOfWeek dayOfWeek) && 
                            time.DayOfWeek == dayOfWeek)
                        {
                            return true;
                        }
                    }
                    return false;
            }
        }

        /// <summary>
        /// Parses the action string to get the action name and parameters
        /// </summary>
        /// <returns>A tuple containing the action name and parameters</returns>
        public (string ActionName, string[] Parameters) ParseAction()
        {
            string[] parts = Action.Split(':');
            string actionName = parts[0];
            string[] parameters = parts.Length > 1 ? parts[1].Split(',') : new string[0];
            
            return (actionName, parameters);
        }

        /// <summary>
        /// Returns a string representation of the schedule
        /// </summary>
        /// <returns>A string containing the schedule details</returns>
        public override string ToString()
        {
            string recurringText = IsRecurring ? $" (Recurring: {RecurrencePattern})" : "";
            return $"Schedule {ScheduleID}: Device {DeviceID} - {Action} from {StartTime} to {EndTime}{recurringText}";
        }
    }
}
