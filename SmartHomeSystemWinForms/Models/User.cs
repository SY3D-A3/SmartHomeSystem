using System;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents a user of the smart home system
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } // e.g., "Admin", "Regular", "Guest"

        public User(int userID, string name, string role)
        {
            UserID = userID;
            Name = name;
            Role = role;
        }

        /// <summary>
        /// Checks if the user has permission to control a specific device
        /// </summary>
        /// <param name="deviceID">The ID of the device to check permissions for</param>
        /// <returns>True if the user has permission, false otherwise</returns>
        public bool HasDevicePermission(int deviceID)
        {
            // In a real implementation, this would check against a database of permissions
            // For now, we'll assume admins have access to all devices, and other roles have limited access
            
            if (Role == "Admin")
            {
                return true;
            }
            
            // For demo purposes, we'll say regular users can access devices with IDs < 1000
            // and guests can only access devices with IDs < 100
            if (Role == "Regular" && deviceID < 1000)
            {
                return true;
            }
            
            if (Role == "Guest" && deviceID < 100)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Checks if the user has admin privileges
        /// </summary>
        /// <returns>True if the user is an admin, false otherwise</returns>
        public bool IsAdmin()
        {
            return Role == "Admin";
        }

        /// <summary>
        /// Returns a string representation of the user
        /// </summary>
        /// <returns>A string containing the user's ID, name, and role</returns>
        public override string ToString()
        {
            return $"User {UserID}: {Name} ({Role})";
        }
    }
}
