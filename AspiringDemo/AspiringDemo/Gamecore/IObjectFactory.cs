using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.GameCore
{
    public interface IObjectFactory
    {
        T GetObject<T>() where T : class, new();
    }
}
