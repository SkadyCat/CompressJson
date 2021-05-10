using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Tool
{
    public class ReflectionTool
    {
        public static Dictionary<Type, object> reflection(Type type)
        {


            ArrayList list = new ArrayList();
            Assembly ass = Assembly.GetAssembly(type);
            Dictionary<Type, object> typeDic = new Dictionary<Type, object>();
            Type[] types = ass.GetTypes();
            foreach (Type item in types)
            {
                if (item.IsInterface) continue;//判断是否是接口
                if (item.IsAbstract) continue;//判断是否是抽象类
                Type[] ins = item.GetInterfaces();
                foreach (Type ty in ins)
                {
                    if (ty == type)
                    {
                        list.Add(item);
                    }
                }
            }
            foreach (var v in list)
            {
                var strategy = Activator.CreateInstance((Type)v);
                typeDic.Add((Type)v, strategy);
            }
            return typeDic;
        }
        public static List<Type> reflectionByList(Type type)
        {
            List<Type> typeList = new List<Type>();
            var type2s = AppDomain.CurrentDomain.GetAssemblies()
      .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(type)))
      .ToArray();
            foreach (var v in type2s)
            {

                if (v.IsInterface) continue;//判断是否是接口

                if (v.IsAbstract) continue;//判断是否是抽象类
                Type[] ins = v.GetInterfaces();
                foreach (Type ty in ins)
                {
                    if (ty == type)
                    {
                        typeList.Add(v);
                    }
                }
            }


            return typeList;
        }
    }
}
