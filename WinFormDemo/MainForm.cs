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
        BackgroundWorker bg = new BackgroundWorker();

        private void button1_Click(object sender, EventArgs e)
        {
            bg.DoWork += Bg_DoWork;
            bg.RunWorkerAsync();
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            DoSomething();
            this.Invoke(new Action(()=> {
                textBox1.Text = "Do job Success";
            }));
            
        }

        private void DoSomething()
        {
            Task t1 = Task.Run(new Action(() => { Thread.Sleep(1000); }));

            t1.ContinueWith(t => { Thread.Sleep(2000); Console.WriteLine("第二次任务"); })
                .ContinueWith(t => { Thread.Sleep(2000); Console.WriteLine("第三次任务"); })
                .ContinueWith(t => { Thread.Sleep(2000); Console.WriteLine("第四次任务"); });

        }
    }
}
