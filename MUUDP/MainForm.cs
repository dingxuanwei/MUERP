using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MUUDP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        MUDP service = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            service = new MUDP();
            service.OnReceiveMessage += Service_OnReceiveMessage;
        }

        private void Service_OnReceiveMessage(IPEndPoint remote, byte[] dgram)
        {
            Invoke(new Action(() =>
            {
                listBox1.Items.Add($"{remote}-{Encoding.UTF8.GetString(dgram)}");
                //textBox1.AppendText($"{DateTime.Now}\t{remote}\t{Encoding.UTF8.GetString(dgram)}\r\n");
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem as string;
            if (!string.IsNullOrEmpty(selected))
            {
                var iped = selected.Split('-')[0];
                int recv = service.Send(Encoding.UTF8.GetBytes(textBox1.Text), new IPEndPoint(IPAddress.Parse(iped.Split(':')[0]), int.Parse(iped.Split(':')[1])));
                label1.Text = $"{DateTime.Now},发送了{recv}个字符";
            }
        }
    }
}
