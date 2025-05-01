using System;
using System.Collections.Generic;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents a scene that groups multiple device actions together
    /// </summary>
    public class Scene
    {
        public int SceneID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SceneAction> Actions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivatedAt { get; set; }
        public int ActivationCount { get; set; }
        public bool IsFavorite { get; set; }

        public Scene()
        {
            Actions = new List<SceneAction>();
            CreatedAt = DateTime.Now;
            LastActivatedAt = DateTime.MinValue;
            ActivationCount = 0;
            IsFavorite = false;
        }

        public Scene(int sceneID, string name, string description = "")
        {
            SceneID = sceneID;
            Name = name;
            Description = description;
            Actions = new List<SceneAction>();
            CreatedAt = DateTime.Now;
            LastActivatedAt = DateTime.MinValue;
            ActivationCount = 0;
            IsFavorite = false;
        }

        /// <summary>
        /// Adds an action to the scene
        /// </summary>
        /// <param name="action">The action to add</param>
        public void AddAction(SceneAction action)
        {
            Actions.Add(action);
        }

        /// <summary>
        /// Records that the scene was activated
        /// </summary>
        public void RecordActivation()
        {
            LastActivatedAt = DateTime.Now;
            ActivationCount++;
        }

        /// <summary>
        /// Returns a string representation of the scene
        /// </summary>
        /// <returns>A string containing the scene details</returns>
        public override string ToString()
        {
            string favoriteFlag = IsFavorite ? " [Favorite]" : "";
            return $"Scene {SceneID}: {Name}{favoriteFlag} - {Actions.Count} actions";
        }
    }

    /// <summary>
    /// Represents an action to be performed as part of a scene
    /// </summary>
    public class SceneAction
    {
        public int ActionID { get; set; }
        public int DeviceID { get; set; }
        public string ActionType { get; set; } // e.g., "TurnOn", "SetBrightness", "SetTemperature"
        public string ActionValue { get; set; } // e.g., "100" for brightness, "22" for temperature
        public int ExecutionOrder { get; set; } // Order in which actions should be executed

        public SceneAction(int actionID, int deviceID, string actionType, string actionValue, int executionOrder = 0)
        {
            ActionID = actionID;
            DeviceID = deviceID;
            ActionType = actionType;
            ActionValue = actionValue;
            ExecutionOrder = executionOrder;
        }

        /// <summary>
        /// Returns a string representation of the action
        /// </summary>
        /// <returns>A string containing the action details</returns>
        public override string ToString()
        {
            return $"Action {ActionID}: Device {DeviceID} - {ActionType}({ActionValue}) [Order: {ExecutionOrder}]";
        }
    }
}
