using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MU.Push
{
    partial class WinService : ServiceBase
    {
        public WinService()
        {
            InitializeComponent();
        }
        ServiceHost host = null;

        protected override void OnStart(string[] args)
        {
            string port = System.Configuration.ConfigurationManager.AppSettings["port"].ToString();
            host = new ServiceHost(typeof(MPService));
            host.Opened += (s, e) => { Console.WriteLine("WCF opened on " + host.BaseAddresses[0]); };
            host.Open();
            PushServer.Instance().StartWebSocket(port, Fleck.LogLevel.Error);
            //System.IO.File.AppendAllText(@"D:\Log.txt", "\r\nService Start :" + DateTime.Now.ToString());
        }

        protected override void OnStop()
        {
            host.Close();
            host = null;
            PushServer.Instance().StopWebSocket();
            //System.IO.File.AppendAllText(@"D:\Log.txt", "\r\nService Stop :" + DateTime.Now.ToString());
        }
    }
}
