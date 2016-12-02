using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCoder
{
    public class Coder
    {
        private MemoryStream stream;
        public Coder()
        {
            stream = new MemoryStream();
        }

        public void addByte(byte b)
        {
            stream.WriteByte((byte)TypCode.BYTE);
            stream.WriteByte(b);
        }

        public void addInteger(int i)
        {
            stream.WriteByte((byte)TypCode.INTEGER);
            stream.WriteByte((byte)(i >> 24));
            stream.WriteByte((byte)(i >> 16));
            stream.WriteByte((byte)(i >> 8));
            stream.WriteByte((byte)i);
        }

        public void addString(string s)
        {
            stream.WriteByte((byte)TypCode.STRING);
            s += "\0";
            foreach (char item in s.ToCharArray())
            {
                stream.WriteByte((byte)(item >> 8));
                stream.WriteByte((byte)(item));
            }
        }

        public void addLong(long l)
        {
            stream.WriteByte((byte)TypCode.LONG);
            stream.WriteByte((byte)(l >> 56));
            stream.WriteByte((byte)(l >> 48));
            stream.WriteByte((byte)(l >> 40));
            stream.WriteByte((byte)(l >> 32));
            stream.WriteByte((byte)(l >> 24));
            stream.WriteByte((byte)(l >> 16));
            stream.WriteByte((byte)(l >> 8));
            stream.WriteByte((byte)l);
        }

        public Stream getStream()
        {
            return stream;
        }
    }
    public static class Decoder
    {
        public static List<object> getData(Stream stream)
        {
            List<object> result = new List<object>();
            long pos = stream.Position;
            stream.Position = 0;
            StreamParser(stream, result);
            stream.Position = pos;
            return result;
        }

        private static void StreamParser(Stream stream, List<object> result)
        {
            int i = stream.ReadByte();
            while (i != -1)
            {
                byte b = (byte)i;
                switch (b)
                {
                    case (byte)TypCode.BYTE:
                        result.Add(ReadByte(stream));
                        break;
                    case (byte)TypCode.INTEGER:
                        result.Add(ReadInteger(stream));
                        break;
                    case (byte)TypCode.STRING:
                        result.Add(ReadString(stream));
                        break;
                    case (byte)TypCode.LONG:
                        result.Add(ReadLong(stream));
                        break;
                    default:
                        break;
                }
                i = stream.ReadByte();
            }
        }

        private static long ReadLong(Stream stream)
        {
            long l = 0;
            for (int x = 1; x <= 8; x++)
            {
                long y = stream.ReadByte();
                l = l | (y << (64 - x * 8));
            }
            return l;
        }

        private static string ReadString(Stream stream)
        {
            int iChar = (stream.ReadByte() << 8) | stream.ReadByte();
            List<char> charList = new List<char>();
            char c = (char)iChar;
            while (c != '\0')
            {
                charList.Add(c);
                iChar = (stream.ReadByte() << 8) | stream.ReadByte();
                c = (char)iChar;
            }
            return new string(charList.ToArray());
        }

        private static int ReadInteger(Stream stream)
        {
            int i = 0;
            for (int x = 1; x <= 4; x++)
            {
                int y = stream.ReadByte();
                i = i | (y << (32 - x * 8));
            }
            return i;
        }

        private static byte ReadByte(Stream stream)
        {
            return (byte)stream.ReadByte();
        }
    }
    public enum TypCode
    {
        BYTE = 1,
        INTEGER = 2,
        STRING = 3,
        LONG = 4
    }
}
