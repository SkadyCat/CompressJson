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
						data.path = pathMaps[key]+"\\Model";
                        string hd = k.Replace("message", " ").Trim();

                        data.name = hd + "Model";
						string head = k.Trim();
						string[] headItems = head.Split(' ');
						rq.Enqueue("public class " + data.name + ":IMsg\n{\n");
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
							if (msgItem[0] == "int" || msgItem[0] == "float"|| msgItem[0] == "string" ||msgItem[0] == "long") {
								rq.Enqueue("	public " + msgItem[0] + " " + msgItem[1] + ";\n	");
                                continue;
                            }
                            if (msgItem[0] == "repeated")
                            {
                                if (msgItem.Length < 3)
                                {
                                    string val = "error: please confirm the proto txt file in line " + line + "\n";
                                    return null;
                                }
                                if (msgItem[1] == "int" || msgItem[1] == "float" || msgItem[1] == "string" || msgItem[1] == "long")
                                {
                                    rq.Enqueue("	public " + msgItem[1] + "[] " + msgItem[2] + ";\n	");
                                }
                                else
                                {
                                    rq.Enqueue("public " + msgItem[1] + "Model[] " + msgItem[2] + ";\n");
                                }
                                continue;
                            }
                            rq.Enqueue("	public " + msgItem[0] + "Model " + msgItem[1] + ";\n	");
                        }
						rq.Enqueue("\n");
						rq.Enqueue("public "+data.name+" parseJson<"+ data.name + ">(JObject jo)\n");
						rq.Enqueue("{\n");
						rq.Enqueue("	"+data.name+" jd= jo.ToObject<"+data.name+">();\n");
						rq.Enqueue("	return jd;\n");
						rq.Enqueue("}\n");
						data.content = rq;
						ls.Add(data);
                        if (!hd.Contains("Ret"))
                            continue;
                        //controller
                        rq = new Queue<string>();
                        data = new FileData();
                        data.path = pathMaps[key] + "\\Controller";
                        data.name = k.Replace("message", " ").Trim() + "Controller";
                        rq.Enqueue("public class "+data.name+" : IController\n{\n");
                        rq.Enqueue("        "+ hd + "Model" + " model;\n");
                        rq.Enqueue("        public void setData(IMsg msg)\n");
                        rq.Enqueue("        {\n");
                        rq.Enqueue("            model = ("+ hd+"Model" + ")msg;\n");
                        rq.Enqueue("        }\n");
                        rq.Enqueue("        public void doing()\n");
                        rq.Enqueue("        {\n");
                        rq.Enqueue("        }\n");
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
                
                if (Directory.Exists(v.path) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(v.path);
                }

                
                String FilePath2 = v.path + "\\" + v.name + ".cs";

                if (v.name.Contains("Controller") && File.Exists(FilePath2))
                {
                    continue;
                }
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
