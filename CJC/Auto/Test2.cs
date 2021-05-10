using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test2:IMsg
{
	public int id;
	public string[] names;

public Test2 parseJson<Test2>(JObject jo)
{
	Test2 jd= jo.ToObject<Test2>();
	return jd;
}

}