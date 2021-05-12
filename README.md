# CompressJson
its a json compress proto 

## 压缩JSON协议
### 优点
  1. 比json更小的数据量
  2. 内核很小，可以嵌入到任意c#，c++应用
  3. 自动化脚本，一键MVC
  
## c# Demo 
```
        static void test(MsgManager manager, string type, string json)
        {
            JObject data = JsonConvert.DeserializeObject<JObject>(json);
            byte[] bf = manager.Json2Buffer(type, json);
            string[] vals = manager.Buffer2Json(bf);
            Console.WriteLine(vals[0] + ":" + vals[1] + ":" + (vals[0] == json));
        }
        static void Main(string[] args)
        {
            while (true)
            {
                //注意文件路径 可能直接运行不通过
                StreamReader sr = new StreamReader("test.txt");
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
```
