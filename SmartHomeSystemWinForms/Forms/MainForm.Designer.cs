namespace SmartHomeSystemWinForms.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDevices = new System.Windows.Forms.TabPage();
            this.btnSetTemperature = new System.Windows.Forms.Button();
            this.btnSetBrightness = new System.Windows.Forms.Button();
            this.btnTurnOff = new System.Windows.Forms.Button();
            this.btnTurnOn = new System.Windows.Forms.Button();
            this.lstDevices = new System.Windows.Forms.ListView();
            this.tabSensors = new System.Windows.Forms.TabPage();
            this.btnSimulateTemperature = new System.Windows.Forms.Button();
            this.btnSimulateMotion = new System.Windows.Forms.Button();
            this.btnDeactivateSensor = new System.Windows.Forms.Button();
            this.btnActivateSensor = new System.Windows.Forms.Button();
            this.lstSensors = new System.Windows.Forms.ListView();
            this.tabSchedules = new System.Windows.Forms.TabPage();
            this.lstSchedules = new System.Windows.Forms.ListView();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnVoiceCommand = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabDevices.SuspendLayout();
            this.tabSensors.SuspendLayout();
            this.tabSchedules.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabDevices);
            this.tabControl.Controls.Add(this.tabSensors);
            this.tabControl.Controls.Add(this.tabSchedules);
            this.tabControl.Controls.Add(this.tabLog);
            this.tabControl.Location = new System.Drawing.Point(12, 41);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(776, 397);
            this.tabControl.TabIndex = 0;
            // 
            // tabDevices
            // 
            this.tabDevices.Controls.Add(this.btnSetTemperature);
            this.tabDevices.Controls.Add(this.btnSetBrightness);
            this.tabDevices.Controls.Add(this.btnTurnOff);
            this.tabDevices.Controls.Add(this.btnTurnOn);
            this.tabDevices.Controls.Add(this.lstDevices);
            this.tabDevices.Location = new System.Drawing.Point(4, 22);
            this.tabDevices.Name = "tabDevices";
            this.tabDevices.Padding = new System.Windows.Forms.Padding(3);
            this.tabDevices.Size = new System.Drawing.Size(768, 371);
            this.tabDevices.TabIndex = 0;
            this.tabDevices.Text = "Devices";
            this.tabDevices.UseVisualStyleBackColor = true;
            // 
            // btnSetTemperature
            // 
            this.btnSetTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetTemperature.Location = new System.Drawing.Point(249, 342);
            this.btnSetTemperature.Name = "btnSetTemperature";
            this.btnSetTemperature.Size = new System.Drawing.Size(75, 23);
            this.btnSetTemperature.TabIndex = 4;
            this.btnSetTemperature.Text = "Set Temp";
            this.btnSetTemperature.UseVisualStyleBackColor = true;
            this.btnSetTemperature.Click += new System.EventHandler(this.btnSetTemperature_Click);
            // 
            // btnSetBrightness
            // 
            this.btnSetBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetBrightness.Location = new System.Drawing.Point(168, 342);
            this.btnSetBrightness.Name = "btnSetBrightness";
            this.btnSetBrightness.Size = new System.Drawing.Size(75, 23);
            this.btnSetBrightness.TabIndex = 3;
            this.btnSetBrightness.Text = "Set Brightness";
            this.btnSetBrightness.UseVisualStyleBackColor = true;
            this.btnSetBrightness.Click += new System.EventHandler(this.btnSetBrightness_Click);
            // 
            // btnTurnOff
            // 
            this.btnTurnOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTurnOff.Location = new System.Drawing.Point(87, 342);
            this.btnTurnOff.Name = "btnTurnOff";
            this.btnTurnOff.Size = new System.Drawing.Size(75, 23);
            this.btnTurnOff.TabIndex = 2;
            this.btnTurnOff.Text = "Turn Off";
            this.btnTurnOff.UseVisualStyleBackColor = true;
            this.btnTurnOff.Click += new System.EventHandler(this.btnTurnOff_Click);
            // 
            // btnTurnOn
            // 
            this.btnTurnOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTurnOn.Location = new System.Drawing.Point(6, 342);
            this.btnTurnOn.Name = "btnTurnOn";
            this.btnTurnOn.Size = new System.Drawing.Size(75, 23);
            this.btnTurnOn.TabIndex = 1;
            this.btnTurnOn.Text = "Turn On";
            this.btnTurnOn.UseVisualStyleBackColor = true;
            this.btnTurnOn.Click += new System.EventHandler(this.btnTurnOn_Click);
            // 
            // lstDevices
            // 
            this.lstDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDevices.HideSelection = false;
            this.lstDevices.Location = new System.Drawing.Point(6, 6);
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(756, 330);
            this.lstDevices.TabIndex = 0;
            this.lstDevices.UseCompatibleStateImageBehavior = false;
            this.lstDevices.View = System.Windows.Forms.View.List;
            // 
            // tabSensors
            // 
            this.tabSensors.Controls.Add(this.btnSimulateTemperature);
            this.tabSensors.Controls.Add(this.btnSimulateMotion);
            this.tabSensors.Controls.Add(this.btnDeactivateSensor);
            this.tabSensors.Controls.Add(this.btnActivateSensor);
            this.tabSensors.Controls.Add(this.lstSensors);
            this.tabSensors.Location = new System.Drawing.Point(4, 22);
            this.tabSensors.Name = "tabSensors";
            this.tabSensors.Padding = new System.Windows.Forms.Padding(3);
            this.tabSensors.Size = new System.Drawing.Size(768, 371);
            this.tabSensors.TabIndex = 1;
            this.tabSensors.Text = "Sensors";
            this.tabSensors.UseVisualStyleBackColor = true;
            // 
            // btnSimulateTemperature
            // 
            this.btnSimulateTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSimulateTemperature.Location = new System.Drawing.Point(249, 342);
            this.btnSimulateTemperature.Name = "btnSimulateTemperature";
            this.btnSimulateTemperature.Size = new System.Drawing.Size(75, 23);
            this.btnSimulateTemperature.TabIndex = 4;
            this.btnSimulateTemperature.Text = "Sim Temp";
            this.btnSimulateTemperature.UseVisualStyleBackColor = true;
            this.btnSimulateTemperature.Click += new System.EventHandler(this.btnSimulateTemperature_Click);
            // 
            // btnSimulateMotion
            // 
            this.btnSimulateMotion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSimulateMotion.Location = new System.Drawing.Point(168, 342);
            this.btnSimulateMotion.Name = "btnSimulateMotion";
            this.btnSimulateMotion.Size = new System.Drawing.Size(75, 23);
            this.btnSimulateMotion.TabIndex = 3;
            this.btnSimulateMotion.Text = "Sim Motion";
            this.btnSimulateMotion.UseVisualStyleBackColor = true;
            this.btnSimulateMotion.Click += new System.EventHandler(this.btnSimulateMotion_Click);
            // 
            // btnDeactivateSensor
            // 
            this.btnDeactivateSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeactivateSensor.Location = new System.Drawing.Point(87, 342);
            this.btnDeactivateSensor.Name = "btnDeactivateSensor";
            this.btnDeactivateSensor.Size = new System.Drawing.Size(75, 23);
            this.btnDeactivateSensor.TabIndex = 2;
            this.btnDeactivateSensor.Text = "Deactivate";
            this.btnDeactivateSensor.UseVisualStyleBackColor = true;
            this.btnDeactivateSensor.Click += new System.EventHandler(this.btnDeactivateSensor_Click);
            // 
            // btnActivateSensor
            // 
            this.btnActivateSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnActivateSensor.Location = new System.Drawing.Point(6, 342);
            this.btnActivateSensor.Name = "btnActivateSensor";
            this.btnActivateSensor.Size = new System.Drawing.Size(75, 23);
            this.btnActivateSensor.TabIndex = 1;
            this.btnActivateSensor.Text = "Activate";
            this.btnActivateSensor.UseVisualStyleBackColor = true;
            this.btnActivateSensor.Click += new System.EventHandler(this.btnActivateSensor_Click);
            // 
            // lstSensors
            // 
            this.lstSensors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSensors.HideSelection = false;
            this.lstSensors.Location = new System.Drawing.Point(6, 6);
            this.lstSensors.Name = "lstSensors";
            this.lstSensors.Size = new System.Drawing.Size(756, 330);
            this.lstSensors.TabIndex = 0;
            this.lstSensors.UseCompatibleStateImageBehavior = false;
            this.lstSensors.View = System.Windows.Forms.View.List;
            // 
            // tabSchedules
            // 
            this.tabSchedules.Controls.Add(this.lstSchedules);
            this.tabSchedules.Location = new System.Drawing.Point(4, 22);
            this.tabSchedules.Name = "tabSchedules";
            this.tabSchedules.Padding = new System.Windows.Forms.Padding(3);
            this.tabSchedules.Size = new System.Drawing.Size(768, 371);
            this.tabSchedules.TabIndex = 2;
            this.tabSchedules.Text = "Schedules";
            this.tabSchedules.UseVisualStyleBackColor = true;
            // 
            // lstSchedules
            // 
            this.lstSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSchedules.HideSelection = false;
            this.lstSchedules.Location = new System.Drawing.Point(6, 6);
            this.lstSchedules.Name = "lstSchedules";
            this.lstSchedules.Size = new System.Drawing.Size(756, 330);
            this.lstSchedules.TabIndex = 0;
            this.lstSchedules.UseCompatibleStateImageBehavior = false;
            this.lstSchedules.View = System.Windows.Forms.View.List;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(768, 371);
            this.tabLog.TabIndex = 3;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(6, 6);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(756, 359);
            this.txtLog.TabIndex = 0;
            // 
            // 
            // btnVoiceCommand
            // 
            this.btnVoiceCommand.Location = new System.Drawing.Point(12, 12);
            this.btnVoiceCommand.Name = "btnVoiceCommand";
            this.btnVoiceCommand.Size = new System.Drawing.Size(100, 23);
            this.btnVoiceCommand.TabIndex = 1;
            this.btnVoiceCommand.Text = "Voice Command";
            this.btnVoiceCommand.UseVisualStyleBackColor = true;
            this.btnVoiceCommand.Click += new System.EventHandler(this.btnVoiceCommand_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnVoiceCommand);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Smart Home Automation System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabDevices.ResumeLayout(false);
            this.tabSensors.ResumeLayout(false);
            this.tabSchedules.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDevices;
        private System.Windows.Forms.TabPage tabSensors;
        private System.Windows.Forms.TabPage tabSchedules;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.ListView lstDevices;
        private System.Windows.Forms.Button btnSetTemperature;
        private System.Windows.Forms.Button btnSetBrightness;
        private System.Windows.Forms.Button btnTurnOff;
        private System.Windows.Forms.Button btnTurnOn;
        private System.Windows.Forms.Button btnSimulateTemperature;
        private System.Windows.Forms.Button btnSimulateMotion;
        private System.Windows.Forms.Button btnDeactivateSensor;
        private System.Windows.Forms.Button btnActivateSensor;
        private System.Windows.Forms.ListView lstSensors;
        private System.Windows.Forms.ListView lstSchedules;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnVoiceCommand;
    }
}
