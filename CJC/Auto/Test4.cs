using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test4:IMsg
{
	public float x;
		public float y;
		public float z;
	
public Test4 parseJson<Test4>(JObject jo)
{
	Test4 jd= jo.ToObject<Test4>();
	return jd;
}

}