using System;
using System.Collections.Generic;
using System.Text;

namespace CJ_CSHARP
{
   public class SerializeNode
    {
		public object val;
		public int size;
		public Dictionary<string, int> maps;
		public SerializeNode next;
		public string type;
		public string key;
		public SerializeNode parent;
		public string dataKey;
		public bool isRepeated;
		public int repeatedNum;
		public bool isDynamic;
		public void setVal(object val)
		{
			this.val = val;
		}
		public object getVal()
		{
			return this.val;
		}

		public string valToString()
		{
			return val.ToString();
		}

		public void initMap() {
			maps = new Dictionary<string, int>();
			maps.Add("int", -1);
			maps.Add("float", -1);
			maps.Add("string", -1);
		}

		
		public SerializeNode(string type, string key, object data)
		{
			initMap();
			this.type = type;
			next = null;
			parent = null;
			this.key = key;
			this.isRepeated = false;
			this.val = data;
			isDynamic = true;
			
		}
		public SerializeNode(SerializeNode node)
		{
			initMap();
			key = node.key;
			type = node.type;
			parent = null;
			next = null;
			val = null;
			maps = node.maps;
			isRepeated = node.isRepeated;
			isDynamic = true;
		}
		public SerializeNode grow(string type, string key, object data)
		{
			SerializeNode nNode = new SerializeNode(type, key, data);
			nNode.parent = this;
			this.next = nNode;
			nNode.maps = this.maps;
			//nNode.maps[type] = this.maps[type] + 1;
			return nNode;
		}

		public SerializeNode copy(SerializeNode node)
		{
			SerializeNode cur = node;
			SerializeNode pt = this;
			SerializeNode tailPoint = null;
			while (cur != null)
			{
				SerializeNode tp = new SerializeNode(cur);
				tp.parent = pt;
				pt.next = tp;
				pt = pt.next;
				tailPoint = cur;
				cur = cur.next;
			}
			return tailPoint;
		}
	}
}
