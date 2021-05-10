using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test6:IMsg
{
	public int id;
	
public Test6 parseJson<Test6>(JObject jo)
{
	Test6 jd= jo.ToObject<Test6>();
	return jd;
}

}