using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test10:IMsg
{
public string[] arr;

public Test10 parseJson<Test10>(JObject jo)
{
	Test10 jd= jo.ToObject<Test10>();
	return jd;
}

}