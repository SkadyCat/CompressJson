using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CJ.Core
{
	public class MsgManager
	{


		
		string preProcess;
		Dictionary<string, SerializeNode> headMap;
		Dictionary<string, SerializeNode> serialsNodes;
        public Dictionary<string, string> idMap;
        public Dictionary<int, string> id_sMap;
        public MsgManager() {
			headMap = new Dictionary<string, SerializeNode>();
			serialsNodes = new Dictionary<string, SerializeNode>();
            idMap = new Dictionary<string, string>();
            //id_sMap = new Dictionary<int, string>();
        }
        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(myString);//
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }
            return byte2String;
        }
        bool contains(string origin, string key)
		{
			return origin.Contains(key);
		}
		string[] split(string s, char delimiter)
		{
			return s.Split(delimiter);
		}
		Queue<string> load(string data)
		{
            Queue<string> q = new Queue<string>();

            string[] arr = data.Split('\n');
            foreach (string k in arr)
            {
                q.Enqueue(k);
            }

            return q;
		}

		public void process(string data)
		{

			Queue<string> qq = load(data);
			int line = 0;
			bool isOpen = false;
			while (qq.Count != 0)
			{
				string k = qq.Dequeue();
				line++;
				if (k == "")
				{
					continue;
				}
				if (contains(k, "message"))
				{
					isOpen = true;
					string head = k.Trim();
					string[] headItems = split(head, ' ');
					headMap.Add(headItems[1], new SerializeNode(headItems[0], headItems[1], null));
                    
                    idMap.Add(GetMD5(headItems[1]).Substring(0,10), headItems[1]);
                    SerializeNode cur = headMap[headItems[1]];
					cur.isDynamic = false;
					while (qq.Count != 0)
					{
						line++;
						string msg = qq.Dequeue();
						if (msg == "")
						{
							continue;
						}
						msg = msg.Trim();
						string[] msgItem = split(msg, ' ');
						if (msg == "end")
						{
							isOpen = false;
							cur = cur.grow(msgItem[0], msgItem[0], 0);
							break;
						}

						if (msgItem[0] == "repeated")
						{
							if (msgItem.Length < 3)
							{
								string val = "error: please confirm the proto txt file in line " + line + "\n";
								return;
							}

							cur = cur.grow(msgItem[1], msgItem[2], 0);
							cur.isRepeated = true;
						}
						else
						{
							cur = cur.grow(msgItem[0], msgItem[1], 0);
						}

						if (contains(msg, "message"))
						{
							string val = "error: please confirm the message have end syntax line: " + line + "\n";
						}
						cur.isDynamic = false;
					}


					cur.isDynamic = false;
				}
			}

		}

		void tranverse(string msg)
		{
			SerializeNode cur = headMap[msg];
			while (cur != null)
			{

				cur = cur.next;
			}
		}

		object parseData(string type, string val)
		{
			object data = null;
			if (type == "int")
			{
				data = int.Parse(val.ToString());
			}
			if (type == "float")
			{
				data = float.Parse(val.ToString());
			}
			if (type == "string")
			{
				data = val.ToString();
			}
            if (type == "long") {

                data = long.Parse(val.ToLower());
            }
			return data;
		}
		
		//SerializeNode tail = null;

		SerializeNode serialize(string msg, string jdata, SerializeNode _outSerials,out SerializeNode _tail)
		{
			JObject data = JsonConvert.DeserializeObject<JObject>(jdata);
			SerializeNode outSerials = _outSerials;
			Stack<JContainer> dataStack = new Stack<JContainer>();
			dataStack.Push(data);
			SerializeNode dataNodes = new SerializeNode("message", "head", null);
			outSerials = outSerials.grow("string", "head", msg);
			dataNodes.copy(headMap[msg].next);
			SerializeNode cur = dataNodes.next;
			int index = 0;
			SerializeNode cacheCur = null;

			while (cur != null)
			{
				index++;
				if (!cur.isRepeated)
				{
					if (cur.type == "message")
					{
						string val = (string)cur.getVal();
						dataStack.Push(dataStack.Peek()[val].ToObject<JObject>());
						cur = cur.next;
						continue;
					}
					if (cur.type == "int" || cur.type == "float" || cur.type == "string"||cur.type == "long")
					{
						//JObject jo = .ToObject<JObject>();

						outSerials = outSerials.grow(cur.type, cur.key, parseData(cur.type, dataStack.Peek()[cur.key].ToString()));
						cur.setVal(parseData(cur.type, dataStack.Peek()[cur.key].ToString()));
						cur = cur.next;
						continue;
					}
					if (cur.type == "end")
					{

						if (dataStack.Count > 0)
						{
							dataStack.Pop();
						}
						if (cacheCur != null)
						{

							cur.next = cacheCur;
							cacheCur = null;
						}
						cur = cur.next;
						continue;
					}

					if (contains(cur.type, "repeated"))
					{
						string[] typeClass = split(cur.type, ':');
						string tp = typeClass[1].Trim();
						int len = (int)cur.getVal();
						if (tp == "int" || tp == "float" || tp == "string" || tp == "long")
						{
							for (int i = 0; i < len; i++)
							{
								JArray ja = dataStack.Peek().ToObject<JArray>();
								string vals = ja[i].ToString();
								outSerials = outSerials.grow(tp, "", parseData(tp, vals));
								cur = cur.grow(tp, "", parseData(cur.type, vals));
							}
						}
						else
						{

							for (int i = 0; i < len; i++)
							{
								SerializeNode cache = null;
								JArray ja = dataStack.Peek().ToObject<JArray>();
								JObject jo = ja[i].ToObject<JObject>();
								SerializeNode outs = null;
								cur.next = serialize(tp, jo.ToString(), outSerials, out outs);
								outSerials = outs;
								cur = cur.next;
								
							}

						}

						dataStack.Pop();
						cur.next = cacheCur;
						cur = cur.next;
						cacheCur = null;
						continue;
					}

					string tpKey = cur.key;
					cacheCur = cur.next;
					cur.copy(headMap[cur.type]);
					cur = cur.next;
					cur.type = "message";
					cur.setVal(tpKey);
				}
				else
				{
					JArray arr = dataStack.Peek()[cur.key].ToObject<JArray>();
					dataStack.Push(arr);
					cacheCur = cur.next;
					cur = cur.grow("repeated:" + cur.type, cur.key, arr.Count);
					outSerials = outSerials.grow("int", "repeated", arr.Count);
				}
			}
			_tail = outSerials;
			return dataNodes.next;
		}

		public Queue<DynamicNode> serialize(string msg, string jdata)
		{
			SerializeNode dd = new SerializeNode("head", "msg", 0);
			SerializeNode tail = null;

			SerializeNode head = serialize(msg, jdata, dd, out tail);
			dd = dd.next;

			Dictionary<string, int> posMap = new Dictionary<string, int>();
			posMap.Add("int", -1);
			posMap.Add("float", -1);
			posMap.Add("string", -1);
            posMap.Add("long", -1);
            Queue<DynamicNode> dq = new Queue<DynamicNode>();

            bool addHead = false;
            while (dd != null)
			{
                if (dd.key == "head")
                {
                    if (!addHead)
                    {
                        addHead = true;
                    }
                    else
                    {
                        dd = dd.next;
                        continue;
                    }
                }

                DynamicNode dn = new DynamicNode();
				dn.type = dd.type;
				if (dn.type == "int")
				{
					posMap[dn.type]++;
					dn.ival = (int)dd.getVal();
				}
				if (dn.type == "float")
				{
					posMap[dn.type]++;
					dn.fval = (float)dd.getVal();
				}
				if (dn.type == "string")
				{

                    
					posMap[dn.type]++;
					dn.sval = (string)dd.getVal();
				}
                if (dn.type == "long")
                {
                    posMap[dn.type]++;
                    dn.lval = (long)dd.getVal();
                }
                if (dn.type == "repeated")
				{
					posMap["int"]++;
					dn.ival = (int)dd.getVal();
				}
				dn.copy(posMap);
				dq.Enqueue(dn);
				dd = dd.next;
			}
			return dq;
		}
		public string deSerialize(Layer layer)
		{

			Dictionary<string, int> maps = new Dictionary<string, int>();
			maps.Add("int", 0);
			maps.Add("float", -1);
			maps.Add("string", 0);
            maps.Add("long", -1);
            string protoName = layer.SArr[0];
			SerializeNode head = new SerializeNode("head", "msg", 0);
			head.copy(headMap[protoName].next);
			SerializeNode growData = new SerializeNode("string", "head", layer.SArr[0]);
			SerializeNode thead = growData;
			deSerialize(layer, maps, head, growData);
			JObject json = new JObject();
			string ss = "{";
			while (thead != null)
			{
				if (thead.key == "object_begin")
				{
					ss += "\"" + thead.valToString() + "\":";
					//thead = thead.next;
					ss += deSerialize(thead,out thead);
					if (thead != null)
					{
						ss += ",";
					}
					continue;
				}
				if (thead.key == "end")
				{

					ss += "}";
					if (thead.next != null && thead.next.key != "end")
					{
						ss += ",";
					}
					thead = thead.next;
					break;
				}
				if (thead.key == "head")
				{
					thead = thead.next;
					continue;
				}
				if (thead.key == "arr")
				{
					thead = thead.next;
					continue;
				}
				if (thead.key == "arr_begin")
				{
					ss += "\"" + thead.valToString() + "\":[";
					thead = thead.next;
					while (thead != null && thead.key != "arr_end")
					{

						string preType = thead.type;
						if (preType == "int" || preType == "float" || preType == "long")
						{
							ss += thead.valToString();
							thead = thead.next;
						}
						if (preType == "string")
						{
							ss += "\"" + thead.valToString() + "\"";
							thead = thead.next;
						}
    
						if (headMap.ContainsKey(preType))
						{
							//head = head.next;
							ss += deSerialize(thead,out thead);
						}
						if (thead != null && thead.key != "arr_end")
						{
							ss += ",";
						}
					}
					ss += "]";
					if (thead != null && (thead.key != "end" && thead.key != "arr_end"))
					{
						ss += ",";
					}
					thead = thead.next;//数组的结束arr_end

                    if (thead == null) {
                        continue;
                    }
                    thead = thead.next;//数组对应的对象obj_end结束
                    if (thead.key != "end") {
                        ss += ",";
                    }
					continue;

				}
                

                if (thead.type == "int")
				{

					ss += "\"" + thead.key + "\"" + ":" + thead.valToString();
				}
                if (thead.type == "long")
                {

                    ss += "\"" + thead.key + "\"" + ":" + thead.valToString();
                }
                if (thead.type == "float")
				{
					ss += "\"" + thead.key + "\"" + ":" + thead.valToString();
				}
				if (thead.type == "string")
				{
					ss += "\"" + thead.key + "\"" + ":" + "\"" + thead.valToString() + "\"";
				}
				if (thead.next != null && thead.next.key != "end")
				{
					ss += ",";
				}
				thead = thead.next;
			}
			ss += "}";
			return ss;
		}
		string deSerialize(SerializeNode thead,out SerializeNode end)
		{
			string ss = "{";
			while (thead != null)
			{
				if (thead.key == "object_begin")
				{

					thead = thead.next;
					continue;
				}
				if (thead.key == "object_end")
				{
					thead = thead.next;
					break;
				}
				if (thead.key == "head")
				{
					thead = thead.next;
					continue;
				}
				if (thead.type == "int")
				{
					ss += "\"" + thead.key + "\"" + ":" + thead.valToString();
				}
				if (thead.type == "float")
				{
					ss += "\"" + thead.key + "\"" + ":" + thead.valToString();
				}
				if (thead.type == "long")
				{
					ss += "\"" + thead.key + "\"" + ":" + thead.valToString();
				}
				if (thead.type == "string")
				{
					ss += "\"" + thead.key + "\"" + ":" + "\"" + thead.valToString() + "\"";
				}
				if (thead.next != null && thead.next.key != "object_end")
				{
					ss += ",";
				}
				thead = thead.next;
			}
			ss += "}";
			end = thead;
			return ss;
		}
		SerializeNode deSerialize(Layer layer, Dictionary<string, int> maps, SerializeNode info, SerializeNode growData)
		{
			SerializeNode head = info.next;
			SerializeNode thead = growData;
			SerializeNode lastNodes = null;
			while (head != null)
			{
				if (!head.isRepeated)
				{

					if (head.type == "int")
					{
						maps[head.type]++;
						growData = growData.grow(head.type, head.key, layer.IArr[maps[head.type]]);
						head = head.next;
						continue;
					}
                    if (head.type == "long")
                    {
                        maps[head.type]++;
                        growData = growData.grow(head.type, head.key, layer.LArr[maps[head.type]]);
                        head = head.next;
                        continue;
                    }
                    if (head.type == "float")
					{
						maps[head.type]++;
						growData = growData.grow(head.type, head.key, layer.FArr[maps[head.type]]);
						head = head.next;
						continue;
					}
					if (head.type == "string")
					{
						maps[head.type]++;
						growData = growData.grow(head.type, head.key, layer.SArr[maps[head.type]]);
						head = head.next;
						continue;
					}
					if (head.type == "end")
					{

						if (lastNodes != null)
						{
							growData = growData.grow("string", "object_end", head.key);
							head.next = lastNodes;
							lastNodes = null;
							head = head.next;
							continue;
						}
						else
						{
							break;
						}
					}
					growData = growData.grow("string", "object_begin", head.key);
					lastNodes = head.next;
					head.copy(headMap[head.type].next);
				}
				else
				{
					growData = growData.grow("string", "arr_begin", head.key);
					maps["int"]++;
					int len = layer.IArr[maps["int"]];
					string type = head.type;
					for (int i = 0; i < len; i++)
					{
						if (type == "int")
						{
							maps["int"]++;
							growData = growData.grow("int", "", layer.IArr[maps["int"]]);
							continue;
						}
						if (type == "float")
						{
							maps["float"]++;
							growData = growData.grow("float", "", layer.FArr[maps["float"]]);
							continue;
						}
						if (type == "long")
						{
							maps["long"]++;
							growData = growData.grow("long", "", layer.LArr[maps["long"]]);
							continue;
						}
						if (type == "string")
						{
							maps["string"]++;
							growData = growData.grow("string", "arr_string", layer.SArr[maps["string"]]);
							continue;
						}
						if (headMap.ContainsKey(type))
						{
							lastNodes = head.next;
							head.copy(headMap[type]);
							growData = growData.grow(type, "object_begin", 0);
							head = head.next;
							growData = deSerialize(layer, maps, head, growData);
							growData = growData.grow(type, "object_end", 0);
							head.next = lastNodes;
							continue;
						}
					}
					head = head.next;
					growData = growData.grow("string", "arr_end", head.key);
				}
				head = head.next;
			}

			return growData;
		}
		public string[] Buffer2Json(byte[] buf)
		{
			string[] str = new string[2];
			Layer la = new Layer();
			la.fromBuffer(buf,this);
			str[0] = deSerialize(la);
			str[1] = la.SArr[0];
			return str;
		}
		public byte[] Json2Buffer(string msgType, string json)
		{

			Layer la = serializeToLayer(msgType, json);
			return la.toBuffer();
		}

		public Layer serializeToLayer(string msg, string jdata)
		{
			preProcess = jdata;
			Layer layer = new Layer(msg,this);
            layer.SArr.Clear();

            Queue<DynamicNode> qq = serialize(msg, jdata);
			while (qq.Count != 0)
			{
				if (qq.Peek().type == "int")
				{
					layer.IArr.Add(qq.Peek().ival);
				}
				if (qq.Peek().type == "float")
				{
					layer.FArr.Add(qq.Peek().fval);
				}
				if (qq.Peek().type == "string")
				{
					layer.SArr.Add(qq.Peek().sval);
				}
                if (qq.Peek().type == "long")
                {
                    layer.LArr.Add(qq.Peek().lval);
                }
                qq.Dequeue();
			}
			return layer;
		}


	}
}
