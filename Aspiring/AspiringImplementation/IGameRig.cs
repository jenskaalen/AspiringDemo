using AspiringDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringImplementation
{
    public interface IGameRig
    {
        StandardFactory Factory { get; set; }
        int Worldsize { get; set; }
        int FactionCount { get; set; }
        void RigGame();
    }
}
