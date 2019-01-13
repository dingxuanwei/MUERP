using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using MUSystem.MES.Hang;

namespace WinFormDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "true";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox5.Text = "false";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IGSTAdapter ada = new GSTAdapter();
            byte newStation = 0;
            byte newTrack = 0;
            var b = ada.RequestOut(Convert.ToByte(textBox1.Text),
                Convert.ToByte(textBox2.Text),
                Convert.ToByte(textBox3.Text),
                Convert.ToUInt16(textBox4.Text),
                Convert.ToUInt32(textBox6.Text),
                out newStation,
                out newTrack);

            lblMsg.Text = string.Format("返回：{0}，下一站：{1}", b, newStation);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IGSTAdapter ada = new GSTAdapter();
            var b = ada.ReportHangerRecvd(Convert.ToByte(textBox1.Text),
                Convert.ToByte(textBox2.Text),
                Convert.ToByte(textBox3.Text),
                Convert.ToUInt32(textBox6.Text));
            lblMsg.Text = string.Format("返回：{0}", b);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IGSTAdapter ada = new GSTAdapter();
            var b = ada.ReportWorkHanger(Convert.ToByte(textBox1.Text),
                Convert.ToByte(textBox2.Text),
                Convert.ToUInt32(textBox6.Text));
            lblMsg.Text = string.Format("返回：{0}", b);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IGSTAdapter ada = new GSTAdapter();
            var b = ada.ReportStnState(Convert.ToByte(textBox1.Text),
                Convert.ToByte(textBox2.Text),
                Convert.ToByte(textBox3.Text),
                Convert.ToBoolean(textBox5.Text));
            lblMsg.Text = string.Format("返回：{0}", b);
        }
    }
}
