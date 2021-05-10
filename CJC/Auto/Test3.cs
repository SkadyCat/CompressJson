using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class Test3:IMsg
{
	public int id;
	public int[] grid_id;

public Test3 parseJson<Test3>(JObject jo)
{
	Test3 jd= jo.ToObject<Test3>();
	return jd;
}

}