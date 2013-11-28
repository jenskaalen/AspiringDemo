using System.Collections.Generic;

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
            return
                GameFrame.Game.ZonePathfinder.Nodes[GameFrame.Random.Next(0, GameFrame.Game.ZonePathfinder.Nodes.Count)];
        }
    }
}