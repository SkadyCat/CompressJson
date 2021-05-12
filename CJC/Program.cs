using CJ.Auto;
using CJ.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
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

            
            //byte byData = 0xff;
            //MsgManager manager = new MsgManager();
            //manager.process("message BagGridInfo\nint grid_num\nlong cd\nstring v\nfloat s\nend\n");

            //byte[] buf = manager.Json2Buffer("BagGridInfo", "{\"grid_num\":581,\"cd\":283123123312,\"v\":\"hello world,,,,,,,\",\"s\":23.3333333}");
            //string[] json = manager.Buffer2Json(buf);

            //Console.WriteLine(json[0]);

            //Layer la = manager.serializeToLayer("BagGridInfo", "{\"grid_num\":581,\"cd\":283123123312,\"v\":\"hello world,,,,,,,\",\"s\":23.3333333}");

            //string js = manager.deSerialize(la);

            //Console.WriteLine(la.ToString());
            //Console.WriteLine(js);
            ////Console.WriteLine(n0);
            //int b = 255 * 255 * 255 *128;
            ////Layer la = new Layer("BagGridInfo",manager);

            //la.IArr.Add(45);
            //la.FArr.Add(33.333f);
            //la.LArr.Add(3333333333333);
            //la.LArr.Add(33333233333333);
            //la.LArr.Add(333123337333);
            //la.SArr.Add("HELLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
            //la.SArr.Add("HELLLLLLLLLLLafaLLLLLLLLLLLLLL");
            //la.SArr.Add("HELLLLLLLL213LLLafaLLLLLLLLLLLLLL");
            //byte[] bb = la.toBuffer();
            //Layer ls = new Layer();
            //Console.WriteLine(bb.Length);
            //ls.fromBuffer(bb,manager);

            //Console.WriteLine(la.ToString());
            //Console.WriteLine(ls.ToString());
            ////Console.WriteLine(la.bytesToInt(la.int2bytes(255)));
            ////Console.WriteLine(la.bytesToInt(la.int2bytes(65535)));
            ////Console.WriteLine(la.bytesToInt(la.int2bytes(255*255*255)));
            ////Console.WriteLine(la.bytesToInt(la.int2bytes(b)));

            //for (int i = 0; i < 8; i++) {
            //    Console.WriteLine(la.bytes2long(la.long2bytes((long)Math.Pow(255, i))));
            //    Console.WriteLine((long)Math.Pow(255, i));
            //}
            int bs = 0;
            while (true)
            {
                StreamReader sr = new StreamReader("login.txt");
                MsgManager manager = new MsgManager();
                manager.process(sr.ReadToEnd());
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
                //test(manager, "Test12", "{\"l1\":33333333333333,\"l2\":44444444444444444,\"l3\":[22222222222222222,444444444444444,555555555555555555]}");
                test(manager, "Test14", "{\"a1\":{\"l1\":3333333333333333,\"l2\":44444444444444444},\"a2\":[{\"l1\":33333333353333333,\"l2\":444424444444444444},{\"l1\":33333333333331333,\"l2\":444444414444444444},{\"l1\":33332333333333333,\"l2\":44444444444444444}]}");
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
           
            byte[] bt = new byte[4] { 102, 102, 166, 64};
            float v = BitConverter.ToSingle(bt, 0);

            Console.WriteLine(v);
            Console.WriteLine("");
        }
    }
}
