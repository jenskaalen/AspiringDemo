using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;

namespace AspiringDemoTest
{
    /// <summary>
    /// Contains utility for testing
    /// </summary>
    public static class TestUtil
    {
        /// <summary>
        /// Returns a grid of zones
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size"></param>
        /// <param name="nodeDistance"></param>
        /// <returns></returns>
        public static List<T> GetTestNodes<T>(int size, int nodeDistance) where T : IPathfindingNode, new()
        {
            var list = new List<T>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var node = (T) Activator.CreateInstance(typeof(T));
                    node.Position = new Vector2(i * nodeDistance, j * nodeDistance);
                    list.Add(node);
                }
            }

            return list;
        }
    }
}
