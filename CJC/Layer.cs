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
		public List<byte> bytes;
		public Layer() {
			IArr = new List<int>();
			FArr = new List<float>();
			SArr = new List<string>();
		}
		int bytesToInt(byte[] vv)
		{
			return BitConverter.ToInt32(vv, 0);
		}

		byte[] int2bytes(int v)
		{
			byte[] bytes = new byte[4];
			bytes[3] = (byte)(v >> 24);
			bytes[2] = (byte)(v >> 16);
			bytes[1] = (byte)(v >> 8);
			bytes[0] = (byte)v;
			return bytes;
		}

		byte[] string2Byte(string v)
		{
			byte[] tp = Encoding.UTF8.GetBytes(v);
			byte[] t2 = new byte[tp.Length + 4];
			byte[] t3 = int2bytes(tp.Length);

			for (int i = 0; i < 4; i++)
			{

				t2[i] = t3[i];
			}

			for (int i = 0; i < tp.Length; i++)
			{

				t2[i + 4] = tp[i];
			}
			return t2;
		}
		public byte[] toBuffer()
		{
			Queue<byte> q = new Queue<byte>();

			setQueue(q, int2bytes(IArr.Count));
			setQueue(q, int2bytes(FArr.Count));
			setQueue(q, int2bytes(SArr.Count));
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
