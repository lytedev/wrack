using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

using Microsoft.Xna.Framework;

namespace WrackEngine.Net
{
    public class TcpConnection
    {
        protected bool reading = false;

        public TcpClient Sock { get; set; }
        public NetworkStream Stream { get; set; }
        public Queue<Packet> RecievedPackets { get; set; }
        public int HandledPackets { get; set; }
        public int State { get; set; }

        public EndPoint LocalEndPoint { get { if (Sock != null) return Sock.Client.LocalEndPoint; else return null; } }
        public EndPoint RemoteEndPoint { get { if (Sock != null) return Sock.Client.RemoteEndPoint; else return null; } }

        public bool Connected
        {
            get
            {
                return Sock.Connected;
            }
        }

        public TcpConnection()
        {
            Sock = new TcpClient();
            Sock.ReceiveBufferSize = 2 ^ 14;
            Sock.SendBufferSize = 2 ^ 14;
            Stream = null;
            HandledPackets = 0;
            RecievedPackets = new Queue<Packet>();
            State = 0;
        }

        public TcpConnection(TcpClient client)
        {
            Sock = client;
            if (Sock.Connected) Stream = Sock.GetStream();
            else Stream = null;
            HandledPackets = 0;
            RecievedPackets = new Queue<Packet>();
            State = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            ReadPackets();
            HandleNextPackets(gameTime);
        }

        public virtual void Disconnect()
        {
            if (Connected)
            {
                Sock.Close();
                Stream = null;
            }
        }

        public virtual void ReadPackets(int numPackets = 32)
        {
            if (Stream == null) return;
            while (Stream.DataAvailable && Connected)
            {
                if (!reading)
                {
                    reading = true;
                    byte[] buffer = new byte[Sock.Client.ReceiveBufferSize];
                    // Stream.BeginRead(buffer, 0, buffer.Length, ReadPacketsCallback, buffer);
                    ///*
                    int len = Stream.Read(buffer, 0, buffer.Length);
                    if (len < 8) return;
                    Packet p;
                    int offset = 0;
                    int length = 0;
                    lock (RecievedPackets)
                    {
                        while (numPackets >= 0)
                        {
                            if (offset + length >= buffer.Length) break;
                            length = BitConverter.ToInt32(buffer, offset);
                            if (offset + length >= buffer.Length) break;
                            if (length <= 0) break;
                            byte[] subBuffer = new byte[length];
                            Buffer.BlockCopy(buffer, offset, subBuffer, 0, length);
                            p = Packet.FromBytes(subBuffer);
                            {
                                RecievedPackets.Enqueue(p);
                            }
                            offset += length;
                            numPackets--;
                        }
                    }
                    reading = false;
                    //*/
                }
            }
        }

        public virtual void ReadPacketsCallback(IAsyncResult ar)
        {
            int len = Stream.EndRead(ar);
            // if (len < 8) return;
            byte[] buffer = (byte[])ar.AsyncState;
            Packet p;
            int offset = 0;
            int length = 0;
            lock (RecievedPackets)
            {
                while (true)
                {
                    if (offset >= buffer.Length) break;
                    length = BitConverter.ToInt32(buffer, offset);
                    if (length == 0) break;
                    byte[] subBuffer = new byte[length];
                    Buffer.BlockCopy(buffer, offset, subBuffer, 0, length);
                    p = Packet.FromBytes(subBuffer);
                    {
                        RecievedPackets.Enqueue(p);
                    }
                    offset += length;
                }
            }
            reading = false;
        }

        public virtual void SendPacket(Packet p, int offset = 0) { SendBytes(p.ToBytes(), offset); }
        public virtual void SendBytes(byte[] b, int offset = 0)
        {
            if (Stream == null || !Connected) return;
            // TODO: Remove!
            try
            {
                Stream.BeginWrite(b, offset, b.Length, new AsyncCallback(SendBytesCallback), null);
            }
            catch (Exception e)
            {
                Wrack.Terminal.WriteLine(TerminalMessageType.Error, "NET: SendBytes() failed: {0}", e.Message);
                Disconnect();
            }
        }

        public virtual void SendBytesCallback(IAsyncResult ar)
        {
            Stream.EndWrite(ar);
        }

        public virtual void HandleNextPackets(GameTime gameTime, int numPackets = 32)
        {
            while (numPackets >= 0 && RecievedPackets.Count > 0)
            {
                HandlePacket(gameTime, RecievedPackets.Dequeue());
            }
        }

        public virtual void HandlePacket(GameTime gameTime, Packet p)
        {
            HandledPackets++;
        }

        public virtual bool HasState(int state)
        {
            return (State & state) == state;
        }

        public virtual void SetState(int state)
        {
            if (HasState(state))
            {
                return;
            }
            else
            {
                State |= state;
            }
        }

        public virtual void UnsetState(int state)
        {
            if (HasState(state))
            {
                State ^= state;
            }
            else
            {
                return;
            }
        }
    }
}
