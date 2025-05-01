using System;
using System.Collections.Generic;
using SmartHomeSystemWinForms.Models;

namespace SmartHomeSystemWinForms.Services
{
    /// <summary>
    /// Interface for advanced database services in the smart home system
    /// </summary>
    public interface IAdvancedDatabaseService : IDatabaseService
    {
        #region Automation Rule Methods
        void AddAutomationRule(AutomationRule rule);
        AutomationRule GetAutomationRule(int ruleID);
        List<AutomationRule> GetAllAutomationRules();
        List<AutomationRule> GetAutomationRulesForEntity(string entityType, int entityID);
        void UpdateAutomationRuleStatus(int ruleID, bool isEnabled);
        void DeleteAutomationRule(int ruleID);
        #endregion

        #region Scene Methods
        void AddScene(Scene scene);
        Scene GetScene(int sceneID);
        List<Scene> GetAllScenes();
        List<Scene> GetFavoriteScenes();
        void UpdateSceneFavoriteStatus(int sceneID, bool isFavorite);
        void DeleteScene(int sceneID);
        #endregion

        #region Energy Usage Methods
        void RecordEnergyUsage(EnergyUsage usage);
        List<EnergyUsage> GetEnergyUsageForDevice(int deviceID, DateTime startTime, DateTime endTime);
        EnergyUsageSummary GetEnergyUsageSummaryForDevice(int deviceID, DateTime startTime, DateTime endTime);
        EnergyUsageSummary GetEnergyUsageSummaryForRoom(int roomID, DateTime startTime, DateTime endTime);
        double GetTotalEnergyUsage(DateTime startTime, DateTime endTime);
        double GetTotalEnergyCost(DateTime startTime, DateTime endTime);
        #endregion

        #region Advanced Query Methods
        List<SensorReading> GetSensorReadingsWithFilter(
            int? sensorID = null,
            string sensorType = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            bool? isAnomalous = null,
            int maxResults = 100);

        List<SmartDevice> GetDevicesWithFilter(
            string deviceType = null,
            bool? status = null,
            string location = null,
            int? roomID = null);

        List<AutomationRule> GetAutomationRulesWithFilter(
            string triggerType = null,
            int? triggerEntityID = null,
            bool? isEnabled = null,
            DateTime? createdAfter = null,
            int minTriggerCount = 0);

        List<Scene> GetScenesWithFilter(
            bool? isFavorite = null,
            DateTime? activatedAfter = null,
            int minActivationCount = 0);
        #endregion

        #region Statistics Methods
        Dictionary<string, int> GetDeviceCountByType();
        Dictionary<string, int> GetSensorCountByType();
        Dictionary<DateTime, int> GetDeviceActivationsByDay(DateTime startDate, DateTime endDate);
        Dictionary<int, double> GetEnergyUsageByRoom(DateTime startTime, DateTime endTime);
        List<KeyValuePair<int, int>> GetMostActiveDevices(int count = 10);
        List<KeyValuePair<int, int>> GetMostTriggeredRules(int count = 10);
        #endregion

        #region Data Management Methods
        void BackupDatabase(string filePath);
        void RestoreDatabase(string filePath);
        void PurgeSensorReadings(DateTime olderThan);
        void PurgeEnergyUsageData(DateTime olderThan);
        void OptimizeDatabase();
        #endregion
    }
}
