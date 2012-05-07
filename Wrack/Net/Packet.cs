using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;

namespace WrackEngine.Net
{
    public class Packet
    {
        public static ASCIIEncoding ASCII = new ASCIIEncoding();

        public int Type { get; set; }
        public List<byte> Bytes { get; set; }

        private int bytePointer { get; set; }

        public Packet() : this(0) { }
        public Packet(int type)
        {
            Type = type;
            Bytes = new List<byte>();

            bytePointer = 0;
        }

        public override string ToString()
        {
            string msg = ASCII.GetString(Bytes.ToArray());
            if (msg.Length > 128) msg = msg.Substring(0, 124) + " ...";
            return "{Packet: Type=" + Type + ", Size=" + Bytes.Count + " \"" + msg + "\"}";
        }

        public void AddInt16(short x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddUInt16(ushort x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddInt32(int x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddUInt32(uint x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddInt64(long x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddUInt64(ulong x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddFloat(float x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddDouble(double x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddBoolean(bool x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddChar(char x)
        {
            AddBytes(BitConverter.GetBytes(x));
        }

        public void AddVector2(Vector2 v)
        {
            AddFloat(v.X);
            AddFloat(v.Y);
        }

        public void AddColor(Color c)
        {
            AddByte(c.A);
            AddByte(c.R);
            AddByte(c.G);
            AddByte(c.B);
        }

        public void AddString(string x)
        {
            if (x == null)
            {
                AddString("");
                return;
            }
            AddBytes(BitConverter.GetBytes(x.Length));
            AddBytes(ASCII.GetBytes(x));
        }

        public void AddBytes(byte[] b)
        {
            Bytes.AddRange(b);
        }

        public void AddByte(byte b)
        {
            Bytes.Add(b);
        }

        public void AddPacket(Packet p)
        {
            AddBytes(p.Bytes.ToArray());
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadBytes(2), 0);
        }

        public int ReadInt32()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadBytes(4), 0);
        }

        public long ReadInt64()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadBytes(8), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBytes(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }

        public bool ReadBoolean()
        {
            return BitConverter.ToBoolean(ReadBytes(1), 0);
        }

        public double ReadChar()
        {
            return BitConverter.ToChar(ReadBytes(2), 0);
        }

        public byte[] ReadBytes(int length)
        {
            byte[] b = new byte[length];
            for (int i = bytePointer; i < bytePointer + length; i++) b[i - bytePointer] = Bytes[i];
            bytePointer += length;
            return b;
        }

        public byte ReadByte()
        {
            return Bytes[bytePointer++];
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(ReadFloat(), ReadFloat());
        }

        public string ReadString()
        {
            int length = ReadInt32();
            if (length == 0) return "";
            return new string(ASCII.GetChars(ReadBytes(length)));
        }

        public Color ReadColor()
        {
            Color c = new Color();
            c.A = ReadByte();
            c.R = ReadByte();
            c.G = ReadByte();
            c.B = ReadByte();
            return c;
        }

        public virtual byte[] ToBytes()
        {
            byte[] bytes = Bytes.ToArray();
            byte[] b = new byte[8 + bytes.Length];
            Buffer.BlockCopy(BitConverter.GetBytes(b.Length), 0, b, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Type), 0, b, 4, 4);
            Buffer.BlockCopy(bytes, 0, b, 8, bytes.Length);
            return b;
        }

        public static Packet FromBytes(byte[] b)
        {
            if (b.Length <= 4) return new Packet();
            Packet p = new Packet();
            int length = BitConverter.ToInt32(b, 0);
            p.Type = BitConverter.ToInt32(b, 4);
            byte[] buffer = new byte[length - 8];
            Buffer.BlockCopy(b, 8, buffer, 0, length - 8);
            p.Bytes.AddRange(buffer);
            return p;
        }

        public Packet DeepClone()
        {
            Packet p = new Packet(Type);
            p.Bytes = new List<byte>();
            p.Bytes.AddRange(Bytes.ToArray());
            return p;
        }
    }
}
