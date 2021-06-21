# CompressJson
its a json compress proto 


## 压缩JSON协议[v1.0 版本]
### 优点
  1. 极致压缩，众享丝滑
  2. 内核很小，可以嵌入到任意c#，c++应用
  3. 为支持Unity提供自动化MVC框架插件
  4. 同时支持c++(cmake)支持windows平台和linux平台，c# 支持unity(移动平台和Windows平台)
 
## Unity自动化通信demo教程

暂略
 

## c# Demo 

协议内容test.txt

```proto
message Test1
    float x
    int id
    long times
    string name
end

message Test2
    repeated long times
end
message Test3
    int id
    repeated string names
end

message Test4
    string name
    repeated int codes
end

message Vec3
    float x
    float y
    float z
    long cd
    string name
    int id
end

message Test5
    repeated Vec3 list
end

```

``` c#
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

```
运行结果
![image](https://user-images.githubusercontent.com/42512280/122782877-a7d38b00-d2e3-11eb-8b54-485fa258da4c.png)


## c++使用demo(lua)
协议内容

message PutOnRequest
    string name
    int equip_id
end

```lua
local msg = require "Msg"
local processer = msg.new()
msg.load(processer,"F:\\C#Proj\\LCJ\\CompressJson\\C++Version\\build\\Share\\Debug\\msgProto.txt")
local j1 = '{"name":"ljy","equip_id":222}'
local buf = msg.Json2Buffer(processer,"PutOnRequest",j1)
local jv = msg.Buffer2Json(processer,buf)
```
测试结果
![image](https://user-images.githubusercontent.com/42512280/122783453-292b1d80-d2e4-11eb-83f9-e96b9ab07220.png)
序列化出来的buffer可以直接在c#和c++之间相互通信



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

