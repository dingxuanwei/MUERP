using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace MUUDPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MUDP service = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            var station = "1-1";

            service = new MUDP();
            service.OnReceiveMessage += Service_OnReceiveMessage;
            service.Send(Encoding.UTF8.GetBytes(station), new IPEndPoint(IPAddress.Parse("192.168.1.17"), 10000));

        }

        private void Service_OnReceiveMessage(IPEndPoint remote, byte[] dgram)
        {
            Invoke(new Action(() =>
            {
                textBox1.AppendText($"{DateTime.Now}\t{remote}\t{Encoding.UTF8.GetString(dgram)}\r\n");
            }));
        }
    }
}
