using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test11:IMsg
{
public float[] arr;

public Test11 parseJson<Test11>(JObject jo)
{
	Test11 jd= jo.ToObject<Test11>();
	return jd;
}

}