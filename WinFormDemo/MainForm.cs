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

namespace WinFormDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Report";

            int left = label1.Left;
            int top = label1.Top;
            int width = label1.Width;
            int height = label1.Height;
            var lbl = new Label[20];
            for (int i = 0; i < lbl.Length; i++)
            {
                lbl[i] = new Label();
                lbl[i].TextAlign = ContentAlignment.TopCenter;
                lbl[i].Text = i.ToString();
                lbl[i].Left = left + width * ((i + 1) % 4);
                lbl[i].Top = top + height * ((i + 1) / 4);
                panel1.Controls.Add(lbl[i]);
            }
            label1.Visible = true;
        }
        
    }
}
