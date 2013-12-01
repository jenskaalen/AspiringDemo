using System;
using System.Collections.Generic;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.Zones;

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

        public static double GetDistance(Vector2 position1, Vector2 position2)
        {

            //double dist = Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2));
            double dist = Math.Sqrt((position1.X - position2.X)*(position1.X - position2.X) +
                                    (position1.Y - position2.Y)*(position1.Y - position2.Y));
            return dist; 
        }
    }
}