using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJ.Auto
{
    public interface IController
    {

        void setData(IMsg msg);
        void doing();
    }
}
