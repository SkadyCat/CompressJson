using CJ.Auto;
using CJ.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace CJ_CSHARP
{
    class Program
    {
        static void test(MsgManager manager, string type, string json)
        {
            JObject data = JsonConvert.DeserializeObject<JObject>(json);
            byte[] bf = manager.Json2Buffer(type, json);
            string[] vals = manager.Buffer2Json(bf);
            Console.WriteLine(vals[1]+":"+ vals[0]);
        }
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("F:\\C#Proj\\LCJ\\CompressJson\\test.txt");
            MsgManager manager = new MsgManager();
            manager.process(sr.ReadToEnd());
            test(manager, "Test1", "{\"x\":23.33,\"id\":9527,\"times\":2333333333,\"name\":\"ljy\"}");
            test(manager, "Test2", "{\"times\":[2333333333,233333333333333,2333333333333]}");
            test(manager, "Test3", "{\"id\":23333,\"names\":[\"xyz\",\"cxx\",\"sss\"]}");
            test(manager, "Test4", "{\"name\":\"xyz\",\"codes\":[233,444,555,666,777]}");
            test(manager, "Test5", "{\"list\":[{\"x\":3.33,\"y\":4.44,\"z\":5.55,\"cd\":23333333333333,\"name\":\"xyz\",\"id\":9527},{\"x\":3.33,\"y\":4.44,\"z\":5.55,\"cd\":23333333333333,\"name\":\"xyz\",\"id\":9527}]}");
        }
    }
    
}
