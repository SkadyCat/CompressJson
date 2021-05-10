using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Auto
{
    public class LoginRetController : IController
    {
        LoginRetModel model;
        public void setData(IMsg msg)
        {
            model = (LoginRetModel)msg;
        }
        public void doing()
        {
            Console.WriteLine("accoutn: "+model.account);
        }

    }
}
