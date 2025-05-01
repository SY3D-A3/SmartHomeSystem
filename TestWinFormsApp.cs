using System;
using System.Windows.Forms;

namespace TestWinFormsApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Create a simple form
            Form form = new Form();
            form.Text = "Test Windows Forms Application";
            form.Width = 400;
            form.Height = 300;
            
            // Add a label
            Label label = new Label();
            label.Text = "If you can see this, Windows Forms is working correctly.";
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(50, 50);
            form.Controls.Add(label);
            
            // Add a button
            Button button = new Button();
            button.Text = "Click Me";
            button.Location = new System.Drawing.Point(50, 100);
            button.Click += (sender, e) => MessageBox.Show("Button clicked!");
            form.Controls.Add(button);
            
            // Run the application
            Application.Run(form);
        }
    }
}
