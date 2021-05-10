using CJ.Auto;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[Serializable]
public class LoginRetController : IController
{
    LoginRetModel model;
    public void setData(IMsg msg)
    {
        model = (LoginRetModel)msg;
    }
    public void doing()
    {

        Console.WriteLine(model.info);
    }

}