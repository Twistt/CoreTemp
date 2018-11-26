using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using System.Management;
using System.IO;

namespace CoreTemp
{
    public partial class CoreTemp : Form
    {
        Timer timer = new Timer();
        string CPUTemps = string.Empty;
        public CoreTemp()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
            timer.Interval = 5000; //5 seconds
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            getTemp();
            toolStripStatusLabel1.Text = "Last updated: " + DateTime.Now.ToLongTimeString();


            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();

        }
        private void getTemp() {
            var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");

            foreach (ManagementObject service in searcher.Get())
            {
                // show the instance
                var temp = Convert.ToDouble(service.GetPropertyValue("CurrentTemperature"));
                temp = (temp - 2732) / 10.0;
                lblCPU.Text = temp.ToString() + "C";
                var faren = (temp * 9 / 5) +32;
                lblCPU.Text += " / " + faren + "F";

                CPUTemps+= DateTime.Now +": "+ (temp.ToString() + "C" + " / " + faren + "F" + "\r\n");
            }
            File.WriteAllText("CPUTemps.txt",CPUTemps);
            
        }
        
    }
}
