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
   public class CJReading
    {
        public static Dictionary<string, Type> maps;
        public static Dictionary<string, Type> controllerMaps;
        public static void init() {
            maps = new Dictionary<string, Type>();
            controllerMaps = new Dictionary<string, Type>();
            List<Type> list = ReflectionTool.reflectionByList(typeof(IMsg));
            foreach (Type tp in list) {

                Console.WriteLine(tp.Name);
                maps.Add(tp.Name, tp);
            }
            list = ReflectionTool.reflectionByList(typeof(IController));
            foreach (Type tp in list)
            {

                Console.WriteLine(tp.Name);
                controllerMaps.Add(tp.Name, tp);
            }
        }
        public static void read(byte[] buf) {
            string[] json = CJUtil.toJson(buf);
            string jd = json[0];
            string head = json[1];
            JObject jo = JsonConvert.DeserializeObject<JObject>(jd);
            Type type = maps[head+"Model"];
            object o = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("parseJson", BindingFlags.Instance | BindingFlags.Public);
            method = method.MakeGenericMethod(type);
            object d2 = method.Invoke(o,new object[] {jo});
            type = controllerMaps[head + "Controller"];
            o = Activator.CreateInstance(type);
            method = type.GetMethod("setData", BindingFlags.Instance | BindingFlags.Public);
            method.Invoke(o, new object[] { d2 });
            method = type.GetMethod("doing", BindingFlags.Instance | BindingFlags.Public);
            method.Invoke(o,null);
            //IController controller = new LoginRetController();


            //Console.WriteLine(ret.account);

        }

        static void Main(string[] args) {

            Console.WriteLine("hello world");
        }
        public void send(IMsg msg) { 
            
            
        }
    }
}
