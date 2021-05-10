using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Auto
{
    public abstract class MsgProcess : IMsg
    {

        public void Test<T>(T t)
        {
            Console.WriteLine(t);
        }

        public void process(Type t,object[] data)
        {
            Type type = this.GetType();
            object o = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("process", BindingFlags.Instance | BindingFlags.Public);
            method = method.MakeGenericMethod(t);
            method.Invoke(o, data);
        }

        public virtual void process<T>(T data)
        {
            
        }

        public T parseJson<T>(JObject jo)
        {
            throw new NotImplementedException();
        }
    }
}
