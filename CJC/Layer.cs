using System;
using System.Collections.Generic;
using System.Text;

namespace CJ_CSHARP
{
	public class Layer
	{


		public List<int> IArr;
		public List<float> FArr;
		public List<string> SArr;
        public List<long> LArr;
		public List<byte> bytes;

        public Layer(string type,int id) {
            IArr = new List<int>();
            FArr = new List<float>();
            SArr = new List<string>();
            LArr = new List<long>();
            SArr.Add(type);
            IArr.Add(id);
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
            switch (vv[0]) {
                case 0x00:
                    v = vv[1];
                    break;

                case 0x01:
                    v = vv[1] | vv[2] << 8;
                    break;

                case 0x02:
                    v = vv[1] | vv[2] << 8| vv[3]<<16;
                    break;

                case 0x03:
                    v = vv[1] | vv[2] << 8 | vv[3] << 16|vv[4]<<24;
                    break;

            }
			return v;
		}

        public byte[] int2byte4(int v) {
            byte[] bytes = new byte[4];
            bytes[3] = (byte)(v >> 24);
            bytes[2] = (byte)(v >> 16);
            bytes[1] = (byte)(v >> 8);
            bytes[0] = (byte)v;

            return bytes;
        }

        public byte[] long2bytes(long v) {

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
            if (v < Math.Pow(256, 1)) {
                aim.Add(0x00);
            }
            if (v >= Math.Pow(256, 1) && v< Math.Pow(256, 2))
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
            for (int i = 0; i < aim[0]+1; i++)
            {
                aim.Add(bytes[i]);
            }
            return aim.ToArray();
        }

        public long bytes2long(byte[] buf) {
            int len = buf[0];
            long v = 0;
            for (int i = 1; i <= len+1; i++) {
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
            if (v < 256) {
                aim.Add(0x00);
            }
            if (v>=256&&v<65536)
            {
                aim.Add(0x01);
            }
            if (v >= 65536 && v < (256 * 256 * 256)) {
                aim.Add(0x02);
            }
            if (v >= (256 * 256 * 256)) {
                aim.Add(0x03);
            }
            switch (aim[0]) {
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
            if (v.Length < 256) {
                lb.Add(0x00);
                lb.Add(tc[0]);
            }
            if (v.Length>256&&v.Length<256*256)
            {
                lb.Add(0x01);
                lb.Add(tc[0]);
                lb.Add(tc[1]);
            }
            if (v.Length > 256*256 && v.Length < 256 * 256*256)
            {
                lb.Add(0x02);
                lb.Add(tc[0]);
                lb.Add(tc[1]);
                lb.Add(tc[2]);
            }
            if (v.Length > 256*256*256)
            {
                lb.Add(0x03);
                lb.Add(tc[0]);
                lb.Add(tc[1]);
                lb.Add(tc[2]);
                lb.Add(tc[3]);
            }
            
			for (int i = 0; i < tp.Length; i++)
			{
                lb.Add(tc[i]);
            }
			return lb.ToArray();
		}
		public byte[] toBuffer()
		{
			Queue<byte> q = new Queue<byte>();
			setQueue(q, int2bytes(IArr.Count));
			setQueue(q, int2bytes(FArr.Count));
			setQueue(q, int2bytes(SArr.Count));
            setQueue(q, int2bytes(LArr.Count));

			foreach (var k in IArr)
			{
				setQueue(q, int2bytes(k));
			}
			foreach (var k in FArr)
			{
				setQueue(q, float2Byte(k));
			}
			foreach (var k in SArr)
			{
				setQueue(q, string2Byte(k));
			}
			return q.ToArray();
		}
		byte[] extractFromQueue(Queue<byte> q)
		{
			byte[] cc = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				cc[i] = (q.Dequeue());
			}
			return cc;
		}

		void setQueue(Queue<byte> q, byte[] data)
		{
			foreach (byte k in data)
			{
				q.Enqueue(k);
			}
		}

		public Layer fromBuffer(byte[] buff)
		{
			Queue<byte> q = new Queue<byte>();
			foreach (byte k in buff)
			{
				q.Enqueue(k);
			}

			return convertBuffer(q);
		}
		Layer convertBuffer(Queue<byte> q)
		{
			byte[] _iarrLen = extractFromQueue(q);
			byte[] _farrLen = extractFromQueue(q);
			byte[] _sarrLen = extractFromQueue(q);
			int iarrLen = bytesToInt(_iarrLen);
			int farrLen = bytesToInt(_farrLen);
			int sarrLen = bytesToInt(_sarrLen);
			for (int i = 0; i < iarrLen; i++)
			{
				int v = bytesToInt(extractFromQueue(q));
				IArr.Add(v);
			}
			for (int i = 0; i < farrLen; i++)
			{
				float v = byteToFloat(extractFromQueue(q));
				FArr.Add(v);
			}
			for (int i = 0; i < sarrLen; i++)
			{
				int v = bytesToInt(extractFromQueue(q));
				byte[] buf = new byte[v];
				for (int j = 0; j < v; j++)
				{
					buf[j] = q.Dequeue();
				}
				SArr.Add(Encoding.UTF8.GetString(buf));
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
			foreach (int k in IArr) {
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
