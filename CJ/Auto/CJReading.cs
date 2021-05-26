using CJ.Core;
using CJ.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Auto
{
   public class CJProcessItem{

       public JObject jo;
        public string head;
    }
   public class CJReading
    {
        public static Dictionary<string, Type> maps;
        public static Dictionary<string, Type> controllerMaps;
        public static Queue<CJProcessItem> jQ;
        public static void init() {
            jQ = new Queue<CJProcessItem>();
            maps = new Dictionary<string, Type>();
            controllerMaps = new Dictionary<string, Type>();
            List<Type> list = ReflectionTool.reflectionByList(typeof(IMsg));
            foreach (Type tp in list) {
                maps.Add(tp.Name, tp);
            }
            list = ReflectionTool.reflectionByList(typeof(IController));
            foreach (Type tp in list)
            {
                controllerMaps.Add(tp.Name, tp);
            }
        }
        public static void read(byte[] buf) {
            string[] json = CJUtil.toJson(buf);
            string jd = json[0];
            string head = json[1];
            JObject jo = JsonConvert.DeserializeObject<JObject>(jd);
            CJProcessItem item = new CJProcessItem();
            item.jo = jo;
            item.head = head;
            jQ.Enqueue(item);
        }

        public static string process()
        {
            CJProcessItem jj = jQ.Dequeue();
            string head = jj.head;
            JObject jo = jj.jo;

            
            Type type = maps[head + "Model"];
            object o = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("parseJson", BindingFlags.Instance | BindingFlags.Public);
            method = method.MakeGenericMethod(type);
            object d2 = method.Invoke(o, new object[] { jo });
            type = controllerMaps[head + "Controller"];
            o = Activator.CreateInstance(type);
            method = type.GetMethod("setData", BindingFlags.Instance | BindingFlags.Public);
            method.Invoke(o, new object[] { d2 });
            method = type.GetMethod("doing", BindingFlags.Instance | BindingFlags.Public);
            method.Invoke(o, null);

            return jo.ToString();
        }
    }
}
