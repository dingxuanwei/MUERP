using System;
using System.Net.Sockets;
using System.Net;
using System.Configuration;

namespace MUUDP
{
    public class MUDP
    {
        public delegate void ReceiveMessage(IPEndPoint remote, byte[] dgram);
        public event ReceiveMessage OnReceiveMessage;

        byte[] buffer = new byte[256];
        byte[] back = new byte[2];
        UdpClient socket = null;

        public MUDP()
        {
            try
            {
                back = System.Text.Encoding.ASCII.GetBytes("OK");
                int port = int.Parse(ConfigurationManager.AppSettings["port"]);
                socket = new UdpClient(port, AddressFamily.InterNetwork);

                IAsyncResult result = socket.BeginReceive(ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (ar.IsCompleted)
                {
                    IPEndPoint epp = new IPEndPoint(IPAddress.Any, 0);
                    byte[] buff = socket.EndReceive(ar, ref epp);
                    ar.AsyncWaitHandle.Close();
                    OnReceiveMessage?.Invoke(epp, buff);
                    socket.Send(back, back.Length, epp);
                    socket.BeginReceive(ReceiveCallback, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public int Send(byte[] dgram, IPEndPoint endPoint)
        {
            return socket.Send(dgram, dgram.Length, endPoint);
        }

        public void Close()
        {
            socket.Close();
            socket.Dispose();
        }
    }
}
