using CJ.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Auto
{


	class FileData {

		public string path;
		public Queue<string> content;
		public string name;

		public FileData()
        {
			content = new Queue<string>();
        }
	}
   public class JsonGenerater
    {
        Dictionary<string, string> pathMaps = new Dictionary<string, string>();
        void readConfig(string path) {

            using (StreamReader sr = new StreamReader(path))
            {
                string line = "";
                // 从文件读取并显示行，直到文件的末尾 
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "") {
                        string[] arr = line.Split(' ');
                        pathMaps.Add(arr[0], arr[1].Trim());
                    }
                }
            }
        }
		Queue<string> load(string path)
		{
			Queue<string> q = new Queue<string>();
			using (StreamReader sr = new StreamReader(path))
			{
				string line = "";
				// 从文件读取并显示行，直到文件的末尾 
				while ((line = sr.ReadLine()) != null)
				{
					q.Enqueue(line);
				}
			}
			return q;
		}
		List<FileData> toQueue() {

			List<FileData> ls = new List<FileData>();
			foreach (string key in pathMaps.Keys) {

				
				Queue<string> qq = load(key);
				int line = 0;
				while (qq.Count != 0)
				{
					string k = qq.Dequeue();
					line++;
					if (k == "")
					{
						continue;
					}
					if (k.Contains("message"))
					{
						Queue<string> rq = new Queue<string>();
						FileData data = new FileData();
						data.path = pathMaps[key];
						data.name = k.Replace("message", " ").Trim();
						string head = k.Trim();
						string[] headItems = head.Split(' ');
						rq.Enqueue("public class " + headItems[1].Trim() + ":IMsg\n{\n");
						while (qq.Count != 0)
						{
							line++;
							string msg = qq.Dequeue();
							if (msg == "")
							{
								continue;
							}
							msg = msg.Trim();
							string[] msgItem = msg.Split(' ');
							if (msg == "end")
							{
								break;
							}
							if (msgItem[0] == "int" || msgItem[0] == "float"|| msgItem[0] == "string") {
								rq.Enqueue("	public " + msgItem[0] + " " + msgItem[1] + ";\n	");
							}
							if (msgItem[0] == "repeated")
							{
								if (msgItem.Length < 3)
								{
									string val = "error: please confirm the proto txt file in line " + line + "\n";
									return null;
								}
								rq.Enqueue("public " + msgItem[1] + "[] " + msgItem[2] + ";\n");

							}
						}
						rq.Enqueue("\n");
						rq.Enqueue("public "+data.name+" parseJson<"+ data.name + ">(JObject jo)\n");
						rq.Enqueue("{\n");
						rq.Enqueue("	"+data.name+" jd= jo.ToObject<"+data.name+">();\n");
						rq.Enqueue("	return jd;\n");
						rq.Enqueue("}\n");
						data.content = rq;
						ls.Add(data);
					}
				}
			}
			return ls;
		}


        public void generate(string name)
        {
			readConfig(name);
			List<FileData> ls = toQueue();

			foreach (var v in ls) {

				String FilePath2 = v.path + "\\" + v.name + ".cs";
				System.IO.StreamWriter file2 = new System.IO.StreamWriter(FilePath2, false);
				//保存数据到文件
				file2.Write("using CJ.Auto;\n");
				file2.Write("using System;\n");
				file2.Write("using System.Collections;\n");
				file2.Write("using System.Collections.Generic;\n");
				file2.Write("using Newtonsoft.Json.Linq;\n");
				file2.Write("\n");
				file2.Write("[Serializable]\n");
				while (v.content.Count != 0) {

					file2.Write(v.content.Dequeue());

					
				}
				file2.Write("\n}");
				file2.Close();
			}
         }
    }
}
