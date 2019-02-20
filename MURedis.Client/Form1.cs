using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceStack.Redis;

namespace MURedis.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var client = new RedisClient();
                var sub = client.CreateSubscription();
                sub.OnMessage += (channel, msg) =>
                {
                    Invoke(new Action(()=> { label1.Text = $"{channel}:{msg}"; }));
                };
                sub.SubscribeToChannels("1-15");
            });
        }
    }
}
