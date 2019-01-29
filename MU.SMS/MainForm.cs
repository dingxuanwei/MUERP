using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace MU.SMS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        bool IsOpen = false;
        SerialPort sp = null;

        private void MainForm_Load(object sender, EventArgs e)
        {
            var names = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(names);
            if (names.Length > 0) comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsOpen)
            {
                sp.DataReceived -= Sp_DataReceived;
                sp.Close();
            }
            else
            {
                sp = new SerialPort((string)comboBox1.SelectedItem, int.Parse(textBox1.Text), Parity.None, 8, StopBits.One);
                sp.DataReceived += Sp_DataReceived;
                sp.Open();
            }
            IsOpen = sp.IsOpen;
            if (IsOpen)
                button1.Text = "关闭(&Q)";
            else
                button1.Text = "打开(&O)";
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buffer = new byte[sp.ReadBufferSize];
            int recv = sp.Read(buffer, 0, buffer.Length);
            var result = Hex2String(buffer.Take(recv).ToArray());
            Invoke(new Action(()=> { textBox2.AppendText($"{DateTime.Now}\t{result}\r\n"); }));
        }

        private string Hex2String(byte[] dgram)
        {
            StringBuilder sb = new StringBuilder(dgram.Length * 3);
            for (int i = 0; i < dgram.Length; i++)
            {
                sb.AppendFormat("{0:X2} ", dgram[i]);
            }
            return sb.ToString().TrimEnd();
        }

        private byte[] String2Hex(string source)
        {
            var s = source.Split(' ');
            var buffer = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                int b = int.Parse(s[i], System.Globalization.NumberStyles.HexNumber);
                buffer[i] = Convert.ToByte(b.ToString());
            }
            return buffer;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!IsOpen)
            {
                MessageBox.Show("串口未打开");
                return;
            }
            if (checkBox1.Checked)
            {
                var txt = textBox3.Text.Trim();
                var buffer = String2Hex(txt);
                sp.Write(buffer, 0, buffer.Length);
                lblMsg.Text = $"{DateTime.Now}\t发送字节流成功";
            }
            else
            {
                sp.Write(textBox3.Text.Trim());
                lblMsg.Text = $"{DateTime.Now}\t发送文字成功";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (IsOpen) sp.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                var b = Encoding.ASCII.GetBytes(textBox3.Text.Trim());
                var txt = Hex2String(b);
                textBox3.Text = txt;
            }
            else
            {
                var b = String2Hex(textBox3.Text.Trim());
                var s = Encoding.ASCII.GetString(b);
                textBox3.Text = s;
            }
        }
    }
}
