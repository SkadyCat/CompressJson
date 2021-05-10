using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Core
{
   public class CJUtil
    {
		private static MsgManager Instance;
		public static void init()
		{
			Instance = new MsgManager();
		}
		public static void load(string path)
		{
			Instance.process(path);
		}

		public static byte[] toBuffer(string type,string json) {

			return Instance.Json2Buffer(type, json);
		}
		public static string[] toJson(byte[] buf)
		{
			return Instance.Buffer2Json(buf);
		}
	}
}
