using System;

namespace SmartHomeSystemWinForms.Models
{
    /// <summary>
    /// Represents a smart security camera device
    /// </summary>
    public class SecurityCamera : SmartDevice
    {
        public bool IsRecording { get; private set; }
        public bool MotionDetectionEnabled { get; private set; }
        public int Resolution { get; set; } // in pixels (e.g., 1080, 2160, etc.)

        public SecurityCamera(int deviceID, string location, int resolution = 1080) 
            : base(deviceID, location)
        {
            IsRecording = false;
            MotionDetectionEnabled = false;
            Resolution = resolution;
        }

        /// <summary>
        /// Starts recording video
        /// </summary>
        public void StartRecording()
        {
            if (!Status)
            {
                Console.WriteLine($"Cannot start recording: Camera {DeviceID} is turned off");
                return;
            }

            IsRecording = true;
            Console.WriteLine($"Camera {DeviceID} started recording at {Resolution}p resolution");
        }

        /// <summary>
        /// Stops recording video
        /// </summary>
        public void StopRecording()
        {
            if (IsRecording)
            {
                IsRecording = false;
                Console.WriteLine($"Camera {DeviceID} stopped recording");
            }
        }

        /// <summary>
        /// Enables motion detection
        /// </summary>
        public void EnableMotionDetection()
        {
            if (!Status)
            {
                Console.WriteLine($"Cannot enable motion detection: Camera {DeviceID} is turned off");
                return;
            }

            MotionDetectionEnabled = true;
            Console.WriteLine($"Camera {DeviceID} motion detection enabled");
        }

        /// <summary>
        /// Disables motion detection
        /// </summary>
        public void DisableMotionDetection()
        {
            MotionDetectionEnabled = false;
            Console.WriteLine($"Camera {DeviceID} motion detection disabled");
        }

        /// <summary>
        /// Simulates motion detection and triggers recording if motion detection is enabled
        /// </summary>
        public void DetectMotion()
        {
            if (!Status || !MotionDetectionEnabled)
            {
                return;
            }

            Console.WriteLine($"Camera {DeviceID} detected motion!");
            
            // Start recording when motion is detected
            if (!IsRecording)
            {
                StartRecording();
            }
        }

        /// <summary>
        /// Takes a snapshot photo
        /// </summary>
        /// <returns>A message indicating the snapshot was taken</returns>
        public string TakeSnapshot()
        {
            if (!Status)
            {
                return $"Cannot take snapshot: Camera {DeviceID} is turned off";
            }

            return $"Camera {DeviceID} snapshot taken at {DateTime.Now} with {Resolution}p resolution";
        }

        /// <summary>
        /// Turns the camera on
        /// </summary>
        public override void TurnOn()
        {
            base.TurnOn();
            Console.WriteLine($"Camera {DeviceID} activated with {Resolution}p resolution");
        }

        /// <summary>
        /// Turns the camera off and stops any active recording
        /// </summary>
        public override void TurnOff()
        {
            if (IsRecording)
            {
                StopRecording();
            }
            
            DisableMotionDetection();
            base.TurnOff();
            Console.WriteLine($"Camera {DeviceID} deactivated");
        }
    }
}
