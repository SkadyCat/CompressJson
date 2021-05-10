using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class LoginRetModel:IMsg
{
	public int code;
		public string info;
	
public LoginRetModel parseJson<LoginRetModel>(JObject jo)
{
	LoginRetModel jd= jo.ToObject<LoginRetModel>();
	return jd;
}

}