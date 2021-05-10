using CJ.Auto;
using CJ.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;

namespace CJ_CSHARP
{
    class Program
    {

        static void test(MsgManager manager,string type, string json) {
            JObject data = JsonConvert.DeserializeObject<JObject>(json);
            byte[] bf = manager.Json2Buffer(type, json);
            string[] vals = manager.Buffer2Json(bf);
            Console.WriteLine(vals[0]+":"+ vals[1]+":"+(vals[0] == json));
        }
        static void Main(string[] args)
        {
            while (true)
            {
                MsgManager manager = new MsgManager();
                manager.process("login.txt");
                test(manager, "LoginReq", "{\"x\":10.33,\"y\":4.22}");
                //test(manager, "Test3", "{\"id\":123,\"grid_id\":[3,4,5,6,7,8]}");
                //test(manager, "Test4", "{\"x\":33.3,\"y\":44.4,\"z\":55.5}");
                //test(manager, "Test5", "{\"pos_arr\":[{\"x\":33.3,\"y\":44.4,\"z\":55.5},{\"x\":313.3,\"y\":424.4,\"z\":535.5},{\"x\":343.3,\"y\":454.4,\"z\":565.5}]}");
                //test(manager, "Test6", "{\"id\":9527,\"pos\":{\"x\":33.3,\"y\":44.4,\"z\":55.5}}");
                //test(manager, "Test7", "{\"id\":3243,\"pos\":{\"x\":3.1,\"y\":4.6,\"z\":9.3},\"pos2\":{\"x\":3.8,\"y\":2.6,\"z\":1.3}}");
                //test(manager, "Test8", "{\"id\":3243,\"pos\":[{\"x\":32.1,\"y\":44.6,\"z\":9.3},{\"x\":31.1,\"y\":4.6,\"z\":92.3},{\"x\":33.1,\"y\":45.6,\"z\":19.3}]}");
                //test(manager, "Test8", "{\"id\":3243,\"pos\":[{\"x\":32.1,\"y\":44.6,\"z\":9.3},{\"x\":31.1,\"y\":4.6,\"z\":92.3},{\"x\":33.1,\"y\":45.6,\"z\":19.3}]}");
                //test(manager, "Test9", "{\"arr\":[1,2,3,4,5,6]}");
                //test(manager, "Test10", "{\"arr\":[\"1\",\"2\",\"3\",\"4\"]}");
                //test(manager, "Test11", "{\"arr\":[1.2,2.3,3.4,4.5]}");
                Thread.Sleep(1);
            }

            //JsonGenerater jg = new JsonGenerater();
            //jg.generate("config.txt");
            //
            //CJUtil.init();
            //CJUtil.load("login.txt");
            //
            //LoginRetModel md = new LoginRetModel();
            //md.code = 9527;
            //md.info = "login success";
            //
            //byte[] buf = CJUtil.toBuffer(md);
            //CJReading.init();
            //CJReading.read(buf);
            //Console.WriteLine(js[0]);

            byte[] b2 = BitConverter.GetBytes(5.2f);
            foreach (byte b in b2) {

                Console.WriteLine(b);
            }
            byte[] bt = new byte[4] { 102, 102, 166, 64};
            float v = BitConverter.ToSingle(bt, 0);

            Console.WriteLine(v);
            Console.WriteLine("");
        }
    }
}
