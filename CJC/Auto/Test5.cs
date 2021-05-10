using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test5:IMsg
{
public Test4[] pos_arr;

public Test5 parseJson<Test5>(JObject jo)
{
	Test5 jd= jo.ToObject<Test5>();
	return jd;
}

}