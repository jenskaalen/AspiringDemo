using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Saving
{
    public class RuntimeType
    {
        public Type RunType { get; set; }

        public RuntimeType(Type type)
        {
            RunType = type;
        }
    }
}
