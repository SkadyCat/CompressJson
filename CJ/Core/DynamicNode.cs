using System;
using System.Collections.Generic;
using System.Text;

namespace CJ.Core
{
   public class DynamicNode
    {
		public string type;
		public Dictionary<string, int> posMap;
		public DynamicNode() {
			posMap = new Dictionary<string, int>();
			posMap.Add("int", -1);
			posMap.Add("float", -1);
			posMap.Add("string", -1);
		}

		public void copy(Dictionary<string, int> org) {
			foreach (string k in org.Keys) {
				posMap[k] = org[k];
			}
		}
		public int ival;
		public string sval;
		public float fval;
        public long lval;
		public int getIndex()
		{
			if (type == "repeated")
			{
				return posMap["int"];
			}
			return posMap[type];
		}
		public string toVal()
		{
			if (type == "int")
			{
				return ival.ToString();
			}
			if (type == "float")
			{
				return fval.ToString();
			}
			if (type == "string")
			{
				return sval;
			}
			return ival.ToString();
		}
	}
}
