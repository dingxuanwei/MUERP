using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetWorkSpeedMonitor
{
    public partial class FormMain : Form
    {
        private NetworkAdapter[] adapters;
        private NetworkMonitor monitor;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            monitor = new NetworkMonitor();
            this.adapters = monitor.Adapters;
            /* If the length of adapters is zero, then no instance  
             * exists in the networking category of performance console.*/
            if (adapters.Length == 0)
            {
                this.ListAdapters.Enabled = false;
                MessageBox.Show("No network adapters found on this computer.");
                return;
            }
            this.ListAdapters.Items.AddRange(this.adapters);
        }

        private void ListAdapters_SelectedIndexChanged(object sender, EventArgs e)
        {
            monitor.StopMonitoring();
            monitor.StartMonitoring(adapters[this.ListAdapters.SelectedIndex]);
            this.TimerCounter.Start();
        }

        private void TimerCounter_Tick(object sender, EventArgs e)
        {
            if (this.adapters.Count() == 0) return;
            if (this.ListAdapters.SelectedIndex == -1) return;
            NetworkAdapter adapter = this.adapters[this.ListAdapters.SelectedIndex];
            this.LabelDownloadValue.Text = string.Format("{0:n} kbps", adapter.DownloadSpeedKbps);
            this.LabelUploadValue.Text = string.Format("{0:n} kbps", adapter.UploadSpeedKbps);
        }
    }
}
