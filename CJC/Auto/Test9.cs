using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test9:IMsg
{
public int[] arr;

public Test9 parseJson<Test9>(JObject jo)
{
	Test9 jd= jo.ToObject<Test9>();
	return jd;
}

}