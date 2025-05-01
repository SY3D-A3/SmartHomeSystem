using System;

namespace SmartHomeSystem.Models
{
    /// <summary>
    /// Represents a smart light device
    /// </summary>
    public class Light : SmartDevice
    {
        public int Brightness { get; set; } // 0-100%
        public string Color { get; set; }

        public Light(int deviceID, string location, string color = "White") 
            : base(deviceID, location)
        {
            Brightness = 0;
            Color = color;
        }

        /// <summary>
        /// Sets the brightness level of the light
        /// </summary>
        /// <param name="level">Brightness level (0-100)</param>
        public void SetBrightness(int level)
        {
            if (level < 0 || level > 100)
            {
                throw new ArgumentOutOfRangeException("Brightness must be between 0 and 100");
            }

            Brightness = level;
            Console.WriteLine($"Light {DeviceID} brightness set to {level}%");
            
            // If brightness is set to 0, turn off the light
            if (level == 0)
            {
                TurnOff();
            }
            // If the light was off and brightness is set > 0, turn it on
            else if (!Status && level > 0)
            {
                TurnOn();
            }
        }

        /// <summary>
        /// Sets the color of the light
        /// </summary>
        /// <param name="color">Color name</param>
        public void SetColor(string color)
        {
            Color = color;
            Console.WriteLine($"Light {DeviceID} color set to {color}");
        }

        /// <summary>
        /// Turns the light on with the current brightness
        /// </summary>
        public override void TurnOn()
        {
            if (Brightness == 0)
            {
                Brightness = 100; // Default to full brightness when turning on
            }
            
            base.TurnOn();
            Console.WriteLine($"Light {DeviceID} turned ON with brightness {Brightness}% and color {Color}");
        }

        /// <summary>
        /// Turns the light off
        /// </summary>
        public override void TurnOff()
        {
            base.TurnOff();
            // We don't reset brightness to 0 here so it remembers the last setting
        }
    }
}
