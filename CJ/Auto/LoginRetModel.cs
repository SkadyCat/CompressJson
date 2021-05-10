using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Auto
{
    public class LoginRetModel
    {

        public string password;
        public string account;
        public LoginRetModel parseJson<LoginRetModel>(JObject jo)
        {
            LoginRetModel md = jo.ToObject<LoginRetModel>();
            return md;
        }

    }
}
