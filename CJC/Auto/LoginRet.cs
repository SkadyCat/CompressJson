using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class LoginRet:IMsg
{
	public string password;
		public string account;
	
public LoginRet parseJson<LoginRet>(JObject jo)
{
	LoginRet jd= jo.ToObject<LoginRet>();
	return jd;
}

}