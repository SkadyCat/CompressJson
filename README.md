# CompressJson
its a json compress proto 

## 压缩JSON协议[v1.0 版本]
### 优点
  1. 比json更小的数据量
  2. 内核很小，可以嵌入到任意c#，c++应用
  3. 自动化脚本，一键MVC
  4. 同时支持c++(cmake)支持windows平台和linux平台，c# 支持unity(移动平台和Windows平台)
  
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

## c++ demo
因为存在一些bug，目前正在修复，敬请期待

## 性能报告[压缩性能]
![image](https://user-images.githubusercontent.com/42512280/117945134-98563f00-b340-11eb-8d60-99b4332ab72c.png)

数字越多，则压缩率越高
## 性能报告[1w次序列化与反序列化耗时]
测试代码
```
void test(MsgManager & manager,string type, string v) {
	string buffer = manager.Json2Buffer(type, v);
	vector<string> json = manager.Buffer2Json(buffer);
	string jData = json[0];
	string head = json[1];
}
```

测试数据[0]：
```
test(manager, "Test2", R"({"id":9527,"names":["xm","xw","ss"]})");
```
耗时(s)：
![image](https://user-images.githubusercontent.com/42512280/117945791-42ce6200-b341-11eb-8f96-96a4325b9ecf.png)
约0.7ms

测试数据[1]：
```
test(manager, "Test14", "{\"a1\":{\"l1\":3333333333333333,\"l2\":444444444444444},\"a2\":[{\"l1\":333333333533333,\"l2\":4444244444444444},{\"l1\":333333333331333,\"l2\":4444444144444444},{\"l1\":333323333333333,\"l2\":444444444444444}]}");
```
耗时(s)：
![image](https://user-images.githubusercontent.com/42512280/117946202-a8bae980-b341-11eb-80fe-20700537979d.png)
约2ms

其他类型协议(单次)

![image](https://user-images.githubusercontent.com/42512280/117946509-f3d4fc80-b341-11eb-82fa-b33667e707cc.png)

## 内存使用率
![image](https://user-images.githubusercontent.com/42512280/117948145-6f837900-b343-11eb-91d0-48a17be84ff2.png)

## cpu使用率
![image](https://user-images.githubusercontent.com/42512280/117948502-c38e5d80-b343-11eb-8e20-5e454a9765cb.png)

## 处理器
![image](https://user-images.githubusercontent.com/42512280/117948599-dbfe7800-b343-11eb-8c6a-b1d0f5e90738.png)


## 下个版本将会对序列化和反序列化的时间进行优化
