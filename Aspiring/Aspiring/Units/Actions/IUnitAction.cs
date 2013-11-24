using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Units.Actions
{
    interface IUnitAction
    {
        bool IsFinished { get; set; }
        void Work(long time);
    }
}
