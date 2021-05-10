using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class LoginReqModel:IMsg
{
	public string account;
		public string password;
	
public LoginReqModel parseJson<LoginReqModel>(JObject jo)
{
	LoginReqModel jd= jo.ToObject<LoginReqModel>();
	return jd;
}

}