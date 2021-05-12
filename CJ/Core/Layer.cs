using System;
using System.Collections.Generic;
using System.Text;

namespace CJ.Core
{
    public class Layer
    {


        public List<int> IArr;
        public List<float> FArr;
        public List<string> SArr;
        public List<long> LArr;
        public List<byte> bytes;

        public Layer(string type, MsgManager manager)
        {

            IArr = new List<int>();
            FArr = new List<float>();
            SArr = new List<string>();
            LArr = new List<long>();
            SArr.Add(type);
            IArr.Add(0);
        }
        public Layer() {
            IArr = new List<int>();
            FArr = new List<float>();
            SArr = new List<string>();
            LArr = new List<long>();
        }
        public int bytesToInt(byte[] vv)
        {
            int v = 0;
            switch (vv[0])
            {
                case 0x00:
                    v = vv[1];
                    break;

                case 0x01:
                    v = vv[1] | vv[2] << 8;
                    break;

                case 0x02:
                    v = vv[1] | vv[2] << 8 | vv[3] << 16;
                    break;

                case 0x03:
                    v = vv[1] | vv[2] << 8 | vv[3] << 16 | vv[4] << 24;
                    break;

            }
            return v;
        }

        public byte[] int2byte4(int v)
        {
            byte[] bytes = new byte[4];
            bytes[3] = (byte)(v >> 24);
            bytes[2] = (byte)(v >> 16);
            bytes[1] = (byte)(v >> 8);
            bytes[0] = (byte)v;

            return bytes;
        }

        public byte[] long2bytes(long v)
        {

            byte[] bytes = new byte[8];
            bytes[7] = (byte)(v >> 56);
            bytes[6] = (byte)(v >> 48);
            bytes[5] = (byte)(v >> 40);
            bytes[4] = (byte)(v >> 32);
            bytes[3] = (byte)(v >> 24);
            bytes[2] = (byte)(v >> 16);
            bytes[1] = (byte)(v >> 8);
            bytes[0] = (byte)v;

            List<byte> aim = new List<byte>();
            if (v < Math.Pow(256, 1))
            {
                aim.Add(0x00);
            }
            if (v >= Math.Pow(256, 1) && v < Math.Pow(256, 2))
            {
                aim.Add(0x01);

            }
            if (v >= Math.Pow(256, 2) && v < Math.Pow(256, 3))
            {
                aim.Add(0x02);
            }
            if (v >= Math.Pow(256, 3) && v < Math.Pow(256, 4))
            {
                aim.Add(0x03);

            }
            if (v >= Math.Pow(256, 4) && v < Math.Pow(256, 5))
            {

                aim.Add(0x04);
            }
            if (v >= Math.Pow(256, 5) && v < Math.Pow(256, 6))
            {

                aim.Add(0x05);
            }
            if (v >= Math.Pow(256, 6) && v < Math.Pow(256, 7))
            {

                aim.Add(0x06);
            }
            if (v >= Math.Pow(256, 7))
            {
                aim.Add(0x07);
            }
            for (int i = 0; i < aim[0] + 1; i++)
            {
                aim.Add(bytes[i]);
            }
            return aim.ToArray();
        }

        public long bytes2long(byte[] buf)
        {
            int len = buf[0];
            long v = 0;
            for (int i = 1; i <= len + 1; i++)
            {
                long a = buf[i];
                v = v | (a << (8 * (i - 1)));
            }
            return v;
        }
        public byte[] int2bytes(int v)
        {
            byte[] bytes = new byte[4];
            bytes[3] = (byte)(v >> 24);
            bytes[2] = (byte)(v >> 16);
            bytes[1] = (byte)(v >> 8);
            bytes[0] = (byte)v;
            List<byte> aim = new List<byte>();
            if (v < 256)
            {
                aim.Add(0x00);
            }
            if (v >= 256 && v < 65536)
            {
                aim.Add(0x01);
            }
            if (v >= 65536 && v < (256 * 256 * 256))
            {
                aim.Add(0x02);
            }
            if (v >= (256 * 256 * 256))
            {
                aim.Add(0x03);
            }
            switch (aim[0])
            {
                case 0x00:
                    aim.Add(bytes[0]);
                    break;

                case 0x01:
                    aim.Add(bytes[0]);
                    aim.Add(bytes[1]);
                    break;

                case 0x02:
                    aim.Add(bytes[0]);
                    aim.Add(bytes[1]);
                    aim.Add(bytes[2]);
                    break;

                case 0x03:
                    aim.Add(bytes[0]);
                    aim.Add(bytes[1]);
                    aim.Add(bytes[2]);
                    aim.Add(bytes[3]);
                    break;
            }
            return aim.ToArray();
        }

        byte[] string2Byte(string v)
        {
            byte[] tp = Encoding.UTF8.GetBytes(v);
            byte[] t2 = new byte[tp.Length + 4];
            List<byte> lb = new List<byte>();
            byte[] tc = int2byte4(v.Length);
            if (v.Length < 256)
            {
                lb.Add(0x00);
                lb.Add(tc[0]);
            }
            if (v.Length > 256 && v.Length < 256 * 256)
            {
                lb.Add(0x01);
                lb.Add(tc[0]);
                lb.Add(tc[1]);
            }
            if (v.Length > 256 * 256 && v.Length < 256 * 256 * 256)
            {
                lb.Add(0x02);
                lb.Add(tc[0]);
                lb.Add(tc[1]);
                lb.Add(tc[2]);
            }
            if (v.Length > 256 * 256 * 256)
            {
                lb.Add(0x03);
                lb.Add(tc[0]);
                lb.Add(tc[1]);
                lb.Add(tc[2]);
                lb.Add(tc[3]);
            }

            for (int i = 0; i < tp.Length; i++)
            {
                lb.Add(tp[i]);
            }
            return lb.ToArray();
        }


        public byte[] toBuffer()
        {
            Queue<byte> q = new Queue<byte>();

            
            //类型信息
            //setQueue(q, int2bytes(IArr[0]));
            
            setQueue(q, string2Byte(SArr[0]));
            List<byte> iarrbytes = new List<byte>();
            List<byte> farrbytes = new List<byte>();
            List<byte> larrbytes = new List<byte>();
            List<byte> sarrbytes = new List<byte>();

            for (int i = 1; i < IArr.Count; i++) {
                foreach (byte k in int2bytes(IArr[i])) {
                    iarrbytes.Add(k);
                }
            }


            for (int i = 0; i < LArr.Count; i++)
            {
                foreach (byte k in long2bytes(LArr[i]))
                {
                    larrbytes.Add(k);
                }
            }
            for (int i = 1; i < SArr.Count; i++)
            {
                foreach (byte k in string2Byte(SArr[i]))
                {
                    sarrbytes.Add(k);
                }
            }

            foreach (var d in FArr)
            {
                foreach (byte k in float2Byte(d))
                {
                    farrbytes.Add(k);
                }
            }

            setQueue(q, int2bytes(IArr.Count-1));
            setQueue(q, int2bytes(LArr.Count));
            setQueue(q, int2bytes(SArr.Count - 1));
            setQueue(q, int2bytes(FArr.Count));
            setQueue(q, iarrbytes.ToArray());
            setQueue(q, larrbytes.ToArray());
            setQueue(q, sarrbytes.ToArray());
            setQueue(q, farrbytes.ToArray());

            return q.ToArray();
        }
        byte[] extractFromQueue(Queue<byte> q)
        {
            List<byte> tp = new List<byte>();
            tp.Add(q.Dequeue());
            for (int i = 0; i < tp[0]+1; i++)
            {
                tp.Add(q.Dequeue());
            }
            return tp.ToArray();
        }

        byte[] extractFromQueue2(Queue<byte> q)
        {
            List<byte> tp = new List<byte>();
            int len = bytesToInt(extractFromQueue(q));

            for (int i = 0; i < len; i++)
            {
                tp.Add(q.Dequeue());
            }
            return tp.ToArray();
        }
        byte[] extractFromQueue3(Queue<byte> q)
        {
            List<byte> tp = new List<byte>();
            int len = 4;
            for (int i = 0; i < len; i++)
            {
                tp.Add(q.Dequeue());
            }
            return tp.ToArray();
        }

        void setQueue(Queue<byte> q, byte[] data)
        {
            foreach (byte k in data)
            {
                q.Enqueue(k);
            }
        }

        public Layer fromBuffer(byte[] buff,MsgManager manager)
        {
            Queue<byte> q = new Queue<byte>();
            foreach (var k in buff) {
                q.Enqueue(k);
            }
            int slen = bytesToInt(extractFromQueue(q));
            
            IArr.Add(0);
            string tt = "";
            for (int i = 0; i < slen; i++) {
                tt += (char)q.Dequeue();
            }
            SArr.Add(tt);
            byte[] tbuf = null;
            int type = 0;
            tbuf = extractFromQueue(q);
            type = bytesToInt(tbuf);
            int iarrLen = type;

            tbuf = extractFromQueue(q);
            type = bytesToInt(tbuf);
            int larrLen = type;

            tbuf = extractFromQueue(q);
            type = bytesToInt(tbuf);
            int sarrLen = type;

            tbuf = extractFromQueue(q);
            type = bytesToInt(tbuf);
            int farrLen = type;

            for (int i = 0; i < iarrLen; i++) {

                int v = bytesToInt(extractFromQueue(q));
                IArr.Add(v);
            }
            for (int i = 0; i < larrLen; i++)
            {
                long v = bytes2long(extractFromQueue(q));
                LArr.Add(v);
            }
            for (int i = 0; i < sarrLen; i++)
            {
                byte[] v = extractFromQueue2(q);
                string vs = Encoding.UTF8.GetString(v);
                SArr.Add(vs);
            }
            for (int i = 0; i < farrLen; i++)
            {
                byte[] v = extractFromQueue3(q);
                FArr.Add(byteToFloat(v));
            }
            return this;
        }
        

        float byteToFloat(byte[] data)
        {
            return BitConverter.ToSingle(data, 0);
        }

        byte[] float2Byte(float v)
        {
            return BitConverter.GetBytes(v);
        }
        void clear()
        {
            IArr.Clear();
            FArr.Clear();
            SArr.Clear();
        }
        public override string ToString()
        {
            string val = "IArr: ";
            foreach (int k in IArr)
            {
                val += k + " ";
            }
            val += "\nLArr: ";
            foreach (long k in LArr)
            {
                val += k + " ";
            }
            val += "\nFArr: ";
            foreach (float k in FArr)
            {
                val += k + " ";
            }
            val += "\nSArr: ";
            foreach (string k in SArr)
            {
                val += k + " ";
            }

            val += "\n";

            return val;
        }

    }
}
