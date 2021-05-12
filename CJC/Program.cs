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
        public static string GetStrMd5_32D(string ConvertString) //32位大写
        {




            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));
            t2 = t2.Replace("-", "");
            return t2;
        }
        
        static void test(MsgManager manager, string type, string json)
        {
            JObject data = JsonConvert.DeserializeObject<JObject>(json);
            //byte[] bf = manager.Json2Buffer(type, json);
           //foreach (var k in bf) {
           //
           //    Console.Write(k + ",");
           //}
           //
           //
            byte[] bf2 = new byte[] { 0, 10, 66, 97, 103, 80, 117, 108, 108, 82, 101, 116, 0, 20, 0, 7, 0, 6, 0, 0, 0, 1, 0, 6, 1, 233, 3, 0, 0, 0, 4, 1, 185, 11, 0, 4, 0, 1, 1, 234, 3, 0, 1, 0, 3, 1, 186, 11, 0, 5, 0, 3, 1, 210, 7, 0, 3, 0, 2, 1, 209, 7, 0, 2, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 214, 239, 155, 96, 0, 11, 66, 97, 103, 71, 114, 105, 100, 73, 110, 102, 111, 0, 11, 66, 97, 103, 71, 114, 105, 100, 73, 110, 102, 111, 0, 11, 66, 97, 103, 71, 114, 105, 100, 73, 110, 102, 111, 0, 11, 66, 97, 103, 71, 114, 105, 100, 73, 110, 102, 111, 0, 11, 66, 97, 103, 71, 114, 105, 100, 73, 110, 102, 111, 0, 11, 66, 97, 103, 71, 114, 105, 100, 73, 110, 102, 111 };
            string[] vals = manager.Buffer2Json(bf2);
            Console.WriteLine(vals[0] + ":" + vals[1] + ":" + (vals[0] == json));
        }
        static void Main(string[] args)
        {
            //50 49 100 50 51 54 52 52 98 97 97 49 48 101 49 101 98 97 55 53 102 100 100 50 57 101 49 55 57 50 56 56
            //33 210 54 68 186 161 14 30 186 117 253 210 158 23 146 136
           // MD5 md5 = System.Security.Cryptography.MD5.Create();
           //
           // byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes("UserInfo");
           //
           // byte[] hash = md5.ComputeHash(inputBytes);
           // 
           // foreach (var k in hash) {
           //
           //     Console.Write(k + " ");
           // }
            // step 2, convert byte array to hex string

           //StringBuilder sb = new StringBuilder();
           //
           //for (int i = 0; i < hash.Length; i++)
           //{
           //    sb.Append(hash[i].ToString("X2"));
           //}
            
            while (true)
            {
                //0,0,0,2,0,0,0,3,0,0,1,55,37,0,3,0,2,120,109,0,2,120,119,0,2,115,115
                //0,0,0,2,0,0,0,3,0,0,1,55,37,0,3,0,2,120,109,0,2,120,119,0,2,115,115
                //注意文件路径 可能直接运行不通过
               StreamReader sr = new StreamReader("F:\\C#Proj\\LCJ\\CompressJson\\test.txt");
               MsgManager manager = new MsgManager();
               manager.process(sr.ReadToEnd());
                test(manager, "Test3", "{\"id\":123,\"grid_id\":[3,4,5,6,7,8]}");
                test(manager, "Test4", "{\"x\":33.3,\"y\":44.4,\"z\":55.5}");
                test(manager, "Test5", "{\"pos_arr\":[{\"x\":33.3,\"y\":44.4,\"z\":55.5},{\"x\":313.3,\"y\":424.4,\"z\":535.5},{\"x\":343.3,\"y\":454.4,\"z\":565.5}]}");
                test(manager, "Test6", "{\"id\":9527,\"pos\":{\"x\":33.3,\"y\":44.4,\"z\":55.5}}");
                test(manager, "Test7", "{\"id\":3243,\"pos\":{\"x\":3.1,\"y\":4.6,\"z\":9.3},\"pos2\":{\"x\":3.8,\"y\":2.6,\"z\":1.3}}");
                test(manager, "Test8", "{\"id\":3243,\"pos\":[{\"x\":32.1,\"y\":44.6,\"z\":9.3},{\"x\":31.1,\"y\":4.6,\"z\":92.3},{\"x\":33.1,\"y\":45.6,\"z\":19.3}]}");
                test(manager, "Test8", "{\"id\":3243,\"pos\":[{\"x\":32.1,\"y\":44.6,\"z\":9.3},{\"x\":31.1,\"y\":4.6,\"z\":92.3},{\"x\":33.1,\"y\":45.6,\"z\":19.3}]}");
                test(manager, "Test9", "{\"arr\":[1,2,3,4,5,6]}");
                test(manager, "Test10", "{\"arr\":[\"1\",\"2\",\"3\",\"4\"]}");
                test(manager, "Test11", "{\"arr\":[1.2,2.3,3.4,4.5]}");
                test(manager, "Test12", "{\"l1\":33333333333333,\"l2\":44444444444444444,\"l3\":[22222222222222222,444444444444444,555555555555555555]}");
                test(manager, "Test14", "{\"a1\":{\"l1\":3333333333333333,\"l2\":44444444444444444},\"a2\":[{\"l1\":33333333353333333,\"l2\":444424444444444444},{\"l1\":33333333333331333,\"l2\":444444414444444444},{\"l1\":33332333333333333,\"l2\":44444444444444444}]}");
                Thread.Sleep(1);
            }
        }
    }
    
}
