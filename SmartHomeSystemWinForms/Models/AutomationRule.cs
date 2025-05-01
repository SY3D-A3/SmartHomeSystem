using System;
using System.Collections.Generic;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents an automation rule that triggers actions based on conditions
    /// </summary>
    public class AutomationRule
    {
        public int RuleID { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public string TriggerType { get; set; } // "Sensor", "Time", "Device"
        public int? TriggerEntityID { get; set; } // ID of the sensor or device that triggers the rule
        public string TriggerCondition { get; set; } // e.g., ">25" for temperature, "==true" for motion
        public List<RuleAction> Actions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastTriggeredAt { get; set; }
        public int TriggerCount { get; set; }

        public AutomationRule()
        {
            Actions = new List<RuleAction>();
            CreatedAt = DateTime.Now;
            LastTriggeredAt = DateTime.MinValue;
            TriggerCount = 0;
            IsEnabled = true;
        }

        public AutomationRule(int ruleID, string name, string triggerType, int? triggerEntityID, string triggerCondition)
        {
            RuleID = ruleID;
            Name = name;
            TriggerType = triggerType;
            TriggerEntityID = triggerEntityID;
            TriggerCondition = triggerCondition;
            Actions = new List<RuleAction>();
            CreatedAt = DateTime.Now;
            LastTriggeredAt = DateTime.MinValue;
            TriggerCount = 0;
            IsEnabled = true;
        }

        /// <summary>
        /// Adds an action to the rule
        /// </summary>
        /// <param name="action">The action to add</param>
        public void AddAction(RuleAction action)
        {
            Actions.Add(action);
        }

        /// <summary>
        /// Evaluates if the rule should be triggered based on a sensor reading
        /// </summary>
        /// <param name="sensor">The sensor that produced a reading</param>
        /// <param name="readingValue">The reading value</param>
        /// <returns>True if the rule should be triggered, false otherwise</returns>
        public bool ShouldTriggerForSensor(Sensor sensor, string readingValue)
        {
            if (!IsEnabled || TriggerType != "Sensor" || TriggerEntityID != sensor.SensorID)
            {
                return false;
            }

            // Parse the condition
            if (TriggerCondition.StartsWith(">"))
            {
                double threshold = double.Parse(TriggerCondition.Substring(1));
                if (double.TryParse(readingValue, out double value))
                {
                    return value > threshold;
                }
            }
            else if (TriggerCondition.StartsWith("<"))
            {
                double threshold = double.Parse(TriggerCondition.Substring(1));
                if (double.TryParse(readingValue, out double value))
                {
                    return value < threshold;
                }
            }
            else if (TriggerCondition.StartsWith("=="))
            {
                string expected = TriggerCondition.Substring(2);
                return readingValue == expected;
            }
            else if (TriggerCondition.StartsWith("!="))
            {
                string expected = TriggerCondition.Substring(2);
                return readingValue != expected;
            }
            else if (TriggerCondition.StartsWith("contains:"))
            {
                string substring = TriggerCondition.Substring(9);
                return readingValue.Contains(substring);
            }

            return false;
        }

        /// <summary>
        /// Evaluates if the rule should be triggered based on a device state change
        /// </summary>
        /// <param name="device">The device that changed state</param>
        /// <returns>True if the rule should be triggered, false otherwise</returns>
        public bool ShouldTriggerForDevice(SmartDevice device)
        {
            if (!IsEnabled || TriggerType != "Device" || TriggerEntityID != device.DeviceID)
            {
                return false;
            }

            // Parse the condition
            if (TriggerCondition == "==true")
            {
                return device.Status;
            }
            else if (TriggerCondition == "==false")
            {
                return !device.Status;
            }

            return false;
        }

        /// <summary>
        /// Evaluates if the rule should be triggered based on the current time
        /// </summary>
        /// <param name="currentTime">The current time</param>
        /// <returns>True if the rule should be triggered, false otherwise</returns>
        public bool ShouldTriggerForTime(DateTime currentTime)
        {
            if (!IsEnabled || TriggerType != "Time")
            {
                return false;
            }

            // Time-based triggers don't use TriggerEntityID
            // Parse the condition which should be in format "HH:mm"
            if (DateTime.TryParse(TriggerCondition, out DateTime triggerTime))
            {
                return currentTime.Hour == triggerTime.Hour && currentTime.Minute == triggerTime.Minute;
            }

            return false;
        }

        /// <summary>
        /// Records that the rule was triggered
        /// </summary>
        public void RecordTrigger()
        {
            LastTriggeredAt = DateTime.Now;
            TriggerCount++;
        }

        /// <summary>
        /// Returns a string representation of the rule
        /// </summary>
        /// <returns>A string containing the rule details</returns>
        public override string ToString()
        {
            string status = IsEnabled ? "Enabled" : "Disabled";
            return $"Rule {RuleID}: {Name} ({status}) - Trigger: {TriggerType} {TriggerEntityID} {TriggerCondition}, Actions: {Actions.Count}";
        }
    }

    /// <summary>
    /// Represents an action to be performed when a rule is triggered
    /// </summary>
    public class RuleAction
    {
        public int ActionID { get; set; }
        public string ActionType { get; set; } // "Device", "Notification", "Scene"
        public int? TargetEntityID { get; set; } // ID of the device or scene to act on
        public string ActionParameters { get; set; } // e.g., "TurnOn", "SetTemperature:22"

        public RuleAction(int actionID, string actionType, int? targetEntityID, string actionParameters)
        {
            ActionID = actionID;
            ActionType = actionType;
            TargetEntityID = targetEntityID;
            ActionParameters = actionParameters;
        }

        /// <summary>
        /// Parses the action parameters
        /// </summary>
        /// <returns>A tuple containing the action name and parameters</returns>
        public (string ActionName, string[] Parameters) ParseActionParameters()
        {
            string[] parts = ActionParameters.Split(':');
            string actionName = parts[0];
            string[] parameters = parts.Length > 1 ? parts[1].Split(',') : new string[0];
            
            return (actionName, parameters);
        }

        /// <summary>
        /// Returns a string representation of the action
        /// </summary>
        /// <returns>A string containing the action details</returns>
        public override string ToString()
        {
            return $"Action {ActionID}: {ActionType} {TargetEntityID} - {ActionParameters}";
        }
    }
}
