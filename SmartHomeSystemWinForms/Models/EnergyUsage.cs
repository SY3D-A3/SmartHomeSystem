using System;
using System.Collections.Generic;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents energy usage data for a device
    /// </summary>
    public class EnergyUsage
    {
        public int UsageID { get; set; }
        public int DeviceID { get; set; }
        public DateTime Timestamp { get; set; }
        public double PowerConsumption { get; set; } // in watts
        public double Duration { get; set; } // in minutes
        public double TotalEnergy { get; set; } // in watt-hours
        public double EstimatedCost { get; set; } // in currency units

        public EnergyUsage()
        {
            Timestamp = DateTime.Now;
        }

        public EnergyUsage(int deviceID, double powerConsumption, double duration)
        {
            DeviceID = deviceID;
            Timestamp = DateTime.Now;
            PowerConsumption = powerConsumption;
            Duration = duration;
            TotalEnergy = CalculateTotalEnergy();
            EstimatedCost = CalculateEstimatedCost();
        }

        /// <summary>
        /// Calculates the total energy consumption in watt-hours
        /// </summary>
        /// <returns>Total energy in watt-hours</returns>
        private double CalculateTotalEnergy()
        {
            // Energy (Wh) = Power (W) * Time (h)
            return PowerConsumption * (Duration / 60.0);
        }

        /// <summary>
        /// Calculates the estimated cost based on a default rate
        /// </summary>
        /// <param name="ratePerKWh">Cost per kilowatt-hour</param>
        /// <returns>Estimated cost</returns>
        private double CalculateEstimatedCost(double ratePerKWh = 0.15)
        {
            // Cost = Energy (kWh) * Rate (per kWh)
            return (TotalEnergy / 1000.0) * ratePerKWh;
        }

        /// <summary>
        /// Updates the cost calculation with a specific rate
        /// </summary>
        /// <param name="ratePerKWh">Cost per kilowatt-hour</param>
        public void UpdateCostCalculation(double ratePerKWh)
        {
            EstimatedCost = (TotalEnergy / 1000.0) * ratePerKWh;
        }

        /// <summary>
        /// Returns a string representation of the energy usage
        /// </summary>
        /// <returns>A string containing the energy usage details</returns>
        public override string ToString()
        {
            return $"Device {DeviceID} at {Timestamp}: {PowerConsumption}W for {Duration} minutes = {TotalEnergy:F2} Wh (${EstimatedCost:F2})";
        }
    }

    /// <summary>
    /// Represents a summary of energy usage for a device or room
    /// </summary>
    public class EnergyUsageSummary
    {
        public int EntityID { get; set; } // Device ID or Room ID
        public string EntityType { get; set; } // "Device" or "Room"
        public string EntityName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalEnergyUsage { get; set; } // in watt-hours
        public double TotalCost { get; set; }
        public List<EnergyUsage> DetailedUsages { get; set; }

        public EnergyUsageSummary()
        {
            DetailedUsages = new List<EnergyUsage>();
        }

        public EnergyUsageSummary(int entityID, string entityType, string entityName, DateTime startTime, DateTime endTime)
        {
            EntityID = entityID;
            EntityType = entityType;
            EntityName = entityName;
            StartTime = startTime;
            EndTime = endTime;
            TotalEnergyUsage = 0;
            TotalCost = 0;
            DetailedUsages = new List<EnergyUsage>();
        }

        /// <summary>
        /// Adds a usage record to the summary and updates totals
        /// </summary>
        /// <param name="usage">The usage record to add</param>
        public void AddUsage(EnergyUsage usage)
        {
            DetailedUsages.Add(usage);
            TotalEnergyUsage += usage.TotalEnergy;
            TotalCost += usage.EstimatedCost;
        }

        /// <summary>
        /// Gets the average power consumption over the period
        /// </summary>
        /// <returns>Average power in watts</returns>
        public double GetAveragePower()
        {
            if (DetailedUsages.Count == 0)
            {
                return 0;
            }

            double totalPower = 0;
            foreach (var usage in DetailedUsages)
            {
                totalPower += usage.PowerConsumption;
            }

            return totalPower / DetailedUsages.Count;
        }

        /// <summary>
        /// Gets the peak power consumption over the period
        /// </summary>
        /// <returns>Peak power in watts</returns>
        public double GetPeakPower()
        {
            if (DetailedUsages.Count == 0)
            {
                return 0;
            }

            double peakPower = 0;
            foreach (var usage in DetailedUsages)
            {
                if (usage.PowerConsumption > peakPower)
                {
                    peakPower = usage.PowerConsumption;
                }
            }

            return peakPower;
        }

        /// <summary>
        /// Returns a string representation of the energy usage summary
        /// </summary>
        /// <returns>A string containing the summary details</returns>
        public override string ToString()
        {
            return $"{EntityType} {EntityName} ({StartTime:d} to {EndTime:d}): {TotalEnergyUsage:F2} Wh (${TotalCost:F2})";
        }
    }
}
