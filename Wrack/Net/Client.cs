using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace WrackEngine.Net
{
    public class Client : TcpConnection
    {
        public virtual void Connect(string ipStr) { Connect(ipStr, Settings.GetIntSetting("default_port")); }
        public virtual void Connect(string ipStr, int port)
        {
            Disconnect();
            Sock = new TcpClient();
            Sock.BeginConnect(ipStr, port, new AsyncCallback(ConnectCallback), Sock);
        }

        public virtual void ConnectCallback(IAsyncResult ar)
        {
            Sock = (TcpClient)ar.AsyncState;
            try
            {
                Sock.EndConnect(ar);
                Stream = Sock.GetStream();
                Wrack.Terminal.WriteLine(TerminalMessageType.Good, "CLIENT: Connected to {0}.", Sock.Client.RemoteEndPoint);
            }
            catch (Exception e)
            {
                Wrack.Terminal.WriteLine(TerminalMessageType.Error, "CLIENT: Failed to connect: {0}.", e.Message);
            }
        }
    }
}
