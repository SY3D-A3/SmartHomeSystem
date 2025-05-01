using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SmartHomeSystemWinForms.Controllers;
using SmartHomeSystemWinForms.Models;
using SmartHomeSystemWinForms.Forms;
using SmartHomeSystemWinForms.Events;

namespace SmartHomeSystemWinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly SmartHomeController _controller;
        private bool _isInitialized = false;

        public MainForm()
        {
            InitializeComponent();
            
            // Create the controller
            _controller = new SmartHomeController();
            
            // Subscribe to controller events
            _controller.OnDeviceStatusChanged += Controller_OnDeviceStatusChanged;
            _controller.OnSensorReadingTaken += Controller_OnSensorReadingTaken;
            _controller.OnScheduleStatusChanged += Controller_OnScheduleStatusChanged;
            _controller.OnLogMessage += Controller_OnLogMessage;
            
            // Initialize the UI
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize the controller
            _controller.Initialize();
            
            // Populate the device list
            RefreshDeviceList();
            
            // Populate the sensor list
            RefreshSensorList();
            
            // Populate the schedule list
            RefreshScheduleList();
            
            // Start the controller
            _controller.Start();
            
            _isInitialized = true;
        }

        private void RefreshDeviceList()
        {
            // Clear the device list
            lstDevices.Items.Clear();
            
            // Get all devices
            var devices = _controller.GetAllDevices();
            
            // Add each device to the list
            foreach (var device in devices)
            {
                string status = device.GetStatus() ? "ON" : "OFF";
                string deviceType = device.GetType().Name;
                string deviceInfo = $"{deviceType} {device.DeviceID}: {device.Location} - {status}";
                
                // Add additional device-specific information
                if (device is Light light)
                {
                    deviceInfo += $" (Brightness: {light.Brightness}%, Color: {light.Color})";
                }
                else if (device is Thermostat thermostat)
                {
                    deviceInfo += $" (Current: {thermostat.CurrentTemperature}°C, Target: {thermostat.TargetTemperature}°C, Mode: {thermostat.Mode})";
                }
                else if (device is SecurityCamera camera)
                {
                    string recording = camera.IsRecording ? "Recording" : "Not Recording";
                    deviceInfo += $" ({recording}, {camera.Resolution}p)";
                }
                
                // Add the device to the list
                ListViewItem item = new ListViewItem(deviceInfo);
                item.Tag = device;
                lstDevices.Items.Add(item);
            }
        }

        private void RefreshSensorList()
        {
            // Clear the sensor list
            lstSensors.Items.Clear();
            
            // Get all sensors
            var sensors = _controller.GetAllSensors();
            
            // Add each sensor to the list
            foreach (var sensor in sensors)
            {
                string status = sensor.IsActive ? "Active" : "Inactive";
                string sensorType = sensor.GetType().Name;
                string sensorInfo = $"{sensorType} {sensor.SensorID}: {sensor.Location} - {status}";
                
                // Add additional sensor-specific information
                if (sensor is MotionSensor motionSensor)
                {
                    string motion = motionSensor.MotionDetected ? "Motion Detected" : "No Motion";
                    sensorInfo += $" ({motion}, Sensitivity: {motionSensor.Sensitivity})";
                }
                else if (sensor is TemperatureSensor tempSensor)
                {
                    sensorInfo += $" (Current: {tempSensor.CurrentTemperature}°C, Low: {tempSensor.LowTemperatureThreshold}°C, High: {tempSensor.HighTemperatureThreshold}°C)";
                }
                
                // Add the sensor to the list
                ListViewItem item = new ListViewItem(sensorInfo);
                item.Tag = sensor;
                lstSensors.Items.Add(item);
            }
        }

        private void RefreshScheduleList()
        {
            // Clear the schedule list
            lstSchedules.Items.Clear();
            
            // Get all schedules
            var schedules = _controller.GetAllSchedules();
            
            // Add each schedule to the list
            foreach (var schedule in schedules)
            {
                bool isActive = schedule.IsActiveAt(DateTime.Now);
                string status = isActive ? "Active" : "Inactive";
                string recurring = schedule.IsRecurring ? $"Recurring: {schedule.RecurrencePattern}" : "One-time";
                string scheduleInfo = $"Schedule {schedule.ScheduleID}: Device {schedule.DeviceID} - {schedule.Action} - {status}";
                scheduleInfo += $" ({schedule.StartTime.ToShortTimeString()} - {schedule.EndTime.ToShortTimeString()}, {recurring})";
                
                // Add the schedule to the list
                ListViewItem item = new ListViewItem(scheduleInfo);
                item.Tag = schedule;
                lstSchedules.Items.Add(item);
            }
        }

        private void Controller_OnDeviceStatusChanged(object sender, DeviceStatusChangedEventArgs e)
        {
            // Update the device list on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => RefreshDeviceList()));
            }
            else
            {
                RefreshDeviceList();
            }
        }

        private void Controller_OnSensorReadingTaken(object sender, SensorReadingEventArgs e)
        {
            // Update the sensor list on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => RefreshSensorList()));
            }
            else
            {
                RefreshSensorList();
            }
        }

        private void Controller_OnScheduleStatusChanged(object sender, ScheduleStatusChangedEventArgs e)
        {
            // Update the schedule list on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => RefreshScheduleList()));
            }
            else
            {
                RefreshScheduleList();
            }
        }

        private void Controller_OnLogMessage(object sender, string message)
        {
            // Add the message to the log on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddLogMessage(message)));
            }
            else
            {
                AddLogMessage(message);
            }
        }

        private void AddLogMessage(string message)
        {
            // Add the message to the log
            txtLog.AppendText($"[{DateTime.Now.ToLongTimeString()}] {message}{Environment.NewLine}");
            
            // Scroll to the bottom
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void btnTurnOn_Click(object sender, EventArgs e)
        {
            // Get the selected device
            if (lstDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a device to turn on.", "No Device Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected device
            SmartDevice device = (SmartDevice)lstDevices.SelectedItems[0].Tag;
            
            // Turn on the device
            device.TurnOn();
            
            // Refresh the device list
            RefreshDeviceList();
        }

        private void btnTurnOff_Click(object sender, EventArgs e)
        {
            // Get the selected device
            if (lstDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a device to turn off.", "No Device Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected device
            SmartDevice device = (SmartDevice)lstDevices.SelectedItems[0].Tag;
            
            // Turn off the device
            device.TurnOff();
            
            // Refresh the device list
            RefreshDeviceList();
        }

        private void btnSetBrightness_Click(object sender, EventArgs e)
        {
            // Get the selected device
            if (lstDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a light to set brightness.", "No Device Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected device
            SmartDevice device = (SmartDevice)lstDevices.SelectedItems[0].Tag;
            
            // Check if the device is a light
            if (!(device is Light light))
            {
                MessageBox.Show("The selected device is not a light.", "Invalid Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Prompt for brightness
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter brightness (0-100):", "Set Brightness", light.Brightness.ToString());
            
            // Parse the input
            if (int.TryParse(input, out int brightness) && brightness >= 0 && brightness <= 100)
            {
                // Set the brightness
                light.SetBrightness(brightness);
                
                // Refresh the device list
                RefreshDeviceList();
            }
            else
            {
                MessageBox.Show("Please enter a valid brightness value between 0 and 100.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSetTemperature_Click(object sender, EventArgs e)
        {
            // Get the selected device
            if (lstDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a thermostat to set temperature.", "No Device Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected device
            SmartDevice device = (SmartDevice)lstDevices.SelectedItems[0].Tag;
            
            // Check if the device is a thermostat
            if (!(device is Thermostat thermostat))
            {
                MessageBox.Show("The selected device is not a thermostat.", "Invalid Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Prompt for temperature
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter temperature (10-32):", "Set Temperature", thermostat.TargetTemperature.ToString());
            
            // Parse the input
            if (double.TryParse(input, out double temperature) && temperature >= 10 && temperature <= 32)
            {
                // Set the temperature
                thermostat.SetTargetTemperature(temperature);
                
                // Refresh the device list
                RefreshDeviceList();
            }
            else
            {
                MessageBox.Show("Please enter a valid temperature value between 10 and 32.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnVoiceCommand_Click(object sender, EventArgs e)
        {
            // Prompt for voice command
            string command = Microsoft.VisualBasic.Interaction.InputBox("Enter voice command:", "Voice Command", "");
            
            // Process the command
            if (!string.IsNullOrEmpty(command))
            {
                string result = _controller.ProcessVoiceCommand(command);
                
                // Show the result
                MessageBox.Show(result, "Voice Command Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // Open the add user form
            using (AddEntityForm form = new AddEntityForm(_controller, "User"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the UI
                    RefreshDeviceList();
                    RefreshSensorList();
                    RefreshScheduleList();
                    AddLogMessage("User added successfully.");
                }
            }
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            // Open the add room form
            using (AddEntityForm form = new AddEntityForm(_controller, "Room"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the UI
                    RefreshDeviceList();
                    RefreshSensorList();
                    RefreshScheduleList();
                    AddLogMessage("Room added successfully.");
                }
            }
        }

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            // Open the add device form
            using (AddEntityForm form = new AddEntityForm(_controller, "Device"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the UI
                    RefreshDeviceList();
                    AddLogMessage("Device added successfully.");
                }
            }
        }

        private void btnAddSensor_Click(object sender, EventArgs e)
        {
            // Open the add sensor form
            using (AddEntityForm form = new AddEntityForm(_controller, "Sensor"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the UI
                    RefreshSensorList();
                    AddLogMessage("Sensor added successfully.");
                }
            }
        }

        private void btnAddSchedule_Click(object sender, EventArgs e)
        {
            // Open the add schedule form
            using (AddEntityForm form = new AddEntityForm(_controller, "Schedule"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the UI
                    RefreshScheduleList();
                    AddLogMessage("Schedule added successfully.");
                }
            }
        }

        private void btnActivateSensor_Click(object sender, EventArgs e)
        {
            // Get the selected sensor
            if (lstSensors.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a sensor to activate.", "No Sensor Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected sensor
            Sensor sensor = (Sensor)lstSensors.SelectedItems[0].Tag;
            
            // Activate the sensor
            sensor.Activate();
            
            // Refresh the sensor list
            RefreshSensorList();
        }

        private void btnDeactivateSensor_Click(object sender, EventArgs e)
        {
            // Get the selected sensor
            if (lstSensors.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a sensor to deactivate.", "No Sensor Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected sensor
            Sensor sensor = (Sensor)lstSensors.SelectedItems[0].Tag;
            
            // Deactivate the sensor
            sensor.Deactivate();
            
            // Refresh the sensor list
            RefreshSensorList();
        }

        private void btnSimulateMotion_Click(object sender, EventArgs e)
        {
            // Get the selected sensor
            if (lstSensors.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a motion sensor to simulate motion.", "No Sensor Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected sensor
            Sensor sensor = (Sensor)lstSensors.SelectedItems[0].Tag;
            
            // Check if the sensor is a motion sensor
            if (!(sensor is MotionSensor motionSensor))
            {
                MessageBox.Show("The selected sensor is not a motion sensor.", "Invalid Sensor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Simulate motion
            motionSensor.DetectMotion(true);
            
            // Refresh the sensor list
            RefreshSensorList();
        }

        private void btnSimulateTemperature_Click(object sender, EventArgs e)
        {
            // Get the selected sensor
            if (lstSensors.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a temperature sensor to simulate temperature change.", "No Sensor Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get the selected sensor
            Sensor sensor = (Sensor)lstSensors.SelectedItems[0].Tag;
            
            // Check if the sensor is a temperature sensor
            if (!(sensor is TemperatureSensor tempSensor))
            {
                MessageBox.Show("The selected sensor is not a temperature sensor.", "Invalid Sensor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Prompt for temperature
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter temperature:", "Simulate Temperature", tempSensor.CurrentTemperature.ToString());
            
            // Parse the input
            if (double.TryParse(input, out double temperature))
            {
                // Update the temperature
                tempSensor.UpdateTemperature(temperature);
                
                // Refresh the sensor list
                RefreshSensorList();
            }
            else
            {
                MessageBox.Show("Please enter a valid temperature value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the controller
            if (_isInitialized)
            {
                _controller.Stop();
            }
        }
    }
}
