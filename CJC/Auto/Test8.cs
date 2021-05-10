using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test8:IMsg
{
	public int id;
	public Test4[] pos;

public Test8 parseJson<Test8>(JObject jo)
{
	Test8 jd= jo.ToObject<Test8>();
	return jd;
}

}