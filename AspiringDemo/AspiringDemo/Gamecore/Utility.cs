using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Pathfinding;

namespace AspiringDemo.Gamecore
{
    public static class Utility
    {
        public static T GetRandomNode<T>(this List<T> nodes)
        {
            return nodes[GameFrame.Random.Next(0, nodes.Count)];
        }

        public static IZone GetRandomZone()
        {
            return GameFrame.Game.ZonePathfinder.Nodes[GameFrame.Random.Next(0, GameFrame.Game.ZonePathfinder.Nodes.Count)];
        }
    }
}
