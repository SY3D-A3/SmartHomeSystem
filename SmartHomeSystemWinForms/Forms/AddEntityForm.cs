using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SmartHomeSystemWinForms.Controllers;
using SmartHomeSystemWinForms.Models;

namespace SmartHomeSystemWinForms.Forms
{
    public partial class AddEntityForm : Form
    {
        private readonly SmartHomeController _controller;
        private readonly string _entityType;

        public AddEntityForm(SmartHomeController controller, string entityType)
        {
            InitializeComponent();
            _controller = controller;
            _entityType = entityType;

            // Set the form title based on the entity type
            this.Text = $"Add {_entityType}";

            // Initialize the form based on the entity type
            InitializeFormForEntityType();
        }

        private void InitializeFormForEntityType()
        {
            // Clear any existing controls
            panelFields.Controls.Clear();

            // Add fields based on the entity type
            switch (_entityType)
            {
                case "User":
                    AddUserFields();
                    break;
                case "Room":
                    AddRoomFields();
                    break;
                case "Device":
                    AddDeviceFields();
                    break;
                case "Sensor":
                    AddSensorFields();
                    break;
                case "Schedule":
                    AddScheduleFields();
                    break;
                default:
                    MessageBox.Show($"Unknown entity type: {_entityType}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    break;
            }
        }

        private void AddUserFields()
        {
            // User ID
            AddField("User ID:", "txtUserID", TextBoxType.Number);
            
            // Name
            AddField("Name:", "txtName", TextBoxType.Text);
            
            // Role
            AddField("Role:", "cboRole", TextBoxType.ComboBox, new[] { "Admin", "Regular", "Guest" });
        }

        private void AddRoomFields()
        {
            // Room ID
            AddField("Room ID:", "txtRoomID", TextBoxType.Number);
            
            // Name
            AddField("Name:", "txtName", TextBoxType.Text);
        }

        private void AddDeviceFields()
        {
            // Device ID
            AddField("Device ID:", "txtDeviceID", TextBoxType.Number);
            
            // Device Type
            AddField("Device Type:", "cboDeviceType", TextBoxType.ComboBox, new[] { "Light", "Thermostat", "SecurityCamera" });
            
            // Location
            AddField("Location:", "txtLocation", TextBoxType.Text);
            
            // Room ID
            AddField("Room ID:", "cboRoomID", TextBoxType.ComboBox, GetRoomOptions());
            
            // Device Name (for voice commands)
            AddField("Device Name (for voice commands):", "txtDeviceName", TextBoxType.Text);
            
            // Device-specific fields (will be shown/hidden based on device type)
            AddDeviceSpecificFields();
            
            // Add event handler for device type change
            ComboBox cboDeviceType = (ComboBox)panelFields.Controls["cboDeviceType"];
            cboDeviceType.SelectedIndexChanged += DeviceTypeChanged;
        }

        private void AddDeviceSpecificFields()
        {
            // Light-specific fields
            AddField("Brightness (0-100):", "txtBrightness", TextBoxType.Number, null, false);
            AddField("Color:", "txtColor", TextBoxType.Text, null, false);
            
            // Thermostat-specific fields
            AddField("Current Temperature (°C):", "txtCurrentTemp", TextBoxType.Number, null, false);
            AddField("Target Temperature (°C):", "txtTargetTemp", TextBoxType.Number, null, false);
            AddField("Mode:", "cboMode", TextBoxType.ComboBox, new[] { "Heat", "Cool", "Auto", "Off" }, false);
            
            // SecurityCamera-specific fields
            AddField("Resolution (p):", "txtResolution", TextBoxType.Number, null, false);
            AddField("Motion Detection:", "chkMotionDetection", TextBoxType.CheckBox, null, false);
        }

        private void DeviceTypeChanged(object sender, EventArgs e)
        {
            ComboBox cboDeviceType = (ComboBox)sender;
            string deviceType = cboDeviceType.SelectedItem?.ToString();
            
            // Hide all device-specific fields
            panelFields.Controls["txtBrightness"].Visible = false;
            panelFields.Controls["lbltxtBrightness"].Visible = false;
            panelFields.Controls["txtColor"].Visible = false;
            panelFields.Controls["lbltxtColor"].Visible = false;
            
            panelFields.Controls["txtCurrentTemp"].Visible = false;
            panelFields.Controls["lbltxtCurrentTemp"].Visible = false;
            panelFields.Controls["txtTargetTemp"].Visible = false;
            panelFields.Controls["lbltxtTargetTemp"].Visible = false;
            panelFields.Controls["cboMode"].Visible = false;
            panelFields.Controls["lblcboMode"].Visible = false;
            
            panelFields.Controls["txtResolution"].Visible = false;
            panelFields.Controls["lbltxtResolution"].Visible = false;
            panelFields.Controls["chkMotionDetection"].Visible = false;
            panelFields.Controls["lblchkMotionDetection"].Visible = false;
            
            // Show fields based on device type
            switch (deviceType)
            {
                case "Light":
                    panelFields.Controls["txtBrightness"].Visible = true;
                    panelFields.Controls["lbltxtBrightness"].Visible = true;
                    panelFields.Controls["txtColor"].Visible = true;
                    panelFields.Controls["lbltxtColor"].Visible = true;
                    break;
                
                case "Thermostat":
                    panelFields.Controls["txtCurrentTemp"].Visible = true;
                    panelFields.Controls["lbltxtCurrentTemp"].Visible = true;
                    panelFields.Controls["txtTargetTemp"].Visible = true;
                    panelFields.Controls["lbltxtTargetTemp"].Visible = true;
                    panelFields.Controls["cboMode"].Visible = true;
                    panelFields.Controls["lblcboMode"].Visible = true;
                    break;
                
                case "SecurityCamera":
                    panelFields.Controls["txtResolution"].Visible = true;
                    panelFields.Controls["lbltxtResolution"].Visible = true;
                    panelFields.Controls["chkMotionDetection"].Visible = true;
                    panelFields.Controls["lblchkMotionDetection"].Visible = true;
                    break;
            }
        }

        private void AddSensorFields()
        {
            // Sensor ID
            AddField("Sensor ID:", "txtSensorID", TextBoxType.Number);
            
            // Sensor Type
            AddField("Sensor Type:", "cboSensorType", TextBoxType.ComboBox, new[] { "MotionSensor", "TemperatureSensor" });
            
            // Location
            AddField("Location:", "txtLocation", TextBoxType.Text);
            
            // Room ID
            AddField("Room ID:", "cboRoomID", TextBoxType.ComboBox, GetRoomOptions());
            
            // Sensor-specific fields (will be shown/hidden based on sensor type)
            AddSensorSpecificFields();
            
            // Add event handler for sensor type change
            ComboBox cboSensorType = (ComboBox)panelFields.Controls["cboSensorType"];
            cboSensorType.SelectedIndexChanged += SensorTypeChanged;
        }

        private void AddSensorSpecificFields()
        {
            // MotionSensor-specific fields
            AddField("Sensitivity (1-10):", "txtSensitivity", TextBoxType.Number, null, false);
            
            // TemperatureSensor-specific fields
            AddField("Current Temperature (°C):", "txtCurrentTemp", TextBoxType.Number, null, false);
            AddField("Low Temperature Threshold (°C):", "txtLowTemp", TextBoxType.Number, null, false);
            AddField("High Temperature Threshold (°C):", "txtHighTemp", TextBoxType.Number, null, false);
        }

        private void SensorTypeChanged(object sender, EventArgs e)
        {
            ComboBox cboSensorType = (ComboBox)sender;
            string sensorType = cboSensorType.SelectedItem?.ToString();
            
            // Hide all sensor-specific fields
            panelFields.Controls["txtSensitivity"].Visible = false;
            panelFields.Controls["lbltxtSensitivity"].Visible = false;
            
            panelFields.Controls["txtCurrentTemp"].Visible = false;
            panelFields.Controls["lbltxtCurrentTemp"].Visible = false;
            panelFields.Controls["txtLowTemp"].Visible = false;
            panelFields.Controls["lbltxtLowTemp"].Visible = false;
            panelFields.Controls["txtHighTemp"].Visible = false;
            panelFields.Controls["lbltxtHighTemp"].Visible = false;
            
            // Show fields based on sensor type
            switch (sensorType)
            {
                case "MotionSensor":
                    panelFields.Controls["txtSensitivity"].Visible = true;
                    panelFields.Controls["lbltxtSensitivity"].Visible = true;
                    break;
                
                case "TemperatureSensor":
                    panelFields.Controls["txtCurrentTemp"].Visible = true;
                    panelFields.Controls["lbltxtCurrentTemp"].Visible = true;
                    panelFields.Controls["txtLowTemp"].Visible = true;
                    panelFields.Controls["lbltxtLowTemp"].Visible = true;
                    panelFields.Controls["txtHighTemp"].Visible = true;
                    panelFields.Controls["lbltxtHighTemp"].Visible = true;
                    break;
            }
        }

        private void AddScheduleFields()
        {
            // Schedule ID
            AddField("Schedule ID:", "txtScheduleID", TextBoxType.Number);
            
            // Device ID
            AddField("Device ID:", "cboDeviceID", TextBoxType.ComboBox, GetDeviceOptions());
            
            // Start Time
            AddField("Start Time:", "dtpStartTime", TextBoxType.DateTime);
            
            // End Time
            AddField("End Time:", "dtpEndTime", TextBoxType.DateTime);
            
            // Action
            AddField("Action:", "cboAction", TextBoxType.ComboBox, new[] { "TurnOn", "TurnOff", "SetBrightness", "SetTemperature", "SetColor" });
            
            // Action Parameter
            AddField("Action Parameter:", "txtActionParam", TextBoxType.Text);
            
            // Is Recurring
            AddField("Is Recurring:", "chkRecurring", TextBoxType.CheckBox);
            
            // Recurrence Pattern
            AddField("Recurrence Pattern:", "cboRecurrencePattern", TextBoxType.ComboBox, new[] { "Daily", "Weekdays", "Weekends", "Weekly", "Monthly" }, false);
            
            // Add event handler for recurring checkbox
            CheckBox chkRecurring = (CheckBox)panelFields.Controls["chkRecurring"];
            chkRecurring.CheckedChanged += RecurringCheckedChanged;
            
            // Add event handler for action change
            ComboBox cboAction = (ComboBox)panelFields.Controls["cboAction"];
            cboAction.SelectedIndexChanged += ActionChanged;
        }

        private void RecurringCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkRecurring = (CheckBox)sender;
            
            // Show/hide recurrence pattern based on checkbox
            panelFields.Controls["cboRecurrencePattern"].Visible = chkRecurring.Checked;
            panelFields.Controls["lblcboRecurrencePattern"].Visible = chkRecurring.Checked;
        }

        private void ActionChanged(object sender, EventArgs e)
        {
            ComboBox cboAction = (ComboBox)sender;
            string action = cboAction.SelectedItem?.ToString();
            
            // Show/hide action parameter based on action
            bool showParam = action == "SetBrightness" || action == "SetTemperature" || action == "SetColor";
            panelFields.Controls["txtActionParam"].Visible = showParam;
            panelFields.Controls["lbltxtActionParam"].Visible = showParam;
            
            // Update parameter label based on action
            Label lblParam = (Label)panelFields.Controls["lbltxtActionParam"];
            switch (action)
            {
                case "SetBrightness":
                    lblParam.Text = "Brightness (0-100):";
                    break;
                case "SetTemperature":
                    lblParam.Text = "Temperature (°C):";
                    break;
                case "SetColor":
                    lblParam.Text = "Color:";
                    break;
                default:
                    lblParam.Text = "Action Parameter:";
                    break;
            }
        }

        private string[] GetRoomOptions()
        {
            // Get all rooms from the controller
            List<Room> rooms = _controller.GetAllRooms();
            
            // Create options array
            string[] options = new string[rooms.Count];
            for (int i = 0; i < rooms.Count; i++)
            {
                options[i] = $"{rooms[i].RoomID}: {rooms[i].Name}";
            }
            
            return options;
        }

        private string[] GetDeviceOptions()
        {
            // Get all devices from the controller
            List<SmartDevice> devices = _controller.GetAllDevices();
            
            // Create options array
            string[] options = new string[devices.Count];
            for (int i = 0; i < devices.Count; i++)
            {
                options[i] = $"{devices[i].DeviceID}: {devices[i].GetType().Name} ({devices[i].Location})";
            }
            
            return options;
        }

        private enum TextBoxType
        {
            Text,
            Number,
            ComboBox,
            CheckBox,
            DateTime
        }

        private void AddField(string label, string name, TextBoxType type, string[] options = null, bool visible = true)
        {
            // Calculate the position for the new field
            int fieldCount = panelFields.Controls.Count / 2;
            int yPos = fieldCount * 30;
            
            // Create the label
            Label lbl = new Label();
            lbl.Name = "lbl" + name;
            lbl.Text = label;
            lbl.Location = new Point(10, yPos + 3);
            lbl.AutoSize = true;
            lbl.Visible = visible;
            panelFields.Controls.Add(lbl);
            
            // Create the input control based on type
            Control control = null;
            
            switch (type)
            {
                case TextBoxType.Text:
                case TextBoxType.Number:
                    TextBox txt = new TextBox();
                    txt.Name = name;
                    txt.Location = new Point(200, yPos);
                    txt.Width = 200;
                    control = txt;
                    break;
                
                case TextBoxType.ComboBox:
                    ComboBox cbo = new ComboBox();
                    cbo.Name = name;
                    cbo.Location = new Point(200, yPos);
                    cbo.Width = 200;
                    cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                    
                    if (options != null)
                    {
                        cbo.Items.AddRange(options);
                        if (cbo.Items.Count > 0)
                        {
                            cbo.SelectedIndex = 0;
                        }
                    }
                    
                    control = cbo;
                    break;
                
                case TextBoxType.CheckBox:
                    CheckBox chk = new CheckBox();
                    chk.Name = name;
                    chk.Location = new Point(200, yPos);
                    chk.Width = 200;
                    control = chk;
                    break;
                
                case TextBoxType.DateTime:
                    DateTimePicker dtp = new DateTimePicker();
                    dtp.Name = name;
                    dtp.Location = new Point(200, yPos);
                    dtp.Width = 200;
                    dtp.Format = DateTimePickerFormat.Time;
                    dtp.ShowUpDown = true;
                    control = dtp;
                    break;
            }
            
            if (control != null)
            {
                control.Visible = visible;
                panelFields.Controls.Add(control);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Save the entity based on the entity type
                switch (_entityType)
                {
                    case "User":
                        SaveUser();
                        break;
                    case "Room":
                        SaveRoom();
                        break;
                    case "Device":
                        SaveDevice();
                        break;
                    case "Sensor":
                        SaveSensor();
                        break;
                    case "Schedule":
                        SaveSchedule();
                        break;
                }
                
                // Close the form
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving {_entityType}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveUser()
        {
            // Get the values from the form
            int userID = int.Parse(((TextBox)panelFields.Controls["txtUserID"]).Text);
            string name = ((TextBox)panelFields.Controls["txtName"]).Text;
            string role = ((ComboBox)panelFields.Controls["cboRole"]).SelectedItem.ToString();
            
            // Create the user
            User user = new User(userID, name, role);
            
            // Add the user to the database
            _controller.AddUser(user);
        }

        private void SaveRoom()
        {
            // Get the values from the form
            int roomID = int.Parse(((TextBox)panelFields.Controls["txtRoomID"]).Text);
            string name = ((TextBox)panelFields.Controls["txtName"]).Text;
            
            // Create the room
            Room room = new Room(roomID, name);
            
            // Add the room to the database
            _controller.AddRoom(room);
        }

        private void SaveDevice()
        {
            // Get the common values from the form
            int deviceID = int.Parse(((TextBox)panelFields.Controls["txtDeviceID"]).Text);
            string deviceType = ((ComboBox)panelFields.Controls["cboDeviceType"]).SelectedItem.ToString();
            string location = ((TextBox)panelFields.Controls["txtLocation"]).Text;
            
            // Get the room ID
            string roomOption = ((ComboBox)panelFields.Controls["cboRoomID"]).SelectedItem.ToString();
            int roomID = int.Parse(roomOption.Split(':')[0]);
            
            // Get the device name for voice commands
            string deviceName = ((TextBox)panelFields.Controls["txtDeviceName"]).Text;
            
            // Create the device based on type
            SmartDevice device = null;
            
            switch (deviceType)
            {
                case "Light":
                    int brightness = int.Parse(((TextBox)panelFields.Controls["txtBrightness"]).Text);
                    string color = ((TextBox)panelFields.Controls["txtColor"]).Text;
                    
                    Light light = new Light(deviceID, location, color);
                    light.SetBrightness(brightness);
                    device = light;
                    break;
                
                case "Thermostat":
                    double currentTemp = double.Parse(((TextBox)panelFields.Controls["txtCurrentTemp"]).Text);
                    double targetTemp = double.Parse(((TextBox)panelFields.Controls["txtTargetTemp"]).Text);
                    string mode = ((ComboBox)panelFields.Controls["cboMode"]).SelectedItem.ToString();
                    
                    Thermostat thermostat = new Thermostat(deviceID, location, currentTemp);
                    thermostat.SetTargetTemperature(targetTemp);
                    thermostat.SetMode(mode);
                    device = thermostat;
                    break;
                
                case "SecurityCamera":
                    int resolution = int.Parse(((TextBox)panelFields.Controls["txtResolution"]).Text);
                    bool motionDetection = ((CheckBox)panelFields.Controls["chkMotionDetection"]).Checked;
                    
                    SecurityCamera camera = new SecurityCamera(deviceID, location, resolution);
                    if (motionDetection)
                    {
                        camera.EnableMotionDetection();
                    }
                    device = camera;
                    break;
            }
            
            // Add the device to the database
            if (device != null)
            {
                _controller.AddDevice(device, roomID, deviceName);
            }
        }

        private void SaveSensor()
        {
            // Get the common values from the form
            int sensorID = int.Parse(((TextBox)panelFields.Controls["txtSensorID"]).Text);
            string sensorType = ((ComboBox)panelFields.Controls["cboSensorType"]).SelectedItem.ToString();
            string location = ((TextBox)panelFields.Controls["txtLocation"]).Text;
            
            // Get the room ID
            string roomOption = ((ComboBox)panelFields.Controls["cboRoomID"]).SelectedItem.ToString();
            int roomID = int.Parse(roomOption.Split(':')[0]);
            
            // Create the sensor based on type
            Sensor sensor = null;
            
            switch (sensorType)
            {
                case "MotionSensor":
                    int sensitivity = int.Parse(((TextBox)panelFields.Controls["txtSensitivity"]).Text);
                    
                    MotionSensor motionSensor = new MotionSensor(sensorID, location, sensitivity);
                    sensor = motionSensor;
                    break;
                
                case "TemperatureSensor":
                    double currentTemp = double.Parse(((TextBox)panelFields.Controls["txtCurrentTemp"]).Text);
                    double lowTemp = double.Parse(((TextBox)panelFields.Controls["txtLowTemp"]).Text);
                    double highTemp = double.Parse(((TextBox)panelFields.Controls["txtHighTemp"]).Text);
                    
                    TemperatureSensor tempSensor = new TemperatureSensor(sensorID, location, currentTemp);
                    tempSensor.SetLowTemperatureThreshold(lowTemp);
                    tempSensor.SetHighTemperatureThreshold(highTemp);
                    sensor = tempSensor;
                    break;
            }
            
            // Add the sensor to the database
            if (sensor != null)
            {
                _controller.AddSensor(sensor, roomID);
            }
        }

        private void SaveSchedule()
        {
            // Get the values from the form
            int scheduleID = int.Parse(((TextBox)panelFields.Controls["txtScheduleID"]).Text);
            
            // Get the device ID
            string deviceOption = ((ComboBox)panelFields.Controls["cboDeviceID"]).SelectedItem.ToString();
            int deviceID = int.Parse(deviceOption.Split(':')[0]);
            
            // Get the start and end times
            DateTime startTime = ((DateTimePicker)panelFields.Controls["dtpStartTime"]).Value;
            DateTime endTime = ((DateTimePicker)panelFields.Controls["dtpEndTime"]).Value;
            
            // Get the action
            string action = ((ComboBox)panelFields.Controls["cboAction"]).SelectedItem.ToString();
            
            // Get the action parameter if applicable
            string actionParam = ((TextBox)panelFields.Controls["txtActionParam"]).Text;
            if (!string.IsNullOrEmpty(actionParam))
            {
                action += ":" + actionParam;
            }
            
            // Create the schedule
            Schedule schedule = new Schedule(scheduleID, startTime, endTime, action, deviceID);
            
            // Set recurrence if applicable
            bool isRecurring = ((CheckBox)panelFields.Controls["chkRecurring"]).Checked;
            if (isRecurring)
            {
                string pattern = ((ComboBox)panelFields.Controls["cboRecurrencePattern"]).SelectedItem.ToString();
                schedule.SetRecurring(pattern);
            }
            
            // Add the schedule to the database
            _controller.AddSchedule(schedule);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Close the form without saving
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
