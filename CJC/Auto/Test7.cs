using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test7:IMsg
{
	public int id;
	
public Test7 parseJson<Test7>(JObject jo)
{
	Test7 jd= jo.ToObject<Test7>();
	return jd;
}

}